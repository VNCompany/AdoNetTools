using System;
using System.Collections.Generic;

namespace ANT
{
    public static partial class ANTProvider
    {
        public static readonly Dictionary<Type, string> DBTypes = new Dictionary<Type, string>();

        private static void _InitializeDBTypesDictionary()
        {
            DBTypes.Add(typeof(Boolean), "BOOL");
            DBTypes.Add(typeof(Byte), "TINYINT UNSIGNED");
            DBTypes.Add(typeof(Char), "TINYINT UNSIGNED");
            DBTypes.Add(typeof(Int16), "SMALLINT");
            DBTypes.Add(typeof(UInt16), "SMALLINT UNSIGNED");
            DBTypes.Add(typeof(Int32), "INT");
            DBTypes.Add(typeof(UInt32), "INT UNSIGNED");
            DBTypes.Add(typeof(Int64), "BIGINT");
            DBTypes.Add(typeof(UInt64), "BIGINT UNSIGNED");
            DBTypes.Add(typeof(Single), "FLOAT");
            DBTypes.Add(typeof(Double), "DOUBLE");
            DBTypes.Add(typeof(Decimal), "DECIMAL");
            DBTypes.Add(typeof(DateTime), "DATETIME");
            DBTypes.Add(typeof(String), "TEXT");
        }
        
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