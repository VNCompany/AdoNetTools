using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ANT.ORM.Models;
using ANT.ORM.Tools;

namespace ANT.ORM
{
    public static class AntProvider
    {
        // Private
        
        private static readonly Dictionary<string, DbEntityMetadata> _registeredClasses = new();

        private static DbFieldMetadata _InitializeFieldMetadata(PropertyInfo propertyInfo)
        {
            var fieldAttr = propertyInfo.GetCustomAttribute(typeof(DbFieldAttribute)) as DbFieldAttribute;
            if (fieldAttr == null)
                return new DbFieldMetadata(propertyInfo);
            return new DbFieldMetadata(propertyInfo, fieldAttr);
        }

        private static DbEntityMetadata _InitializeEntityMetadata(Type entityType)
        {
            var entityAttr = entityType.GetCustomAttribute<DbEntityAttribute>();
            string tableName = entityAttr is { Name: not null }
                ? entityAttr.Name
                : NameConversions.ToPlural(entityType.Name);
            
            return new DbEntityMetadata(
                entityType, tableName,
                from propInfo in entityType.GetProperties()
                where propInfo.Name != "Metadata"
                      && propInfo.GetCustomAttribute(typeof(DbIgnoreAttribute)) == null
                select _InitializeFieldMetadata(propInfo));
        }
        
        // Public

        public static void RegisterClass<T>() where T: IDbEntity
        {
            Type entityType = typeof(T);
            string classFullName = entityType.FullName!;
            if (_registeredClasses.ContainsKey(classFullName))
                throw new InvalidOperationException("Entity class already registered");

            _registeredClasses.Add(classFullName, _InitializeEntityMetadata(entityType));
        }
        
        public static DbEntityMetadata? GetEntityMetadata(Type entityType)
        {
            if (entityType.FullName != null
                && _registeredClasses.TryGetValue(entityType.FullName, out DbEntityMetadata? value))
                return value;
            return null;
        }
    }
}