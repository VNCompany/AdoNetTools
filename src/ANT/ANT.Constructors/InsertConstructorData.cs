using System;
using System.Collections.Generic;
using System.Linq;

namespace ANT.Constructors
{
    public class InsertConstructorData
    {
        private readonly string[] _fields;
        private readonly List<object?[]> _rows;

        public int RowsCount => _rows.Count;

        internal string[] Fields => _fields;

        internal object?[] this[int rowIndex] => _rows[rowIndex];

        public InsertConstructorData(IEnumerable<string> fields)
        {
            _fields = fields as string[] ?? fields.ToArray();
            if (_fields.Length == 0)
                throw new InvalidOperationException("fields is empty");

            if (_fields.Length != new HashSet<string>(_fields).Count)
                throw new InvalidOperationException("The fields contains duplicates");

            _rows = new List<object?[]>();
        }

        public InsertConstructorData(IEnumerable<string> fields, IEnumerable<object?> values)
            : this(fields) => AddValues(values);

        public InsertConstructorData(IEnumerable<string> fields, IEnumerable<IEnumerable<object?>> values)
            : this(fields) => AddValuesRange(values);

        public void AddValues(IEnumerable<object?> values)
        {
            object?[] row = values as object?[] ?? values.ToArray();

            if (row.Length != _fields.Length)
                throw new InvalidOperationException(
                    "the length of the values is not equal to the length of the fields");

            _rows.Add(row);
        }

        public string GetParameterName(int row, int field)
        {
            if (row < 0 || row >= RowsCount) throw new IndexOutOfRangeException("row");
            if (field < 0 || field >= Fields.Length) throw new IndexOutOfRangeException("field");

            return $"f{row + 1}_{_fields[field]}";
        }

        public void AddValuesRange(IEnumerable<IEnumerable<object?>> values)
        {
            foreach (var it in values)
                AddValues(it);
        }

        public void Remove(int rowIndex)
        {
            _rows.RemoveAt(rowIndex);
        }

        public void Clear()
        {
            _rows.Clear();
        }
    }
}