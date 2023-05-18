using System;
using System.Data.Common;

using ANT.ValueConverters;
using ANT.Model;

namespace ANT
{
    public static partial class ANTProvider
    {
        public static void DBMapToEntity(object? entity, DbDataReader dataReader)
        {
            if (entity is IDBEntity { Metadata: not null } entityObject)
            {
                foreach (var item in entityObject.Metadata.FieldMetadatas.Values)
                {
                    IValueConverter converter = (IValueConverter)Activator.CreateInstance(item.ValueConverterType)!;
                    item.PropertyInfo.SetValue(
                        entity, 
                        converter.ConvertTo(dataReader, item.FieldName, item.PropertyInfo.PropertyType));
                }
            }
        }

        public static object? DBMapToEntity(Type entityType, DbDataReader dataReader)
        {
            if (typeof(IDBEntity).IsAssignableFrom(entityType))
            {
                object? entity = Activator.CreateInstance(entityType);
                DBMapToEntity(entity, dataReader);
                return entity;
            }
            return null;
        }

        public static void DBMapToEntity<T>(T? entity, DbDataReader dataReader) where T: IDBEntity
            => DBMapToEntity((object?)entity, dataReader);

        public static T? DBMapToEntity<T>(DbDataReader dataReader) where T: IDBEntity
            => (T?)DBMapToEntity(typeof(T), dataReader);
    }
}