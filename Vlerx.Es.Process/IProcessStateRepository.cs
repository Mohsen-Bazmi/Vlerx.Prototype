using System.Threading.Tasks;

namespace Vlerx.Es.Process
{
    public interface IProcessStateRepository<TState>
        where TState : IProcessState<TState>
    {
        Task<TState> Load(string id);
        Task Save(TState state, long version);
    }
}