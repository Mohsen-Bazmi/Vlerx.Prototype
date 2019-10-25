using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.EventMapping.Serialization
{
    public interface IEventDeserializer
    {
        UntypedEventEnvelope Deserialize(RawEvent resolvedEvent);
    }
}