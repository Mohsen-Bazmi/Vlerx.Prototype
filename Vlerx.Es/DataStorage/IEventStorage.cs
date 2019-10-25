using System.Collections.Generic;
using System.Threading.Tasks;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.DataStorage
{
    public interface IEventStorage
    {
        Task AppendEvents(string streamName
            , long version
            , UntypedEventEnvelope[] changes);

        Task<IEnumerable<UntypedEventEnvelope>> ReadEvents(string streamName);
        Task<bool> Exists(string streamName);
    }
}