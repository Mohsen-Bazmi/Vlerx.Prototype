namespace Vlerx.Es.EventMapping.Contracts
{
    public class SchemaFor<T> : IEventSchema<T>
    {
        private readonly string _eventName;

        protected SchemaFor()
        {
            _eventName = typeof(T).FullName;
        }

        public SchemaFor(string eventName)
        {
            _eventName = eventName;
        }

        public virtual void Map(ISchemaBuilder<T> type)
        {
            type.Name(_eventName);
        }
    }
}