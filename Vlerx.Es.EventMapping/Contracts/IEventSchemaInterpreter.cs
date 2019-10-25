using Vlerx.Es.EventMapping.Context;
using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.EventMapping.Contracts
{
    public interface IEventSchemaInterpreter
    {
        UntypedEventEnvelope Deserialize(ContextSchemaConfigs configs, RawEvent resolvedEvent);
        RawEvent Serialize(ContextSchemaConfigs configs, UntypedEventEnvelope change);
    }
}