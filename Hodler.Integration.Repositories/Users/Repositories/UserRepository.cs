using Corz.DomainDriven.Abstractions.DomainEvents;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.ValueObjects;
using Hodler.Integration.Repositories.Users.Context;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Hodler.Integration.Repositories.Users.Entities;
using ApiKey = Hodler.Domain.Users.Models.ApiKey;
using UserSettings = Hodler.Domain.Users.Models.UserSettings;

namespace Hodler.Integration.Repositories.Users.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly UserManager<Entities.User> _userManager;
    private readonly SignInManager<Entities.User> _signInManager;
    private readonly UserDbContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserDbContext dbContext,
        ILogger<UserRepository> logger,
        IDomainEventDispatcher domainEventDispatcher,
        UserManager<Entities.User> userManager,
        SignInManager<Entities.User> signInManager
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
        _userManager = userManager;
        _signInManager = signInManager;
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
            //if (existingEntity is null)
            //{
            //    await _dbContext.AddAsync(newEntity, cancellationToken);
            //}
            //else
            //{
            //    _dbContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            //    _dbContext.Entry(existingEntity).State = EntityState.Modified;
            //}

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

        if (existingEntity is null)
            await _dbContext.AddAsync(userSettings, cancellationToken);
    }


    private IQueryable<Entities.User> IncludeAggregate() =>
        _dbContext.Users
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.UserSettings)
            .Include(x => x.ApiKeys);

    public async Task StoreAsync(IUser aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
        await _domainEventDispatcher.PublishEventsOfAsync(aggregateRoot.DomainEventQueue, cancellationToken);
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

    public async Task<IUser?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);
        if (user is null)
            return null;
        var domainUser = user.Adapt<IUser>();
        //domainUser.AddContactInfo(user.UserName, user.PhoneNumber, user.Email); ;
        return domainUser;
    }

    public async Task<IUser?> FindByUserNameAsync(UserName userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName.Value);
        if (user is null)
            return null;
        var domainUser = user.Adapt<IUser>();
        //domainUser.AddContactInfo(user.UserName, user.PhoneNumber, user.Email);
        return domainUser;
    }

    public async Task<bool> IsExistUserAsync(UserName? userName, EmailAddress email, CancellationToken cancellationToken)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == userName.Value
                               || u.Email == email.Value, cancellationToken);
    }
    public async Task<bool> IsExistUserAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        return await _userManager.Users.AnyAsync(e => e.Email == email.Value, cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(IUser user, string password, CancellationToken cancellationToken)
    {
        var entity = user.Adapt<Entities.User>();
        var identityResult = await _userManager.CreateAsync(entity, password);
        return identityResult;
    }
    public async Task<IUser?> CheckLoginCredentialsAsync(UserName userName, string password, CancellationToken cancellationToken)
    {
        var entity = await _userManager.Users
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .Include(u => u.ApiKeys)
                                        .SingleOrDefaultAsync(u => u.UserName == userName.Value,
                                                                         cancellationToken);
        if (entity is null)
            return null;
        var signInResult = await _signInManager.PasswordSignInAsync(entity, password, false, false);
        return !signInResult.Succeeded ? null : entity.Adapt<IUser>();
    }
    public async Task<IUser?> CheckLoginCredentialsAsync(EmailAddress email, string password, CancellationToken cancellationToken)
    {
        var entity = await _userManager.Users
                                       .AsNoTracking()
                                       .AsSplitQuery()
                                       .Include(u => u.ApiKeys).SingleOrDefaultAsync(u => u.Email == email.Value, cancellationToken);
        if (entity is null)
            return null;
        var signInResult = await _signInManager.PasswordSignInAsync(entity, password, false, false);
        return signInResult.Succeeded ? entity.Adapt<IUser>() : null;
    }
    public async Task<bool> ResetPasswordAsync(EmailAddress email, string newPassword, string token)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }
    public async Task<string> GenerateResetPasswordTokenAsync(EmailAddress email)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    public async Task<string> GenerateConfirmEmailTokenAsync(EmailAddress email)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> ConfirmEmailAsync(EmailAddress email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email.Value);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task StoreRefreshTokenAsync(string refreshToken, DateTime expiryDate, UserId userId , CancellationToken cancellationToken)
    {
        var entity = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId.Value.ToString(),cancellationToken);
        if (entity is not null)
        {
            var usertoken = new UserRefreshTokens()
            {
                Id = Guid.CreateVersion7(),
                UserId = userId.Value.ToString(),
                CreatedTime = DateTime.UtcNow,
                ExpDateTime = expiryDate,
                RefreshToken = refreshToken
            };
            await _dbContext.UserRefreshTokens.AddAsync(usertoken,cancellationToken);
            // await _userManager.SetAuthenticationTokenAsync(entity, "Default", "RefreshToken", refreshToken);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Error while storing user Token with userId ({userId.Value}).");
            }
        }
    }

    public async Task<bool> IsRefreshTokenValidAsync(string refreshToken, UserId userId,CancellationToken cancellationToken)
    {
        return await _dbContext.UserRefreshTokens.AnyAsync(t => t.UserId == userId.Value.ToString() 
                                                                && t.RefreshToken == refreshToken
                                                                && t.ExpDateTime > DateTime.UtcNow
                                                                ,cancellationToken);
    }

    public async Task<bool> UpdateRefreshTokenAsync(string oldRefreshToken, string newRefreshToken,UserId userId, DateTime expiryDate , CancellationToken cancellationToken)
    {
        var userToken =await _dbContext.UserRefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == userId.Value.ToString() && t.RefreshToken == oldRefreshToken
                                 , cancellationToken);
        if (userToken is null)
            return false;
        userToken.RefreshToken = newRefreshToken;
        userToken.ExpDateTime = expiryDate;
        try
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while updating user Token with userId ({userId.Value}).");
            return false;
        }
    }

    public Task<bool> DeleteRefreshTokenAsync(string refreshToken, UserId userId , CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }



}