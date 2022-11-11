using System.Collections.Generic;

namespace Vnc.Ado.Extensions
{
    public class SqlCommandParameters : Dictionary<string, object>
    {
        public SqlCommandParameters() { }

        public SqlCommandParameters(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
                Add(keyValuePair.Key, keyValuePair.Value);
        }

        public SqlCommandParameters(Dictionary<string, object> dictionary)
        {
            foreach (var keyValuePair in dictionary)
                Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
