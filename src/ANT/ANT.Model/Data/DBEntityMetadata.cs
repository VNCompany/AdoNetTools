using System;
using System.Linq;
using System.Collections.Generic;

namespace ANT.Model.Data
{
    public class DBEntityMetadata
    {
        private readonly Dictionary<string, DBFieldMetadata> _fieldMetadatas;

        public Type ClassType { get; }
        public string TableName { get; }

        public IReadOnlyDictionary<string, DBFieldMetadata> FieldMetadatas => _fieldMetadatas;
        public IEnumerable<DBFieldMetadata> PrimaryKeys => _fieldMetadatas.Values.Where(m => m.Info.IsPrimaryKey);

        public DBEntityMetadata(Type classType, string tableName, IEnumerable<DBFieldMetadata> fieldMetadatas)
        {
            ClassType = classType;
            TableName = tableName;

            _fieldMetadatas = new Dictionary<string, DBFieldMetadata>(
                from fm in fieldMetadatas
                select new KeyValuePair<string, DBFieldMetadata>(fm.Info.FieldName, fm));
        }
    }
}