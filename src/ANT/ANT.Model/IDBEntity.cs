using System.Collections.Generic;

using ANT.Model.Data;

namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata Metadata { get; }
        void DBEntityImport(System.Data.Common.DbDataReader dbDataReader);
        IReadOnlyDictionary<string, Data.MappingModels.EntityFieldData> DBEntityExport();
    }
}