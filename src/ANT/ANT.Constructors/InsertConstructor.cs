using System.Collections.Generic;
using System.Linq;
using ANT.Model;

namespace ANT.Constructors
{
    public class InsertConstructor : IQueryConstructor
    {
        private readonly string _table;
        private readonly string[] _fields;
        private readonly ValuesCollection[] _values;
        
        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, ValuesCollection values)
        {
            _table = tableName;
            _fields = fieldNames.ToArray();
            _values = new[] { values };
        }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, IEnumerable<ValuesCollection> values)
        {
            _table = tableName;
            _fields = fieldNames.ToArray();
            _values = values.ToArray();
        }
        
        public IEnumerable<KeyValuePair<string, object?>> GetCommandParameters()
        {
            return Enumerable.Empty<KeyValuePair<string, object?>>();
        }

        public string? Build()
        {
            return null;
        }
    }
}