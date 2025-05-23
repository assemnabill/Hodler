using Hodler.Domain.Shared.Events;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Users.Events;

public class UserDisplayCurrencyChanged : DomainEventBase
{
    public FiatCurrency NewDisplayCurrency { get; }

    public UserDisplayCurrencyChanged(
        FiatCurrency newDisplayCurrency,
        DateTimeOffset? occurredOn = null
    ) : base(occurredOn)
    {
        NewDisplayCurrency = newDisplayCurrency;

    }
}