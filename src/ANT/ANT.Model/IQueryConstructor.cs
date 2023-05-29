using System.Collections.Generic;

namespace ANT.Model
{
    public interface IQueryConstructor
    {
        IDictionary<string, object?>? CommandParameters { get; }
        string? Build();
    }
}