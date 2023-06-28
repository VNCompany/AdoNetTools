namespace ANT.ORM.Tools
{
    internal static class NameConversions
    {
        private static readonly string ConsonantLetters = "bcdfghjklmnpqrstvwxz";
        
        public static string CamelToSnakeNamingStyle(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            char[] ch = new char[input.Length * 2];
            ch[0] = char.ToLower(input[0]);
            
            int chi = 1;
            for (int i = 1; i < input.Length; i++, chi++)
            {
                if (char.IsUpper(input[i]))
                    (ch[chi++], ch[chi]) = ('_', char.ToLower(input[i]));
                else
                    ch[chi] = input[i];
            }

            return new string(ch, 0, chi);
        }

        public static string ToPlural(string input, bool camelToSnake = true)
        {
            if (camelToSnake)
                input = CamelToSnakeNamingStyle(input);

            char[] buf = new char[input.Length + 2];
            input.CopyTo(0, buf, 0, input.Length);
            int lastCharPtr = input.Length - 1;

            if (buf[lastCharPtr] == 's' || buf[lastCharPtr] == 'x' || buf[lastCharPtr] == 'z' || buf[lastCharPtr] == 'o'
                || _EqualsParts(buf, lastCharPtr - 1, "sh")
                || _EqualsParts(buf, lastCharPtr - 1, "ch")
                || _EqualsParts(buf, lastCharPtr - 1, "ss"))
            {
                _WritePart(buf, lastCharPtr + 1, "es");
                lastCharPtr += 2;
            }
            else if (buf[lastCharPtr] == 'y' && ConsonantLetters.Contains(buf[lastCharPtr - 1]))
            {
                _WritePart(buf, lastCharPtr, "ies");
                lastCharPtr += 2;
            }
            else if (buf[lastCharPtr] == 'f' || _EqualsParts(buf, lastCharPtr - 1, "fe"))
            {
                _WritePart(buf, lastCharPtr, "ves");
                lastCharPtr += 2;
            }
            else
                buf[++lastCharPtr] = 's';

            return new string(buf, 0, lastCharPtr + 1);
        }

        private static bool _EqualsParts(char[] buf, int start, string part)
        {
            for (int i = start, j = 0; j < part.Length && i < buf.Length; j++)
                if (buf[i] != part[j]) return false;
            return true;
        }

        private static void _WritePart(char[] buf, int start, string part)
        {
            for (int i = start, j = 0; j < part.Length && i < buf.Length; i++, j++)
                buf[i] = part[j];
        }
    }
}