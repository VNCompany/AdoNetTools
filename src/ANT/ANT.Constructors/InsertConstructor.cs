using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ANT.Constructors.Internal;

namespace ANT.Constructors
{
    public class InsertConstructor : IQueryConstructor
    {
        private readonly string _table;
        private readonly string[] _cols;
        private readonly List<InsertCommandRow> _rows = new();

        public void Add(IEnumerable<object?> values)
        {
            var row = values as InsertCommandRow ?? new InsertCommandRow(values);
            if (row.Length != _cols.Length) throw new ArgumentException("values");
            _rows.Add(row);
        }

        public void AddRange(IEnumerable<InsertCommandRow> values)
        {
            foreach (var row in values)
            {
                if (row.Length != _cols.Length) throw new ArgumentException("values");
                _rows.Add(row);
            }
        }

        private InsertConstructor(string table, IEnumerable<string> fields)
        {
            _table = table;
            _cols = fields as string[] ?? fields.ToArray();
        }

        public InsertConstructor(string table, IEnumerable<string> fields, IEnumerable<object?> values)
            : this(table, fields) => Add(values);

        public InsertConstructor(string table, IEnumerable<string> fields, IEnumerable<InsertCommandRow> values)
            : this(table, fields) => AddRange(values);

        public string Construct(ConstructorParameters parameters)
        {
            if (_rows.Count == 0) throw new InvalidOperationException("values empty");
            
            var qb = new StringBuilder();
            qb.AppendFormat("INSERT INTO `{0}` (`", _table);
            qb.AppendJoin("`,`", _cols);
            qb.Append("`) VALUES ");

            int iter = 0;
            qb.AppendJoin(',', _rows.Select(
                r => r.Format(
                    (rowId, value) 
                        => parameters.Add($"r{iter++}_{_cols[rowId]}", value))));
            qb.Append(';');
            return qb.ToString();
        }
    }
}