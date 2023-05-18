using System;

namespace ANT
{
    public class DBEntityAttribute : Attribute
    {
        public string? TableName { get; set; }

        public DBEntityAttribute() { }

        public DBEntityAttribute(string tableName)
        {
            TableName = tableName;
        }
    } 
}