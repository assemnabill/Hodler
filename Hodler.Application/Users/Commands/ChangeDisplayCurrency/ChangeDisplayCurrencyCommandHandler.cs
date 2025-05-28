using Hodler.Domain.Users.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Users.Commands.ChangeDisplayCurrency;

public class ChangeDisplayCurrencyCommandHandler(IServiceScopeFactory serviceScopeFactory)
    : IRequestHandler<ChangeDisplayCurrencyCommand, bool>
{
    public async Task<bool> Handle(ChangeDisplayCurrencyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.UserId);
        ArgumentNullException.ThrowIfNull(request.FiatCurrency);

        using var scope = serviceScopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var domainService = serviceProvider.GetRequiredService<IUserSettingsService>();

        var result = await domainService.ChangeDisplayCurrencyAsync(
            request.UserId,
            request.FiatCurrency,
            cancellationToken
        );

        return result;
    }
}