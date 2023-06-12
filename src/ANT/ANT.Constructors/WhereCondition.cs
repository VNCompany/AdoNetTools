using System.Collections.Generic;
using System.Linq;
using ANT.Model;

namespace ANT.Constructors
{
    public class WhereCondition
    {
        public string Condition { get; }

        public IEnumerable<KeyValuePair<string, object?>>? Parameters { get; set; }

        public WhereCondition(string condition)
        {
            Condition = condition;
        }

        public WhereCondition(string condition, IEnumerable<KeyValuePair<string, object?>>? parameters)
            : this(condition)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            return Condition;
        }

        private static WhereCondition ConditionsConcat(WhereCondition cond1, WhereCondition cond2, string separator)
        {
            IEnumerable<KeyValuePair<string, object?>>? parameters;
            if (cond1.Parameters != null && cond2.Parameters != null)
                parameters = cond1.Parameters.Concat(cond2.Parameters);
            else if (cond1.Parameters != null)
                parameters = cond1.Parameters;
            else if (cond2.Parameters != null)
                parameters = cond2.Parameters;
            else
                parameters = null;

            string concated = string.Concat(cond1.Condition, separator, cond2.Condition);
            return new WhereCondition(concated, parameters);
        }
        
        public static implicit operator WhereCondition(string condition)
            => new WhereCondition(condition);

        public static WhereCondition operator +(WhereCondition left, WhereCondition right)
            => ConditionsConcat(left, right, " OR ");

        public static WhereCondition operator *(WhereCondition left, WhereCondition right)
            => ConditionsConcat(left, right, " AND ");
    }
}