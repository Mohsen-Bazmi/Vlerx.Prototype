using System;
using System.Collections.Generic;
using Vlerx.Es.EventMapping.Contracts;

namespace Vlerx.Es.EventMapping.Context
{
    public class ContextSchemaConfigs : IContextSchemaConfigs
    {
        private readonly Dictionary<Type, string> NamesByType
            = new Dictionary<Type, string>();

        private readonly Dictionary<string, Type> TypesByName
            = new Dictionary<string, Type>();

        public void Add<T>(IEventSchema<T> map)
        {
            var builder = new SchemaBuilder<T>();
            map.Map(builder);
            var type = typeof(T);
            var name = builder.TypeName;

            //TODO: Replace this by some runtime checking against db
            if (string.IsNullOrWhiteSpace(builder.TypeName))
                builder.Name(type.FullName);

            //TODO: Replace this by some runtime checking against db
            if (TypesByName.ContainsKey(name))
                throw new InvalidOperationException(
                    $"'{type}' is already mapped to the following name: {TypesByName[name]}"
                );

            TypesByName[name] = type;
            NamesByType[type] = name;
        }

        public bool TryGetType(string name, out Type type)
        {
            return TypesByName.TryGetValue(name, out type);
        }

        public bool TryGetTypeName(Type type, out string name)
        {
            return NamesByType.TryGetValue(type, out name);
        }

        public string GetTypeName(Type type)
        {
            if (!TryGetTypeName(type, out var name))
                throw new Exception($"Failed to find name mapped with '{type}'");

            return name;
        }

        public Type GetType(string name)
        {
            if (!TryGetType(name, out var type))
                throw new Exception($"Failed to find type mapped with '{name}'");

            return type;
        }
    }
}