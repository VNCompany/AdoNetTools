using System;
using System.Linq;
using System.Collections.Generic;

using ANT.Model;

namespace ANT
{
    public abstract class DBEntity : IDBEntity
    {
        private DBEntityMetadata? metadata;
        public DBEntityMetadata Metadata
        {
            get
            {
                if (metadata == null)
                    if ((metadata = ANTProvider.GetEntityMetadata(this.GetType())) == null)
                        throw new InvalidOperationException("Entity class is not registered in ANTProvider");
                return metadata;
            }
        }

        public void DBEntityImport(System.Data.Common.DbDataReader dbDataReader)
        {
            foreach (var (fieldName, fieldMeta) in Metadata.FieldMetadatas)
            {
                fieldMeta.PropertyInfo.SetValue(this, fieldMeta.Converter.ConvertTo(
                    dbDataReader, fieldName, fieldMeta.PropertyInfo.PropertyType));
            }
        }

        public IReadOnlyDictionary<string, object> DBEntityExport()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var (fieldName, fieldMeta) in Metadata.FieldMetadatas)
            {
                if (fieldMeta.Info.IsAuto) continue;
                
                var convertedValue = fieldMeta.Converter.ConvertFrom(
                    fieldMeta.PropertyInfo.GetValue(this),
                    fieldMeta.PropertyInfo.PropertyType);
                result.Add(fieldName, convertedValue);
            }
            
            return result;
        }
    }
}