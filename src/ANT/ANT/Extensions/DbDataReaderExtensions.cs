using System;
using System.Collections.Generic;
using System.Data.Common;

using ANT.Model;

namespace ANT.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static void ToEntity<T>(this DbDataReader reader, T refEntity) where T : IDBEntity
        {
            refEntity.DBEntityImport(reader);
        }
        
        public static T ToEntity<T>(this DbDataReader reader) where T : IDBEntity
        {
            T instance = Activator.CreateInstance<T>();
            ToEntity(reader, instance);
            return instance;
        }

        public static IEnumerable<T> ToEntities<T>(this DbDataReader reader) where T : IDBEntity
        {
            while (reader.Read())
                yield return ToEntity<T>(reader);
        }
    }
}