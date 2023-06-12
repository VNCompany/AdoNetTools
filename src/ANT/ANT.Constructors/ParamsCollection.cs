using System.Collections.Generic;

namespace ANT.Constructors
{
    public class ParamsCollection : Dictionary<string, object?>
    {
        public ParamsCollection() { }
        public ParamsCollection(IEnumerable<KeyValuePair<string, object?>> items) : base(items) { }
    }
}