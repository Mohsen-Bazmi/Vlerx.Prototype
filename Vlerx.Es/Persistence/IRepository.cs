using System.Threading.Tasks;

namespace Vlerx.Es.Persistence
{
    public interface IRepository<TState>
        where TState : class
    {
        Task<TState> Load(UnitOfWork<TState> uow, string id);
        Task Save(UnitOfWork<TState> uow, long version);
    }
}