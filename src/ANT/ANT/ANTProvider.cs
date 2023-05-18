using System;
using System.Collections.Generic;
using System.Reflection;

using ANT.Model;

namespace ANT
{
    public static partial class ANTProvider
    {
        private static readonly Dictionary<string, DBEntityMetadata> _registeredClasses;
        static ANTProvider()
        {
            _InitializeDBTypesDictionary();
            _registeredClasses = new Dictionary<string, DBEntityMetadata>();
        }

        private static DBFieldMetadata _InitializeFieldMetadata(PropertyInfo propertyInfo)
        {
            DBFieldAttribute fieldAttribute =
                propertyInfo.GetCustomAttribute<DBFieldAttribute>() ?? new DBFieldAttribute();
            
            fieldAttribute.FieldName ??= propertyInfo.Name;
            fieldAttribute.DBType ??= DBTypes.TryGetValue(propertyInfo.GetType().FullName ?? "_none", out string? value)
                ? value
                : throw new InvalidCastException("Unknown type");
            fieldAttribute.ValueConverterType = typeof(ANTProvider);  // TODO: Realize value converter

            return new DBFieldMetadata(
                fieldAttribute.FieldName,
                propertyInfo,
                fieldAttribute.DBType,
                fieldAttribute.IsNotNull,
                fieldAttribute.IsPrimaryKey,
                fieldAttribute.ValueConverterType);
        }
        
        private static DBEntityMetadata _InitializeEntityMetadata(Type entityType)
        {
            string tableName;
            DBEntityAttribute? entityAttribute;
            if ((entityAttribute = entityType.GetCustomAttribute<DBEntityAttribute>()) != null &&
                entityAttribute.TableName != null)
                tableName = entityAttribute.TableName;
            else
                tableName = entityType.Name;

            List<DBFieldMetadata> fieldMetadatas = new List<DBFieldMetadata>();
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                // If property has atribute `DBIgnore` then skip it
                if (propertyInfo.GetCustomAttribute<DBIgnoreAttribute>() != null)
                    continue;
                
                fieldMetadatas.Add(_InitializeFieldMetadata(propertyInfo));
            }

            return new DBEntityMetadata(entityType, tableName, fieldMetadatas);
        }
        
        public static void RegisterClass<T>() where T: IDBEntity
        {
            Type entityType = typeof(T);
            string classFullName = entityType.FullName ?? throw new InvalidOperationException();
            if (_registeredClasses.ContainsKey(classFullName))
                throw new InvalidOperationException("Entity class already registered");

            _registeredClasses[classFullName] = _InitializeEntityMetadata(entityType);
        }
        
        public static DBEntityMetadata? GetEntityMetadata(Type entityType)
        {
            if (entityType.FullName != null
                && _registeredClasses.TryGetValue(entityType.FullName, out DBEntityMetadata? value))
                return value;
            return null;
        }

        public static DBEntityMetadata? GetEntityMetadata<T>() where T: IDBEntity
            => GetEntityMetadata(typeof(T));

        public static string? CamelToSnake(string? input)
        {
            // TODO: Realize method "CamelToSnake"
            return input;
        }
    }
}