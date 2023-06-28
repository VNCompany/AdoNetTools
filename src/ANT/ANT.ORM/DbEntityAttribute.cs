using System;

namespace ANT.ORM
{
    public class DbEntityAttribute : Attribute
    {
        public string? Name { get; set; }

        public DbEntityAttribute() { }

        public DbEntityAttribute(string name)
        {
            Name = name;
        }
    } 
}