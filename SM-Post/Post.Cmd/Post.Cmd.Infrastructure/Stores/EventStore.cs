using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;
        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<string>> GetAggregateIdsAsync()
        {
            var eventStream = await _eventStoreRepository.FindAllAsync();
            if(eventStream == null || !eventStream.Any())
            {
                throw new ArgumentNullException(nameof(eventStream),"Could not retrieve event from the event store");
            }
            return eventStream
                        .Select(x => x.AggregateIdentifier)
                        .Distinct()
                        .ToList();
            
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
            if(eventStream == null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect post Id provided!");
            }
            return eventStream.OrderBy(c => c.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
            if(expectedVersion != -1 && eventStream[^1].Version != expectedVersion) // ^1 is the same as lenght-1 so last element
            {
                throw new ConcurencyException();
            }
            var version = expectedVersion;
            foreach(var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModle = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId.ToString(),
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };
                await _eventStoreRepository.SaveAsync(eventModle);

                // send event to Kafka topic
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC"); // this topic name is on launch.json file in .vscode, 
                //on production we can save those data not in launch set. but on k8s or docker deployment files
                // if mongo db has set with replicas, then save those event on mongo db with transaction, like if mongo or kafka fails to save or send that event, 
                // then transaction will be reversed and makes it like no actions or events were performed 
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}