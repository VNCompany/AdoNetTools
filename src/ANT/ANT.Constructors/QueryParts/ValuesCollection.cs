using System.Linq;
using System.Collections.Generic;

namespace ANT.Constructors.QueryParts
{
    public class ValuesCollection : List<object?>
    {
        public class CollectionConfig
        {
            public string Start { get; set; } = "(";
            public string End { get; set; } = ")";
            public string Separator { get; set; } = ",";
        }

        public readonly CollectionConfig Config = new CollectionConfig();

        public ValuesCollection() { }
        
        public ValuesCollection(IEnumerable<object?> values) : base(values) { }

        public override string ToString()
        {
            string combined = string.Join(Config.Separator, from item in this 
                select item is null ? "NULL" : item.ToString());
            return $"{Config.Start}{combined}{Config.End}";
        }
    }
}