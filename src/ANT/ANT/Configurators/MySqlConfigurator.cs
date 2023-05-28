using System;
using System.Collections.Generic;

using ANT.Model;

namespace ANT.Configurators
{
    public class MySqlConfigurator : IANTConfigurator
    {
        private readonly Dictionary<Type, string> dbTypes = new Dictionary<Type, string>()
        {
            { typeof(Boolean), "BOOL" },
            { typeof(Byte), "TINYINT UNSIGNED" },
            { typeof(Char), "TINYINT UNSIGNED" },
            { typeof(Int16), "SMALLINT" },
            { typeof(UInt16), "SMALLINT UNSIGNED" },
            { typeof(Int32), "INT" },
            { typeof(UInt32), "INT UNSIGNED" },
            { typeof(Int64), "BIGINT" },
            { typeof(UInt64), "BIGINT UNSIGNED" },
            { typeof(Single), "FLOAT" },
            { typeof(Double), "DOUBLE" },
            { typeof(Decimal), "DECIMAL" },
            { typeof(DateTime), "DATETIME" },
            { typeof(TimeOnly), "TIME" },
            { typeof(TimeSpan), "TIME" },
            { typeof(String), "TEXT" }
        };

        public IReadOnlyDictionary<Type, string> DBTypes => dbTypes;

        public string AutoIncrementDefinition => "AUTO_INCREMENT";
    }
}