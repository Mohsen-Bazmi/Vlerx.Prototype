using System;
using System.Threading.Tasks;
public interface IAtomicWriter<in TKey, TState>
{
    Task AddAsync(TKey key, TState state);
    Task UpdateAsync(TKey key, Action<TState> update);
    Task DeleteAsync(TKey key);
}