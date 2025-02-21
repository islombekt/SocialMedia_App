using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;
        public EventSourcingHandler(IEventStore eventStore,IEventProducer eventProducer)
        {
            _eventProducer = eventProducer;
            _eventStore = eventStore;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);
            if(events == null || !events.Any())
            {
                return aggregate;
            }
            aggregate.ReplayEvents(events);
            var latestVersion = events.Select(x => x.Version).Max();
            aggregate.Version = latestVersion;
            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();
            if(aggregateIds == null || !aggregateIds.Any()) return;
            foreach(var aggregateId in aggregateIds)
            {
                Guid aggId = new Guid(aggregateId);
                var aggregate = await GetByIdAsync(aggId);
                if(aggregate == null || !aggregate.Active) continue;
                var events = await _eventStore.GetEventsAsync(aggId);
                foreach(var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProduceAsync(topic,@event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}