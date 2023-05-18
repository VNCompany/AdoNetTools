using System.Collections.Generic;

namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata? Metadata { get; }

        void SetFieldValue(string fieldName, object? value);
        DBField? GetFieldValue(string fieldName);
        IEnumerable<KeyValuePair<string, DBField>> GetFields();
    }
}