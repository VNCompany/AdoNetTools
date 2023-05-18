using System;

namespace ANT
{
    public class DBFieldAttribute : Attribute
    {
        public string? FieldName { get; set; }
        
        public string? DBType { get; set; }
        public bool IsNotNull { get; set;  } = false;
        public bool IsPrimaryKey { get; set; } = false;
        
        public Type? ValueConverterType { get; set; }

        public DBFieldAttribute() { }

        public DBFieldAttribute(string? fieldName, string? dbType = null)
        {
            FieldName = fieldName;
            DBType = dbType;
        }
    } 
}