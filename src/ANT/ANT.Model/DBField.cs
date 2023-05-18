using System;

namespace ANT.Model
{
    public class DBField
    {
        public string Name { get; }
        public object? Value { get; }

        public Type ValueConverterType { get; }

        public DBField(string name, object? value, Type valueConverterType)
        {
            Name = name;
            Value = value;
            ValueConverterType = valueConverterType;
        }
    }
}