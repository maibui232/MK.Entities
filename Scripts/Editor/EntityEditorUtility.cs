namespace MK.Entities.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class EntityEditorUtility
    {
#region Reflection Fields

        // Entity type and fields
        private static readonly Type         EntityType      = typeof(Entity);
        private static readonly PropertyInfo NameProperty    = EntityType.GetProperty(EntityReflectionCache.NamePropertyName,  EntityReflectionCache.PrivateInstanceFlags);
        private static readonly PropertyInfo IndexProperty   = EntityType.GetProperty(EntityReflectionCache.IndexPropertyName, EntityReflectionCache.PrivateInstanceFlags);
        private static readonly FieldInfo    ComponentsField = EntityType.GetField(EntityReflectionCache.ComponentsFieldName, EntityReflectionCache.PrivateInstanceFlags);

        // EntityManager and factory fields
        private static readonly Type      EntityManagerType  = typeof(EntityManager);
        private static readonly FieldInfo EntityFactoryField = EntityManagerType.GetField(EntityReflectionCache.EntityFactoryFieldName, EntityReflectionCache.PrivateInstanceFlags);

#endregion

#region Entity Methods

        public static IEnumerable<Entity> GetEntities(World world)
        {
            if (world == null) return Enumerable.Empty<Entity>();
            try
            {
                var entityFactory        = EntityFactoryField?.GetValue(world.EntityManager);
                var spawnedEntitiesField = entityFactory?.GetType().GetField("spawnedEntities", EntityReflectionCache.PrivateInstanceFlags);
                if (spawnedEntitiesField != null)
                {
                    return spawnedEntitiesField.GetValue(entityFactory) as IEnumerable<Entity> ?? Enumerable.Empty<Entity>();
                }
            }
            catch (Exception)
            {
                //ignore
            }

            return Enumerable.Empty<Entity>();
        }

        public static string GetEntityName(Entity entity)
        {
            // if (entity == null) return "Null Entity";// todo fix

            return NameProperty?.GetValue(entity) as string ?? "Unknown";
        }

#endregion

#region Component Methods

        public static IEnumerable<object> GetComponents(Entity entity)
        {
            // if (entity == null) return Enumerable.Empty<object>(); // todo fix

            return ComponentsField?.GetValue(entity) is not Dictionary<Type, IComponent> components
                       ? Enumerable.Empty<object>()
                       : components.Values;
        }

        public static string GetComponentName(object component) { return component?.GetType().Name ?? "Unknown"; }

#endregion
    }
}