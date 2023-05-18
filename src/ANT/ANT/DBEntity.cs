using System.Collections.Generic;
using ANT.Model;

namespace ANT
{
    public class DBEntity : IDBEntity
    {
        public DBEntityMetadata? Metadata { get; }
        
        
        public void SetFieldValue(string fieldName, object? value)
        {
            throw new System.NotImplementedException();
        }

        public DBField? GetFieldValue(string fieldName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, DBField>> GetFields()
        {
            throw new System.NotImplementedException();
        }
    }
}