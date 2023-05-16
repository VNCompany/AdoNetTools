using System;
using System.Collections.Generic;

namespace ANT.Model
{
    public class DBEntityMetadata
    {
        private readonly DBFieldMetadata[] _primaryKeys;
        private readonly Dictionary<string, DBFieldMetadata> _fieldMetadatas;

        public Type ClassType { get; }
        public string TableName { get; }

        public IReadOnlyDictionary<string, DBFieldMetadata> FieldMetadatas => _fieldMetadatas;
        public IReadOnlyCollection<DBFieldMetadata> PrimaryKeys => _primaryKeys;

        public DBEntityMetadata(Type classType, string tableName, IEnumerable<DBFieldMetadata> fieldMetadatas)
        {
            ClassType = classType;
            TableName = tableName;
            
            _fieldMetadatas = new Dictionary<string, DBFieldMetadata>();
            List<DBFieldMetadata> primaryKeysList = new List<DBFieldMetadata>();
            foreach (var fieldMetadata in fieldMetadatas)
            {
                _fieldMetadatas[fieldMetadata.FieldName] = fieldMetadata;
                if (fieldMetadata.IsPrimaryKey)
                    primaryKeysList.Add(fieldMetadata);
            }

            _primaryKeys = primaryKeysList.ToArray();
        }
    }
}