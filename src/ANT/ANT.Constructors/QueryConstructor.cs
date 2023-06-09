using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using ANT.Model;

namespace ANT.Constructors
{
    public abstract class QueryConstructor : IQueryConstructor
    {
        protected List<object> CommandParts { get; }
        
        [AllowNull]
        protected Dictionary<string, object?> Params { get; init; }
        
        protected QueryConstructor(bool withoutParams = false)
        {
            CommandParts = new List<object>();

            if (!withoutParams)
                Params = new Dictionary<string, object?>();
        }

        public virtual IEnumerable<KeyValuePair<string, object?>> GetCommandParameters() =>
            Params ?? Enumerable.Empty<KeyValuePair<string, object?>>();

        public virtual string? Build()
        {
            if (CommandParts.Count == 0) return null;

            StringBuilder queryStringBuilder = new StringBuilder();
            queryStringBuilder.AppendJoin(' ', from o in CommandParts select o.ToString());
            return queryStringBuilder.ToString() + ';';
        }
    }
}