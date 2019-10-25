using System.Threading.Tasks;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Process
{
    public interface IProcessReactor
    {
        Task ReactTo(EventEnvelope @event);

        bool CanReactTo(IDomainEvent @event);
        // IEnumerable<Type> EventTypes { get; }
    }
}