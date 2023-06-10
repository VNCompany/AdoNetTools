using System;

using ANT.Model.Data;
using ANT.ValueConverters;

namespace ANT
{
    public class DBFieldAttribute : Attribute
    {
        public DBFieldInfo Info { get; }
        public Type ValueConverterType { get; set; } = typeof(DefaultValueConverter);

        public string FieldName
        {
            get => Info.FieldName;
            set => Info.FieldName = value;
        }

        public string? DBType
        {
            get => Info.DBType;
            set => Info.DBType = value;
        }

        public DBFieldAttribute()
        {
            Info = new();
        }

        public DBFieldAttribute(string fieldName, string? dbType = null)
        {
            Info = new()
            {
                FieldName = fieldName,
                DBType = dbType
            };
        }
    } 
}