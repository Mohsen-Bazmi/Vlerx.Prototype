using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Vlerx.Es.Messaging;

namespace Vlerx.InternalMessaging
{
    public sealed class EventBroadcaster : IMessenger<EventEnvelope>
    {
        //        private readonly IDictionary<Type, IListener[]> _listeners;
        //
        //        private EventBroadcaster(params IListener[] listeners)
        //        {
        //            _listeners = listeners
        //                .SelectMany(listener =>
        //                    listener.GetType()
        //                        .GetInterfaces()
        //                        .Where(i => i.IsGenericType
        //                                    && i.GetGenericTypeDefinition() == typeof(IListenTo<>))
        //                        .Select(i => (EventType: i.GetGenericArguments().Single()
        //                            , ListenerInstance: listener)))
        //                .GroupBy(e => e.EventType)
        //                .ToDictionary(g => g.Key
        //                    , g => g.Select(v => v.ListenerInstance).ToArray());
        //        }
        private readonly IServiceProvider _services;

        private EventBroadcaster(IServiceProvider services)
        {
            _services = services;
        }

        object GetPayload(EventEnvelope message)
            => message.Payload;
        public Task Dispatch(EventEnvelope message)
        {
            var payload = GetPayload(message);
            var listenerType = typeof(IListenTo<>).MakeGenericType(payload.GetType());
            var listeners = _services.GetServices(listenerType);
            return Task.WhenAll(listeners.Select(
                listener => (Task) ((dynamic) listener).On((dynamic) payload)));
            //            DispatchT((dynamic) message);
        }

        public static IMessenger<EventEnvelope> Subscribe(IServiceProvider services)
        {
            return new EventBroadcaster(services);
        }


        //        private void DispatchT<T>(T message)
        //        {
        //            if (_listeners.TryGetValue(typeof(T), out var listeners))
        //                Task.WaitAll(listeners.Select(l => ((IListenTo<T>) l).As(message)).ToArray());
        //        }
    }
}