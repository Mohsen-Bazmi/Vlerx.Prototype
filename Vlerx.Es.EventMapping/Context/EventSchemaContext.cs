using Vlerx.Es.EventMapping.Contracts;
using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.EventMapping.Serialization;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.EventMapping.Context
{
    public abstract class EventSchemaContext : IEventSerdes
    {
        protected readonly ContextSchemaConfigs _configs = new ContextSchemaConfigs();
        private readonly IEventSchemaInterpreter _serializationInterpreter;

        public EventSchemaContext(IEventSchemaInterpreter serializationInterpreter)
        {
            OnModelCreating(_configs);
            _serializationInterpreter = serializationInterpreter;
        }

        public UntypedEventEnvelope Deserialize(RawEvent e)
        {
            return _serializationInterpreter.Deserialize(_configs, e);
        }

        public RawEvent Serialize(UntypedEventEnvelope change)
        {
            return _serializationInterpreter.Serialize(_configs, change);
        }

        public abstract void OnModelCreating(IContextSchemaConfigs configs);
    }
}