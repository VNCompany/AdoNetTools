using System.Linq;
using System.Collections.Generic;

namespace ANT.Constructors.QueryParts
{
    public class KeyValuePairsCollection : List<KeyValuePair<string, object?>>
    {
        public KeyValuePairsCollection() { }
        
        public KeyValuePairsCollection(IEnumerable<KeyValuePair<string, object?>> values) : base(values) { }

        public void Add(string key, object? value)
        {
            Add(new KeyValuePair<string, object?>(key, value));
        }

        public override string ToString()
        {
            if (Count == 0) return string.Empty;
            
            return string.Join(",", from item in this 
                select $"{item.Key}={item.Value ?? "NULL"}");
        }
    }
}