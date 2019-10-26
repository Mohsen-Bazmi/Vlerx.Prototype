using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Serilog;
using Serilog.Events;
using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.EventMapping.Serialization;
using Vlerx.Es.Messaging;
using Vlerx.InternalMessaging;
using ILogger = Serilog.ILogger;

namespace Vlerx.Es.EventStore.Subscription
{
    public class SubscriptionIntegrator
    {
        private static readonly ILogger _log = Log.ForContext<SubscriptionIntegrator>();

        private readonly ICheckpointStore _checkpointStore;
        private readonly IEventStoreConnection _connection;

        private readonly IEventDeserializer _deserializer;
        private readonly IOnewayAsyncMessenger<EventEnvelope> _messenger;

        private readonly string _subscriptionName;
        // EventStoreAllCatchUpSubscription _subscription;

        public SubscriptionIntegrator(IEventStoreConnection connection
            , ICheckpointStore checkpointStore
            , string subscriptionName
            , IOnewayAsyncMessenger<EventEnvelope> messenger
            , IEventDeserializer deserializer)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _subscriptionName = subscriptionName;
            _messenger = messenger;
            _deserializer = deserializer;
        }

        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(
                2000
                , 500
                , _log.IsEnabled(LogEventLevel.Debug)
                , false);

            _log.Information("Starting the subscription manager...");

            var position = await _checkpointStore.GetCheckpoint();

            _log.Information("Retrieved the checkpoint: {checkpoint}", position);

            var _subscription = _connection.SubscribeToAllFrom(position
                , settings
                , EventAppeared
                , LiveProcessingStarted
                , SubscriptionDropped);

            // var _subscription = _connection.SubscribeToStreamFrom("$all"
            //                                                         ,position.Value.CommitPosition
            //                                                         , settings
            //                                                         , EventAppeared
            //                                                         , LiveProcessingStarted
            //                                                         , SubscriptionDropped);

            _log.Information("Subscribed to $all stream");
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription subscription
            , SubscriptionDropReason reason
            , Exception exception)
        {
            _log.Warning(exception
                , "Subscription {subscription} dropped because of {reason}"
                , _subscriptionName
                , reason);

            if (reason != SubscriptionDropReason.UserInitiated)
                Task.Run(Start);
        }

        private void LiveProcessingStarted(EventStoreCatchUpSubscription obj)
        {
            _log.Information("Live processing started for {subscription}", _subscriptionName);
        }

        private Task EventAppeared(EventStoreCatchUpSubscription _
            , ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return Task.CompletedTask;

            var @event = new RawEvent(resolvedEvent.Event.EventType
                , resolvedEvent.Event.EventId
                , resolvedEvent.Event.Metadata
                , resolvedEvent.Event.Data);

            return On(@event, resolvedEvent.OriginalPosition, resolvedEvent.Event.Created);
        }

        private Task On(RawEvent rawEvent, Position? position, DateTime eventCreationTime)
        {
            var @event = _deserializer.Deserialize(rawEvent);

            _log.Debug("Projecting event {event}", rawEvent.Body.ToString());
            try
            {
                // return Measure(() =>// Should be moved to a messenger decorator
                //     {
                var envelopedEvent = new EventEnvelope(
                    new MessageChainInfo(@event.Id.ToString()
                        , @event.Metadata.CorrelationId
                        , @event.Metadata.CausationId)
                    , @event.Body);
                _messenger.Dispatch(envelopedEvent);
                return _checkpointStore.StoreCheckpoint(position);
                // }, rawEvent.EventType
                // , eventCreationTime
                // , _subscriptionName);
            }
            catch (Exception exception)
            {
                _log.Error(exception
                    , "Error occured when projecting the event {event} from {subscriptionName}"
                    , @event
                    , _subscriptionName);
                throw;
            }
        }

        // static Task Measure(Func<Task> project
        //     , string eventType
        //     , DateTime eventCreationTime
        //     , string subscriptionName)
        // {
        //     await PrometheusMetrics.Measure(project
        //         , PrometheusMetrics.SubscriptionTimer(subscriptionName));
        //     PrometheusMetrics.ObserveLeadTime(eventType
        //         , eventCreationTime
        //         , subscriptionName);
        // }

        // Task Project(EnvelopedUntypedEvent @event, Position? position)
        // {
        //     _bus.Dispatch(@event.Body);
        //     return _checkpointStore.StoreCheckpoint(position);
        // }
    }
}

// public class EventStoreSubscription : IConnect
// {
//     internal interface IConnect
//     {

//     }
//     internal interface ICheckpoint
//     {

//     }
//     readonly IEventStoreConnection _connection;
//     readonly ICheckpointStore _checkpointStore;
//     readonly string _subscriptionName;
//     readonly IEventBroadcaster _bus;
//     public EventStoreSubscription(IEventStoreConnection connection
//                                 , ICheckpointStore checkpointStore
//                                 , string subscriptionName
//                                 , IEventBroadcaster bus)
//     {
//         _connection = connection;
//         _checkpointStore = checkpointStore;
//         _subscriptionName = subscriptionName;
//         _bus = bus;
//     }

//     public EventStoreSubscription To(IEventStoreConnection connection)
//     => new EventStoreSubscription(connection,
//                                   _checkpointStore,
//                                   _subscriptionName,
//                                   _bus);

// }