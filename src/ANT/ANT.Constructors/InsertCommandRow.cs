using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANT.Constructors
{
    public class InsertCommandRow : IEnumerable<object?>
    {
        private readonly IList<object?> _values;

        public int Length => _values.Count;

        public InsertCommandRow(IEnumerable<object?> values)
        {
            _values = values as IList<object?> ?? values.ToArray();
        }

        public override string ToString()
        {
            var collection = _values.Select(v => v?.ToString()?.Replace("'", "\\'") ?? "null");
            return string.Concat("('", string.Join("','", collection), "')");
        }

        public string Format(Func<int, object?, string> converter)
        {
            if (_values.Count == 0) return "()";
            
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Concat("(", converter.Invoke(0, _values[0])));
            for (int i = 1; i < _values.Count; i++)
                sb.Append(string.Concat(",", converter.Invoke(i, _values[i])));
            sb.Append(')');
            return sb.ToString();
        }

        public IEnumerator<object?> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}