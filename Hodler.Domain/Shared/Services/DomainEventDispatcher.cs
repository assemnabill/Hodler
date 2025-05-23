using Hodler.Domain.Shared.Events;

namespace Hodler.Domain.Shared.Services;

internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // todo: implement transactional outbox pattern
        var domainEventType = domainEvent.GetType();
        // var serviceType = typeof(IRequestHandler<>).MakeGenericType(domainEventType);
        // var services = _serviceProvider.GetServices(serviceType);
        //
        // foreach (var service in services)
        // {
        //     cancellationToken.ThrowIfCancellationRequested();
        //
        //     if (service != null)
        //         await ExecuteHandler(serviceType, service, domainEvent, cancellationToken);
        // }

    }

    public async Task PublishEventsOfAsync(
        DomainEventQueue queue,
        CancellationToken cancellationToken = default
    )
    {
        while (queue.TryDequeueDomainEvent(out var domainEvent))
        {
            if (domainEvent != null)
                await PublishAsync(domainEvent, cancellationToken);
        }
    }

    private Task ExecuteHandler(
        Type serviceType,
        object service,
        IDomainEvent domainEvent,
        CancellationToken cancellationToken
    )
    {
        var method = serviceType.GetMethod("HandleAsync");

        if ((object)method == null)
            throw new InvalidOperationException("method HandleAsync not found");

        if (!(method.Invoke(service, [domainEvent, cancellationToken]) is Task task))
            throw new InvalidOperationException("HandleAsync returned null");

        return task;
    }
}