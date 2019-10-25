using Vlerx.Es.EventMapping.Context;

namespace Vlerx.Es.EventMapping.Json
{
    public abstract class JsonEventSchemaContext : EventSchemaContext
    {
        public JsonEventSchemaContext()
            : base(new JsonEventSchemaInterpreter())
        {
        }
    }
}