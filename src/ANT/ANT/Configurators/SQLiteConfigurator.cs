using System;
using System.Collections.Generic;

using ANT.Model;

namespace ANT.Configurators
{
    public class SQLiteConfigurator : IANTConfigurator
    {
        private readonly Dictionary<Type, string> dbTypes = new Dictionary<Type, string>()
        {
            { typeof(Boolean), "INTEGER" },
            { typeof(Byte), "INTEGER" },
            { typeof(Char), "TEXT" },
            { typeof(Int16), "INTEGER" },
            { typeof(UInt16), "INTEGER" },
            { typeof(Int32), "INTEGER" },
            { typeof(UInt32), "INTEGER" },
            { typeof(Int64), "INTEGER" },
            { typeof(UInt64), "INTEGER" },
            { typeof(Single), "REAL" },
            { typeof(Double), "REAL" },
            { typeof(Decimal), "TEXT" },
            { typeof(DateTime), "TEXT" },
            { typeof(TimeOnly), "TEXT" },
            { typeof(TimeSpan), "TEXT" },
            { typeof(String), "TEXT" }
        };

        public IReadOnlyDictionary<Type, string> DBTypes => dbTypes;

        public string AutoIncrementDefinition => "AUTOINCREMENT";
    }
}