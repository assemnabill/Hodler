using System.Diagnostics;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using Hodler.Integration.Repositories.Users.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.Users.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserDbContext dbContext,
        ILogger<UserRepository> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    private async Task SaveChangesAsync(IUser aggregateRoot, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregateRoot);

        cancellationToken.ThrowIfCancellationRequested();

        if (_dbContext.Database.CurrentTransaction is null)
            _logger.LogWarning
            (
                $"No active database transaction found while storing user ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}."
            );

        try
        {
            var existingEntity = await _dbContext.Users
                .Include(x => x.UserSettings)
                .Include(x => x.ApiKeys)
                .FirstOrDefaultAsync(t => t.Id == aggregateRoot.Id.Value.ToString(), cancellationToken);

            var newEntity = aggregateRoot.Adapt<IUser, Entities.User>();
            if (existingEntity is null)
            {
                await _dbContext.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                _dbContext.Entry(existingEntity).State = EntityState.Modified;
            }

            await ChangeUserSettings(aggregateRoot, newEntity, cancellationToken);
            await ChangeUserApiKeys(aggregateRoot, newEntity, cancellationToken);

            var rows = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while storing user ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}.");
        }
    }

    private async Task ChangeUserApiKeys(IUser aggregateRoot, Entities.User entity, CancellationToken cancellationToken)
    {
        var existingEntities = _dbContext.ApiKeys
            .Where(x => x.UserId == entity.Id)
            .ToList();

        var newEntities = aggregateRoot.ApiKeys
            .Select(x => x.Adapt<ApiKey, Entities.ApiKey>());

        foreach (var newEntity in newEntities)
        {
            var existingEntity = existingEntities
                .FirstOrDefault(x => x.ApiKeyId == newEntity.ApiKeyId);

            if (existingEntity is null)
            {
                await _dbContext.AddAsync(newEntity, cancellationToken);
            }
        }
    }

    private async Task ChangeUserSettings(
        IUser aggregateRoot,
        Entities.User entity,
        CancellationToken cancellationToken
    )
    {
        var existingEntity = _dbContext.UserSettings
            .FirstOrDefault(x => x.UserId == entity.Id);

        var userSettings = aggregateRoot.UserSettings.Adapt<UserSettings, Entities.UserSettings>();

        if (existingEntity is null && userSettings is not null)
        {
            await _dbContext.AddAsync(userSettings, cancellationToken);
        }
    }


    private IQueryable<Entities.User> IncludeAggregate()
    {
        return _dbContext.Users
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.UserSettings)
            .Include(x => x.ApiKeys);
    }

    public async Task StoreAsync(IUser aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
        aggregateRoot.OnAfterStore();
    }

    public async Task<IUser?> FindByAsync(
        UserId userId,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var entity = await IncludeAggregate()
            .Where(x => x.Id == userId.Value.ToString())
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.Adapt<Entities.User, IUser>();
    }
}