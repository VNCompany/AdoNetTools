using System;
using System.Collections.Generic;
using System.Text;

using ANT.Constructors.Internal;

namespace ANT.Constructors
{
    public class MultiQueryConstructor : IQueryConstructor
    {
        private readonly List<IQueryConstructor> queries = new();
        
        public MultiQueryConstructor() { }

        public MultiQueryConstructor(IEnumerable<IQueryConstructor> constructors)
            => queries.AddRange(constructors);

        public MultiQueryConstructor(params IQueryConstructor[] constructors)
            => queries.AddRange(constructors);

        public void Add(IQueryConstructor constructor) => queries.Add(constructor);

        public string Construct(ConstructorParameters parameters)
        {
            if (queries.Count == 0) throw new OperationCanceledException("Empty queries set");

            StringBuilder queryBuilder = new StringBuilder();
            for (int i = 0; i < queries.Count; i++)
                queryBuilder.Append(queries[i].Construct(parameters.CreateChild($"q{i}_")));
            return queryBuilder.ToString();
        }
    }
}