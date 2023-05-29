using System.Text;
using System.Collections.Generic;
using System.Linq;
using ANT.Model;

namespace ANT.Constructors
{
    public class QueryConstructor : IQueryConstructor
    {
        private readonly Dictionary<string, object?>? _params;

        public IDictionary<string, object?>? CommandParameters => _params;
        
        public List<object>? CommandParts { get; }

        public QueryConstructor(bool withoutParameters, bool withoutBody = false)
        {
            if (!withoutParameters)
                _params = new Dictionary<string, object?>();
            if (!withoutBody)
                CommandParts = new List<object>();
        }

        public QueryConstructor() : this(false) { }

        public virtual string? Build()
        {
            if (CommandParts == null || CommandParts.Count == 0) return null;

            StringBuilder queryStringBuilder = new StringBuilder();
            queryStringBuilder.AppendJoin(' ', from o in CommandParts select o.ToString());

            return queryStringBuilder.ToString() + ';';
        }
    }
}