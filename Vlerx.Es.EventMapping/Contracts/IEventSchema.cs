namespace Vlerx.Es.EventMapping.Contracts
{
    public interface IEventSchema<T>
    {
        void Map(ISchemaBuilder<T> map);
    }
}