using System.Collections;
using System.Collections.Generic;

namespace ANT.Model.Data.MappingModels
{
    public class EntityFieldData
    {
        public string Name { get; }
        public DBFieldInfo Info { get; }
        public object? Value { get; }

        public EntityFieldData(string name, DBFieldInfo info, object? value)
        {
            Name = name;
            Info = info;
            Value = value;
        }
    }
}

