using System;

namespace ANT
{
    public class DBFieldAttribute : Attribute
    {
        public string? FieldName { get; set; }
        
        public string? DBType { get; set; }
        public bool IsNotNull { get; set; }

        private bool isPrimaryKey;

        public bool IsPrimaryKey
        {
            get => isPrimaryKey;
            set
            {
                isPrimaryKey = value;
                if (value)
                    IsNotNull = true;
            }
        }
        
        public Type? ValueConverterType { get; set; }

        public DBFieldAttribute() { }

        public DBFieldAttribute(string? fieldName, string? dbType = null)
        {
            FieldName = fieldName;
            DBType = dbType;
        }
    } 
}