using System;

namespace Vnc.Ado
{
    public class DbFieldAttribute : Attribute
    {
        public string FieldName { get; set; }
        public Type ConverterType { get; set; } = null;

        public DbFieldAttribute() { }

        public DbFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}