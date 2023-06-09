using System;
using System.Collections.Generic;
using ANT.Constructors.QueryParts;

namespace ANT.Constructors
{
    public class InsertConstructor : QueryConstructor
    {
#nullable disable
        private ValuesCollection _keys, _values;
#nullable restore
        
        private void Init(string tableName)
        {
            _keys = new ValuesCollection();
            _values = new ValuesCollection();
            _values.Config.Start = _values.Config.End = String.Empty;
            
            CommandParts.Add($"INSERT INTO `{tableName}`");
            CommandParts.Add(_keys);
            CommandParts.Add("VALUES");
            CommandParts.Add(_values);
        }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, IEnumerable<object?> fieldValues)
        {
            Init(tableName);
            _keys.AddRange(fieldNames);
            _values.Add(new ValuesCollection(fieldValues));
        }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames,
            IEnumerable<IEnumerable<object?>> fieldValuesCollection)
        {
            
        }
    }
}