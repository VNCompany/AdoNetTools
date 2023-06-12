using System;
using System.Collections.Generic;

using ANT.Model;
using ANT.Model.Data;
using ANT.Model.Data.MappingModels;

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

        public IReadOnlyDictionary<string, EntityFieldData> DBEntityExport(
            IEnumerable<string>? exportedProperties = null)
        {
            HashSet<string>? exportProps = exportedProperties != null ? new HashSet<string>(exportedProperties) : null;
            var result = new Dictionary<string, EntityFieldData>();
            foreach (var (fieldName, fieldMeta) in Metadata.FieldMetadatas)
            {
                if (exportProps != null && exportProps.Contains(fieldName))
                    continue;
                
                var convertedValue = fieldMeta.Converter.ConvertFrom(
                    fieldMeta.PropertyInfo.GetValue(this),
                    fieldMeta.PropertyInfo.PropertyType);
                result.Add(fieldName, new EntityFieldData(fieldName, fieldMeta.Info, convertedValue));
            }

            return result;
        }
    }
}