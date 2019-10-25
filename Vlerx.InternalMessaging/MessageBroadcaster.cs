using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Vlerx.InternalMessaging
{
    public class MessageBroadcaster<TMessage> : IMessenger<TMessage>
    {
        readonly IServiceProvider _services;

        protected MessageBroadcaster(IServiceProvider services
            , Func<TMessage, object> getPayload = null)
        {
            _services = services;
            _getPayload = getPayload ?? (message => message);
        }


        public Task Dispatch(TMessage message)
        {
            var payload = GetPayload(message);
            var listeners = FindListenersOf(payload);
            return On(listeners, payload);
        }

        protected virtual Task On(IEnumerable<object> listeners, object payload)
            => Task.WhenAll(listeners.Select(listener => (Task)
                ((dynamic) listener).On((dynamic) payload)));

        IEnumerable<object> FindListenersOf(object payload)
        {
            var listenerType = typeof(IListenTo<>).MakeGenericType(payload.GetType());
            return _services.GetServices(listenerType);
        }

        public static IMessenger<TMessage> Subscribe(IServiceProvider services
            , Func<TMessage, object> getPayload = null)
            => new MessageBroadcaster<TMessage>(services, getPayload);


        readonly Func<TMessage, object> _getPayload;

        protected virtual object GetPayload(TMessage message)
            => _getPayload(message);
    }
}