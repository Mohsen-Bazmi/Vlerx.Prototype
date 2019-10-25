namespace Vlerx.Es.Process
{
    public interface IProcessState<TState>
    {
        string Id { get; }
        long Version { get; }
    }
}