using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Users.Commands.ChangeDisplayCurrency;

public class ChangeDisplayCurrencyCommand : IRequest<bool>
{
    public UserId UserId { get; }
    public FiatCurrency FiatCurrency { get; }

    public ChangeDisplayCurrencyCommand(UserId userId, FiatCurrency fiatCurrency)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        FiatCurrency = fiatCurrency;
    }
}