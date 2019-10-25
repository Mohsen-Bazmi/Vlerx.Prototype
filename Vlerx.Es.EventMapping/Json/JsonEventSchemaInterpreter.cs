using System.Text;
using Newtonsoft.Json;
using Vlerx.Es.EventMapping.Context;
using Vlerx.Es.EventMapping.Contracts;
using Vlerx.Es.EventMapping.Data;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;

namespace Vlerx.Es.EventMapping.Json
{
    public class JsonEventSchemaInterpreter : IEventSchemaInterpreter
    {
        public virtual UntypedEventEnvelope Deserialize(ContextSchemaConfigs configs, RawEvent resolvedEvent)
        {
            var jsonMetadata = Encoding.UTF8.GetString(resolvedEvent.Metadata);
            var metadata = JsonConvert.DeserializeObject<UntypedEventEnvelope.EventMetadata>(jsonMetadata);

            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Body);
            var dataType = configs.GetType(resolvedEvent.EventType);
            var data = JsonConvert.DeserializeObject(jsonData, dataType);

            return new UntypedEventEnvelope(resolvedEvent.EventId
                , metadata
                , (IDomainEvent) data);
        }

        public virtual RawEvent Serialize(ContextSchemaConfigs configs, UntypedEventEnvelope change)
        {
            return new RawEvent(
                configs.GetTypeName(change.Body.GetType())
                , change.Id
                , Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(change.Metadata))
                , Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(change.Body)));
        }
    }
}