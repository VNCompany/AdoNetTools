using System.Linq;
using System.Collections.Generic;

namespace ANT.Constructors.QueryParts
{
    public class ValuesCollection : List<object?>
    {
        public ValuesCollection() { }
        
        public ValuesCollection(IEnumerable<object?> values) : base(values) { }

        public override string ToString()
        {
            string combined = string.Join(",", from item in this 
                select item is null ? "NULL" : item.ToString());
            return $"({combined})";
        }
    }
}