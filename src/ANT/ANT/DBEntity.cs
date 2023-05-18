using System;
using System.Collections.Generic;

using ANT.Model;

namespace ANT
{
    public abstract class DBEntity : IDBEntity
    {
        private DBEntityMetadata? metadata;

        public DBEntityMetadata Metadata
            => metadata ??= ANTProvider.GetEntityMetadata(this.GetType())
                            ?? throw new InvalidOperationException(
                                "Entity class is not registered in ANTProvider");

        public DBField? GetFieldValue(string fieldName)
        {
            if (Metadata.FieldMetadatas.TryGetValue(fieldName, out DBFieldMetadata? fieldMetadata))
                return new DBField(fieldName, fieldMetadata.PropertyInfo.GetValue(this),
                    fieldMetadata.ValueConverterType);
            return null;
        }

        public IEnumerable<KeyValuePair<string, DBField>> GetFields()
        {
            foreach (var item in Metadata.FieldMetadatas)
            {
                yield return new KeyValuePair<string, DBField>(
                    key: item.Key,
                    value: new DBField(item.Key, item.Value.PropertyInfo.GetValue(this),
                        item.Value.ValueConverterType));
            }
        }
    }
}