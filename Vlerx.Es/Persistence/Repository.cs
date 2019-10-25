using System;
using System.Linq;
using System.Threading.Tasks;
using Vlerx.Es.DataStorage;
using Vlerx.Es.DomainModel;
using Vlerx.Es.Messaging;

namespace Vlerx.Es.Persistence
{
    public class Repository<TState> : IRepository<TState>
        where TState : class, IAggregateState<TState>
    {
        private readonly IEventStorage _store;

        public Repository(IEventStorage store)
        {
            _store = store;
        }

        public Task<TState> Load(UnitOfWork<TState> unitOfWork, string id)
        {
            return ReadAndFold(id
                , unitOfWork.State
                , (state, @event) => state.ApplySingle(state, @event));
        }

        // static string GetStreamName(AggregateState<TState> state)
        // => state.GetType() + state.Id
        public Task Save(UnitOfWork<TState> unitOfWork, long version)
        {
            return _store.AppendEvents(unitOfWork.State.StreamName
                , version
                , unitOfWork.Changes);
        }

        //TODO: Deduplicate by command id in metadata
        private async Task<TState> ReadAndFold(string id, TState state, Func<TState, IDomainEvent, TState> when)
        {
            var streamName = state.StreamNameForId(id);
            var events = await _store.ReadEvents(streamName);
            return events.Select(e => e.Body)
                .Aggregate(state, when);
        }


        // public async Task Update(Func<TState, UnitOfWork<TState>> update
        //                         , string id
        //                         , long expectedVersion)
        // => await Save(update(await Load(id)), expectedVersion);
    }
}