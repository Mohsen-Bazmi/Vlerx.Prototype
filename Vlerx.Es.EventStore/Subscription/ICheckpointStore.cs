using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Vlerx.Es.EventStore.Subscription
{
    public interface ICheckpointStore
    {
        Task<Position?> GetCheckpoint();
        Task StoreCheckpoint(Position? checkpoint);
    }
}