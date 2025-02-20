using MassTransit;
using QimiaSchool.DataAccess.MessageBroker.Abstractions;

namespace QimiaSchool.DataAccess.MessageBroker.Implementations;

public sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
}
