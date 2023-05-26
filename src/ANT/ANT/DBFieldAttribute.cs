using System;

using ANT.Model;
using ANT.ValueConverters;

namespace ANT
{
    public class DBFieldAttribute : Attribute
    {
        public DBFieldInfo Info { get; }
        public Type ValueConverterType { get; set; } = typeof(DefaultValueConverter);

        public DBFieldAttribute()
        {
            Info = new();
        }

        public DBFieldAttribute(string fieldName, string? dbType = null)
        {
            Info = new()
            {
                FieldName = fieldName
            };
            if (dbType != null)
                Info.DBType = dbType;
        }
    } 
}