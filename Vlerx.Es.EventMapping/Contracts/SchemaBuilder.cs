namespace Vlerx.Es.EventMapping.Contracts
{
    internal class SchemaBuilder<T> : ISchemaBuilder<T>
    {
        public string TypeName { get; private set; }

        public void Name(string typeName)
        {
            TypeName = typeName;
        }
    }
}