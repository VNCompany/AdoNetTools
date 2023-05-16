using System;

namespace ANT.Model
{
    public class DBField
    {
        public string Name { get; }
        public string DBType { get; }
        public object? Value { get; }

        public DBField(string name, string dbType, object? value)
        {
            Name = name;
            DBType = dbType;
            Value = value;
        }

        public override string ToString()
        {
            throw new NotImplementedException("Реализовать конвертер и перевод в строку типа");
        }
    }
}