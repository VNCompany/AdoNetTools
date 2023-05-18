using System.Collections.Generic;

namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata? Metadata { get; }

        DBField? GetFieldValue(string fieldName);
        IEnumerable<KeyValuePair<string, DBField>> GetFields();
    }
}