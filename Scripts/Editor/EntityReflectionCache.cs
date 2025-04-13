namespace MK.Entities.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class EntityReflectionCache
    {
#region Constants

        public const string NamePropertyName       = "Name";
        public const string IndexPropertyName      = "Index";
        public const string ComponentsFieldName    = "typeToComponents";
        public const string EntityFactoryFieldName = "entityFactory";

#endregion

#region Fields

        public static readonly BindingFlags PrivateInstanceFlags = BindingFlags.Instance | BindingFlags.NonPublic;

#endregion

#region Cache

        private static readonly Dictionary<Type, FieldInfo[]>    CachedFields     = new();
        private static readonly Dictionary<Type, PropertyInfo[]> CachedProperties = new();

        public static FieldInfo[] GetCachedFields(Type type)
        {
            if (!CachedFields.TryGetValue(type, out var fields))
            {
                fields             = type.GetFields(PrivateInstanceFlags);
                CachedFields[type] = fields;
            }

            return fields;
        }

        public static PropertyInfo[] GetCachedProperties(Type type)
        {
            if (!CachedProperties.TryGetValue(type, out var properties))
            {
                properties             = type.GetProperties(PrivateInstanceFlags);
                CachedProperties[type] = properties;
            }

            return properties;
        }

#endregion
    }
}