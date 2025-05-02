using Hodler.Domain.Users.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Users.Commands.ChangeDisplayCurrency;

public class ChangeDisplayCurrencyCommandHandler : IRequestHandler<ChangeDisplayCurrencyCommand, bool>
{
    private readonly IServiceProvider _serviceProvider;

    public ChangeDisplayCurrencyCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> Handle(ChangeDisplayCurrencyCommand request, CancellationToken cancellationToken)
    {
        var domainService = _serviceProvider.GetRequiredService<IUserSettingsService>();

        var result = await domainService.ChangeDisplayCurrencyAsync(
            request.UserId,
            request.FiatCurrency,
            cancellationToken
        );

        return result;
    }
}