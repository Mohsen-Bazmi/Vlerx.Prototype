using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.Messaging;
using Vlerx.InternalMessaging;

namespace Vlerx.Es.Process
{
    public class ProcessMessenger : IMessenger<EventEnvelope>
    {
        private readonly IProcessReactor[] _processes;

        public ProcessMessenger(params IProcessReactor[] processes)
        {
            _processes = processes;
        }

        public Task Dispatch(EventEnvelope @event)
        {
            return ReactTo(@event);
        }

        private Task ReactTo(EventEnvelope @event)
        {
            var process = _processes.FirstOrDefault(p => p.CanReactTo(@event.Payload));
            return null == process
                ? Task.CompletedTask
                : process.ReactTo(@event);
        }
    }
}