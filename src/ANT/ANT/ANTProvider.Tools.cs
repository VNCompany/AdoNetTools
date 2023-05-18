using System.Collections.Generic;

namespace ANT
{
    public static partial class ANTProvider
    {
        public static readonly Dictionary<string, string> DBTypes = new Dictionary<string, string>();

        private static void _InitializeDBTypesDictionary()
        {
            DBTypes.Add("System.Boolean", "BOOL");
            DBTypes.Add("System.Byte", "TINYINT UNSIGNED");
            DBTypes.Add("System.Char", "TINYINT UNSIGNED");
            DBTypes.Add("System.Int16", "SMALLINT");
            DBTypes.Add("System.UInt16", "SMALLINT UNSIGNED");
            DBTypes.Add("System.Int32", "INT");
            DBTypes.Add("System.UInt32", "INT UNSIGNED");
            DBTypes.Add("System.Int64", "BIGINT");
            DBTypes.Add("System.UInt64", "BIGINT UNSIGNED");
            DBTypes.Add("System.Single", "FLOAT");
            DBTypes.Add("System.Double", "DOUBLE");
            DBTypes.Add("System.Decimal", "DECIMAL");
            DBTypes.Add("System.DateTime", "DATETIME");
        }
        
        public static string? CamelToSnake(string? input)
        {
            // TODO: Realize method "CamelToSnake"
            return input;
        }
    }
}