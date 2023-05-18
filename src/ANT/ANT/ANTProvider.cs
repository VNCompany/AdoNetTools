using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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
            
            fieldAttribute.FieldName ??= ANTProvider.CamelToSnake(propertyInfo.Name)!;
            fieldAttribute.DBType ??= GetDBType(propertyInfo.PropertyType) ??
                                      throw new InvalidCastException(
                                          $"Property `{propertyInfo.Name}` has unknown type");

            if (fieldAttribute.ValueConverterType == null)
                fieldAttribute.ValueConverterType = typeof(ValueConverters.DefaultValueConverter);
            else if (!typeof(ValueConverters.IValueConverter).IsAssignableFrom(fieldAttribute.ValueConverterType))
                throw new FormatException("The ValueConverterType class is not a converter");

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
                tableName = ANTProvider.CamelToSnake(entityType.Name)!;

            List<DBFieldMetadata> fieldMetadatas = new List<DBFieldMetadata>();
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                // If property has atribute `DBIgnore` then skip it
                if (propertyInfo.GetCustomAttribute<DBIgnoreAttribute>() != null
                    || propertyInfo.Name == "Metadata")
                    continue;
                
                fieldMetadatas.Add(_InitializeFieldMetadata(propertyInfo));
            }

            if (fieldMetadatas.Count(m => m.IsPrimaryKey == true) == 0)
                throw new InvalidOperationException("The entity must have a primary key property");
            
            return new DBEntityMetadata(entityType, tableName, fieldMetadatas);
        }
        
        public static int RegisteredClassesCount => _registeredClasses.Count;

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
    }
}