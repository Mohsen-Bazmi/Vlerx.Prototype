using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.EventMapping.Serialization
{
    public interface IEventSerializer
    {
        RawEvent Serialize(UntypedEventEnvelope change);
    }
}