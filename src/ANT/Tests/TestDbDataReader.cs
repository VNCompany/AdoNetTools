using System.Collections;
using System.Data.Common;
using System.Collections.Generic;

namespace Tests
{
    class TestDbDataReader : DbDataReader
    {
        private readonly List<string> _columnNames = new List<string>();
        private readonly List<object> _values = new List<object>();
        
        public TestDbDataReader(IEnumerable<KeyValuePair<string, object>> dict)
        {
            foreach (var item in dict)
            {
                _columnNames.Add(item.Key);
                _values.Add(item.Value);
            }
        }
        
        public override int FieldCount => _columnNames.Count;
        public override bool HasRows => _columnNames.Count > 0;

        public override object this[int ordinal] => _values[ordinal];

        public override object this[string name] => _values[_columnNames.IndexOf(name)];

        public override Type GetFieldType(int ordinal) => _values[ordinal].GetType();

        public override string GetName(int ordinal) => _columnNames[ordinal];

        public override int GetOrdinal(string name) => _columnNames.IndexOf(name);

        public override string GetString(int ordinal) => _values[ordinal].ToString() ?? string.Empty;

        public override object GetValue(int ordinal) => _values[ordinal];

        public override bool Read() => false;
        
        #region NotImplemented
        
        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool IsClosed { get; } = false;
        public override int Depth { get; } = -1;
        public override int RecordsAffected { get; } = 0;
        
        #endregion
        
    }
}