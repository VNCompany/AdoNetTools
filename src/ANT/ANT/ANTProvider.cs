using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using ANT.Model;
using ANT.ValueConverters;

namespace ANT
{
    public static partial class ANTProvider
    {
        // Private
        
        private static readonly Dictionary<string, DBEntityMetadata> _registeredClasses =
            new Dictionary<string, DBEntityMetadata>();
        
#nullable disable
        private static IANTConfigurator Configuration;
#nullable enable

        private static DBFieldMetadata _InitializeFieldMetadata(IDictionary<Type, IValueConverter> converters,
            PropertyInfo propertyInfo)
        {
            DBFieldAttribute? fieldAttribute = propertyInfo.GetCustomAttribute<DBFieldAttribute>();
            if (fieldAttribute != null)
            {
                if (string.IsNullOrEmpty(fieldAttribute.Info.FieldName))
                    fieldAttribute.Info.FieldName = CamelToSnake(propertyInfo.Name)!;
                if (string.IsNullOrEmpty(fieldAttribute.Info.DBType))
                    fieldAttribute.Info.DBType = GetDBType(propertyInfo.PropertyType);

                if (fieldAttribute.ValueConverterType == typeof(DefaultValueConverter))
                    return new DBFieldMetadata(fieldAttribute.Info, propertyInfo, DefaultValueConverter.GetObject);
                else
                {
                    if (!converters.TryGetValue(fieldAttribute.ValueConverterType, out IValueConverter? converterObject))
                    {
                        converterObject = (IValueConverter)Activator.CreateInstance(fieldAttribute.ValueConverterType)!;
                        converters.Add(fieldAttribute.ValueConverterType, converterObject);
                    }
                    return new DBFieldMetadata(fieldAttribute.Info, propertyInfo, converterObject);
                }
            }
            else
                return new DBFieldMetadata(new DBFieldInfo()
                {
                    FieldName = CamelToSnake(propertyInfo.Name)!,
                    DBType = GetDBType(propertyInfo.PropertyType)
                }, propertyInfo, DefaultValueConverter.GetObject);
        }

        private static DBEntityMetadata _InitializeEntityMetadata(Type entityType)
        {
            string tableName;
            DBEntityAttribute? entityAttribute;
            if ((entityAttribute = entityType.GetCustomAttribute<DBEntityAttribute>()) != null &&
                entityAttribute.TableName != null)
                tableName = entityAttribute.TableName;
            else
                tableName = ANTProvider.ModifyEntityName(entityType.Name);

            Dictionary<Type, IValueConverter> valueConverters = new Dictionary<Type, IValueConverter>();
            List<DBFieldMetadata> fieldMetadatas = new List<DBFieldMetadata>();
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<DBIgnoreAttribute>() != null
                    || propertyInfo.Name == "Metadata")
                    continue;
                
                fieldMetadatas.Add(_InitializeFieldMetadata(valueConverters, propertyInfo));
            }

            if (fieldMetadatas.Count(m => m.Info.IsPrimaryKey) == 0)
                throw new InvalidOperationException("The entity must have a primary key property");
            
            return new DBEntityMetadata(entityType, tableName, fieldMetadatas);
        }
        
        // Public

        public static void Use(IANTConfigurator configurator)
        {
            Configuration = configurator;
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
    }
}