using System.Collections;
using System.Collections.Generic;

using KVP = System.Collections.Generic.KeyValuePair<string, object?>;

namespace ANT.Constructors.Internal
{
    public class ConstructorParameters : IEnumerable<KVP>
    {
        private readonly string _prefix;
        private Dictionary<string, object?> _params;

        public ConstructorParameters(string prefix)
        {
            _prefix = prefix;
            _params = new Dictionary<string, object?>();
        }

        public string Add(string name, object? value)
        {
            string finalizeName = string.Concat(_prefix, name);
            _params.Add(finalizeName, value);
            return finalizeName;
        }

        public ConstructorParameters CreateChild(string prefix)
        {
            return new(string.Concat(_prefix, prefix))
            {
                _params = this._params
            };
        }

        public IEnumerator<KVP> GetEnumerator() => _params.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}