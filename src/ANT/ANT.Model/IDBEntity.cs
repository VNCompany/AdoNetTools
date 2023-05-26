using System.Collections.Generic;

namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata Metadata { get; }
        void DBEntityImport(System.Data.Common.DbDataReader dbDataReader);
        IReadOnlyDictionary<string, object> DBEntityExport();
    }
}