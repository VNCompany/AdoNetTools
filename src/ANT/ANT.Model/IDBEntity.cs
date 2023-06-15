using System.Collections.Generic;

using ANT.Model.Data;
using ANT.Model.Data.MappingModels;

namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata Metadata { get; }
        void DBEntityImport(System.Data.Common.DbDataReader dbDataReader, SqlFields? selectedFields = null);
        IReadOnlyDictionary<string, EntityFieldData> DBEntityExport(IEnumerable<string>? exportedProperties = null);
    }
}