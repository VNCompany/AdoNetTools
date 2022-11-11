using System;

namespace Vnc.Ado
{
    public class DbEntityAttribute : Attribute
    {
        public string TableName { get; set; }

        public DbEntityAttribute() { }

        public DbEntityAttribute(string tableName)
        {
            TableName = tableName;
        }

    }
}