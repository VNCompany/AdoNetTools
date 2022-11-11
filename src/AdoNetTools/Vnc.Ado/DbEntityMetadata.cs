using System;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;

using Vnc.Ado.Interfaces;
using Vnc.Ado.Converters;
using Vnc.Ado.Internal;

namespace Vnc.Ado
{
    public class DbEntityMetadata<T> where T : class
    {
        private Type entityType;
        private DbEntityAttribute attribute;
        private Dictionary<string, DbFieldInfo> fieldInfos;

        private void CollectMetadata()
        {
            fieldInfos = new Dictionary<string, DbFieldInfo>();

            foreach (var property in entityType.GetProperties())
            {
                DbFieldAttribute fieldAttribute = property.GetCustomAttribute<DbFieldAttribute>();
                if (fieldAttribute != null)
                {
                    DbFieldInfo fieldInfo = new DbFieldInfo
                    {
                        Name = fieldAttribute.FieldName != null ? fieldAttribute.FieldName : property.Name.ToLower(),
                        Attribute = fieldAttribute,
                        PropertyInfo = property
                    };

                    if (fieldAttribute.ConverterType != null)
                        fieldInfo.Converter = (IDbValueConverter)Activator.CreateInstance(fieldAttribute.ConverterType);
                    else if ((fieldInfo.Converter = DefaultDbConverter.Create(property.PropertyType)) == null)
                        throw new InvalidCastException($"Couldn't find converter for this type: `{property.PropertyType}`");

                    fieldInfos.Add(fieldInfo.Name, fieldInfo);
                }
            }
        }

        string tableName;
        public string TableName
        {
            get
            {
                if (tableName == null)
                {
                    tableName = attribute.TableName != null ? attribute.TableName : entityType.Name.ToLower();
                }
                return tableName;
            }
        }

        public string PrimaryKey { get; set; } = "id";

        public IEnumerable<KeyValuePair<string, DbFieldInfo>> Fields => fieldInfos;
        public ICollection<string> FieldNames => fieldInfos.Keys;
        public DbFieldInfo this[string fieldName] => fieldInfos[fieldName];

        public DbEntityMetadata()
        {
            entityType = typeof(T);
            if ((attribute = entityType.GetCustomAttribute<DbEntityAttribute>()) != null)
                CollectMetadata();
            else
                throw new Exception("Class `" + entityType.FullName + "` doesn't have DbEntityAttribute");
        }

        public void SetFieldValue(T entity, string fieldName, object value)
        {
            fieldInfos[fieldName].PropertyInfo.SetValue(entity, value);
        }
        public object GetFieldValue(T entity, string fieldName)
        {
            return fieldInfos[fieldName].PropertyInfo.GetValue(entity);
        }

        public IEnumerable<object> GetValues(T entity, IEnumerable<string> fields)
            => from fieldName in fields
               select GetFieldValue(entity, fieldName);

        public T MapEntity(DbDataReader dataReader, IEnumerable<string> fields)
        {
            object entityObject = Activator.CreateInstance(entityType);
            foreach (string fieldName in fields)
            {
                DbFieldInfo fieldInfo = fieldInfos[fieldName];
                object fieldValue = fieldInfo.Converter.Convert(dataReader, dataReader.GetOrdinal(fieldName));
                fieldInfo.PropertyInfo.SetValue(entityObject, fieldValue);
            }
            return (T)entityObject;
        }
        public T MapEntity(DbDataReader dataReader) => MapEntity(dataReader, fieldInfos.Keys);
    }
}