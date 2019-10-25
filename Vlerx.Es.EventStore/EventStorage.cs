using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Serilog;
using Vlerx.Es.DataStorage;
using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.EventMapping.Serialization;
using Vlerx.Es.Persistence;
using ILogger = Serilog.ILogger;

namespace Vlerx.Es.EventStore
{
    public class EventStorage : IEventStorage
    {
        private static readonly ILogger _log = Log.ForContext<EventStorage>();
        private readonly IEventStoreConnection _connection;
        private readonly IEventSerdes _serdes;

        public EventStorage(IEventStoreConnection connection
            , IEventSerdes serdes)
        {
            _connection = connection;
            _serdes = serdes;
        }

        public Task AppendEvents(string streamName
            , long version
            , UntypedEventEnvelope[] events)
        {
            foreach (var @event in events)
                _log.Debug($"Persisting event {@event.Body}");


            if (events == null || !events.Any()) return Task.CompletedTask;

            var preparedEvents = events
                .Select(ConvertEventEnvelopeToEventData).ToArray();

            return _connection.AppendToStreamAsync(
                streamName,
                version,
                preparedEvents
            );

            // return _connection.AppendEvents(
            //     streamName, version, events
            // );
        }

        public async Task<IEnumerable<UntypedEventEnvelope>> ReadEvents(string streamName)
        {
            return (await ReadToEnd(streamName))
                .Select(resolvedEvent => _serdes.Deserialize(
                    new RawEvent(resolvedEvent.Event.EventType
                        , resolvedEvent.Event.EventId
                        , resolvedEvent.Event.Metadata
                        , resolvedEvent.Event.Data)));
        }


        public async Task<bool> Exists(string streamName)
        {
            var result = await _connection.ReadEventAsync(streamName, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        private EventData ConvertEventEnvelopeToEventData(UntypedEventEnvelope @event)
        {
            var raw = _serdes.Serialize(@event);
            return new EventData(
                raw.EventId,
                raw.EventType,
                true,
                raw.Body,
                raw.Metadata);
        }

        private async Task<IEnumerable<ResolvedEvent>> ReadToEnd(string streamName)
        {
            var result = new List<ResolvedEvent>();
            var position = 0;
            const int sliceSize = 4096;
            while (true)
            {
                var slice = await _connection.ReadStreamEventsForwardAsync(
                    streamName, position, sliceSize, false
                );
                result.AddRange(slice.Events);
                if (slice.IsEndOfStream) break;
                position += sliceSize;
            }

            return result;
        }
    }
}