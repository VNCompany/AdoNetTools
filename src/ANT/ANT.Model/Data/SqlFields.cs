using System.Collections.Generic;
using System.Linq;

namespace ANT.Model.Data
{
    public class SqlFields : List<string>
    {
        private readonly string? _tableName;

        public SqlFields(IEnumerable<string> fields) : base(fields) { }

        public SqlFields(string tableName, IEnumerable<string> fields) : this(fields)
            => _tableName = tableName;

        public override string ToString()
        {
            IEnumerable<string> e = _tableName == null ? this : this.Select(f => $"{_tableName}.{f}");
            return string.Join(",", e);
        }
    }
}