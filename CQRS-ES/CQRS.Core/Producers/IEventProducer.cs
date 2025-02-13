using CQRS.Core.Events;

namespace CQRS.Core.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic, T @events) where T : BaseEvent;
    }
}