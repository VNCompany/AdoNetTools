using System;

namespace ANT.ORM
{
    public class DbFieldAttribute : Attribute
    {
        public string? Name { get; set; }
        
        public bool IsPrimaryKey { get; set; }
        
        public Type? Converter { get; set; }
        
        public DbFieldAttribute() { }

        public DbFieldAttribute(string name, bool isPrimaryKey = false)
        {
            Name = name;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}