using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Vlerx.InternalMessaging
{
    public class Requester<TMessage> : IRequester<TMessage>
    {
        readonly IServiceProvider _services;

        protected Requester(IServiceProvider services)
        => _services = services;


        public TResponse Get<TResponse>(TMessage message)
        {
            var payload = message;
            var listeners = FindListenersOf<TResponse>(payload);
            return On<TResponse>(listeners, payload);
        }

        protected virtual TResponse On<TResponse>(IEnumerable<object> listeners, object payload)
            => (TResponse)((dynamic)listeners.First()).On((dynamic)payload);

        IEnumerable<object> FindListenersOf<TResponse>(object payload)
        {
            var listenerType = typeof(IRespondTo<,>).MakeGenericType(payload.GetType(), typeof(TResponse));
            return _services.GetServices(listenerType);
        }

        public static Requester<TMessage> Subscribe(IServiceProvider services)
            => new Requester<TMessage>(services);


    }
}