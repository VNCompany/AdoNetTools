using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ANT.ORM.Models;

namespace ANT.ORM
{
    public static class DbCommandExtensions 
    {
        private static T _CreateEntity<T>(DbDataReader reader, IList<string> columns) 
            where T : IDbEntity, new()
        {
            T entity = new T();
            for (int i = 0; i < columns.Count; i++)
            {
                if (entity.Metadata.FieldMetadataDict.TryGetValue(columns[i], out var fieldMeta))
                {
                    object dbValue = reader.GetValue(i);
                    fieldMeta.SetValue(entity, dbValue == DBNull.Value ? null : dbValue);
                }
            }

            return entity;
        }
        
        public static IEnumerable<T> ExecuteQuery<T>(this DbCommand cmd) where T : IDbEntity, new()
        {
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if(reader.Read() == false)
                    yield break;

                List<string> columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                
                yield return _CreateEntity<T>(reader, columns);
                while (reader.Read())
                    yield return _CreateEntity<T>(reader, columns);
            }
        }
    }
}