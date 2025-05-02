using Corz.DomainDriven.Abstractions.DomainEvents;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Users.Events;

public class UserDisplayCurrencyChanged : DomainEventBase<User>
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