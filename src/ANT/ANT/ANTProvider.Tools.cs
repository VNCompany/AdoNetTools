using System;
using System.Collections.Generic;

namespace ANT
{
    public static partial class ANTProvider
    {
        public static readonly Dictionary<Type, string> DBTypes = new Dictionary<Type, string>
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
            { typeof(String), "TEXT" },
        };
        
        public static string? GetDBType(Type type)
        {
            if (!DBTypes.TryGetValue(type, out var dbType)
                && type.GenericTypeArguments.Length == 1)
            {
                DBTypes.TryGetValue(type.GenericTypeArguments[0], out dbType);
            }
            
            return dbType;
        }
        
        public static string? CamelToSnake(string? input)
        {
            // TODO: Realize method "CamelToSnake"
            return input;
        }
    }
}