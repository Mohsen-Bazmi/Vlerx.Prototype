namespace Vlerx.Es.EventMapping.Serialization
{
    public interface IEventSerdes
        : IEventDeserializer
            , IEventSerializer
    {
    }
}