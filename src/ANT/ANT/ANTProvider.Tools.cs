using System;
using System.Linq;
using System.Collections.Generic;

namespace ANT
{
    public static partial class ANTProvider
    {
        public static string? GetDBType(Type type)
        {
            if (!Configuration.DBTypes.TryGetValue(type, out var dbType)
                && type.GenericTypeArguments.Length == 1)
            {
                Configuration.DBTypes.TryGetValue(type.GenericTypeArguments[0], out dbType);
            }
            
            return dbType;
        }

        private static readonly string[] __worldEndings = { "o", "i", "x", "z", "ch", "sh", "ss" };
        private const string __consonantLetters = "bcdfjhjklmnpqrstvwxz";
        private static string ModifyEntityName(string input)
        {
            if (input.Length > 1)
            {
                char[] ch = new char[input.Length + 2];
                input.CopyTo(0, ch, 0, input.Length);
                int chEndPtr = input.Length;

                if (ch[chEndPtr - 1] == 'y' && __consonantLetters.Contains(ch[chEndPtr - 2]))
                    ch[chEndPtr - 1] = 'i';
                if (__worldEndings.Take(4).Contains(ch[chEndPtr - 1].ToString())
                    || __worldEndings.Skip(4).Contains(new string(ch, chEndPtr - 2, 2)))
                {
                    ch[chEndPtr++] = 'e';
                    ch[chEndPtr++] = 's';
                }
                else ch[chEndPtr++] = 's';

                return CamelToSnake(new string(ch, 0, chEndPtr))!;
            }
            return CamelToSnake(input)!;
        }
        
        private static string? CamelToSnake(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            char[] ch = new char[input.Length * 2];
            int j = 0;
            for (int i = 0; i < input.Length; i++, j++)
            {
                if (i == 0) { ch[0] = char.ToLower(input[i]); continue; }

                if (char.IsUpper(input[i]))
                    (ch[j++], ch[j]) = ('_', char.ToLower(input[i]));
                else
                    ch[j] = input[i];
            }

            return new string(ch, 0, j);
        }
    }
}