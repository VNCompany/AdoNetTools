using System.Collections.Generic;
using System.Linq;

using ANT.Model;

namespace ANT.Constructors
{
    public class MultiQueryConstructor : IQueryConstructor
    {
        private readonly IEnumerable<IQueryConstructor> _constructors;

        public MultiQueryConstructor(IEnumerable<IQueryConstructor> queryConstructors)
        {
            _constructors = queryConstructors;
        }

        public MultiQueryConstructor(params IQueryConstructor[] queryConstructors)
        {
            _constructors = queryConstructors;
        }

        public IEnumerable<KeyValuePair<string, object?>> GetCommandParameters()
        {
            List<KeyValuePair<string, object?>> result = new List<KeyValuePair<string, object?>>();
            int prefix = 1;
            foreach (var constructor in _constructors)
            {
                foreach (var (paramName, paramValue) in constructor.GetCommandParameters())
                    result.Add(new KeyValuePair<string, object?>($"p{prefix}_{paramName}", paramValue));
                prefix++;
            }

            return result;
        }

        public string? Build() => string.Join(";", _constructors.Select(c => c.Build()).Where(q => q != null)) + ";";
    }
}