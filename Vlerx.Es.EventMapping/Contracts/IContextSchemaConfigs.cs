namespace Vlerx.Es.EventMapping.Contracts
{
    public interface IContextSchemaConfigs
    {
        void Add<T>(IEventSchema<T> map);
    }
}