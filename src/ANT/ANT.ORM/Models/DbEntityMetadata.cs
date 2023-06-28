using System;
using System.Linq;
using System.Collections.Generic;

namespace ANT.ORM.Models
{
    public class DbEntityMetadata
    {
        private readonly Dictionary<string, DbFieldMetadata> _fieldMetadataDict;

        public Type ClassType { get; }
        public string Name { get; }

        public IReadOnlyDictionary<string, DbFieldMetadata> FieldMetadataDict => _fieldMetadataDict;
        public IEnumerable<DbFieldMetadata> PrimaryKeys => _fieldMetadataDict.Values.Where(fm => fm.IsPrimaryKey);

        public DbEntityMetadata(Type classType, string name, IEnumerable<DbFieldMetadata> fieldMetadataCollection)
        {
            ClassType = classType;
            Name = name;

            _fieldMetadataDict = new Dictionary<string, DbFieldMetadata>(
                from fm in fieldMetadataCollection
                select new KeyValuePair<string, DbFieldMetadata>(fm.Name, fm));
        }
    }
}