using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ANT.Model;

namespace ANT.Constructors
{
    public class InsertConstructor : IQueryConstructor, ICollection<>
    {
        private readonly string _table;
        private readonly ValuesCollection _fields;
        private readonly ValuesCollection[] _values;

        private string[][] _links = null!;

        private void InitLinks()
        {
            _links = new string[_values.Length][];

            for (int i = 0; i < _values.Length; i++)
            {
                if (_values[i].Count != _fields.Count) throw new InvalidCastException($"Invalid row[{i}]");

                _links[i] = new string[_fields.Count];
                for (int j = 0; j < _links[i].Length; j++)
                    _links[i][j] = $"r{i}_c{j}_{_fields[j]}";
            }
        }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, ValuesCollection[] values)
        {
            _table = tableName;
            _fields = new ValuesCollection(fieldNames);
            if (_fields.Count == 0) throw new ArgumentException("fieldNames must have values", nameof(fieldNames));
            _values = values;
            HashSet<string> v = new HashSet<string>();
            var b = v[1];
            InitLinks();
        }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, ValuesCollection values)
            : this(tableName, fieldNames, new[] { values }) { }

        public InsertConstructor(string tableName, IEnumerable<string> fieldNames, IEnumerable<ValuesCollection> values)
            : this(tableName, fieldNames, values.ToArray()) { }
        
        public IEnumerable<KeyValuePair<string, object?>> GetCommandParameters()
        {
            for (int i = 0; i < _links.Length; i++)
            {
                for (int j = 0; j < _links[i].Length; j++)
                    yield return new KeyValuePair<string, object?>(_links[i][j], _values[i][j]);
            }
        }

        public string? Build()
        {
            StringBuilder sb = new StringBuilder($"INSERT INTO `{_table}` ");
            sb.Append(_fields.ToString("()", "``", ","));
            sb.Append(" VALUES ");
            sb.AppendJoin(",", from row in _values )
        }

        public static InsertConstructor FromEntity<T>(T entity) where T : IDBEntity
        {
            var dict = entity.DBEntityExport();
            return new InsertConstructor(entity.Metadata.TableName, dict.Keys,
                new ValuesCollection(dict.Values));
        }

        public static InsertConstructor FromEntities<T>(IEnumerable<T> entities) where T : IDBEntity
        {
            var dicts = new List<IReadOnlyDictionary<string, object?>>(entities.Select(e => e.DBEntityExport()));
            if (dicts.Count == 0) throw new ArgumentException("entities must have values", nameof(entities));

            return new InsertConstructor(
                entities.First().Metadata.TableName,
                dicts[0].Keys,
                from dict in dicts
                select new ValuesCollection(dict.Values));
        }
    }
}