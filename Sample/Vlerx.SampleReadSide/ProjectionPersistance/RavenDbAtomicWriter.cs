using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
public class RaventDbAtomicWriter<TKey, TState> : IAtomicWriter<TKey, TState>
{
    IDocumentStore _store;

    public RaventDbAtomicWriter(IDocumentStore store)
    {
        _store = store;
    }
    public async Task AddAsync(TKey key, TState state)
    {
        using (var session = _store.OpenAsyncSession())
        {
            await session.StoreAsync(state, key.ToString());
            await session.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(TKey key, Action<TState> update)
    {
        using (var session = _store.OpenAsyncSession())
        {
            var state = await session.LoadAsync<TState>(key.ToString());
            update(state);
            await session.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(TKey key)
    {
        using (var session = _store.OpenAsyncSession())
            session.Delete(key);
        return Task.CompletedTask;
    }

}