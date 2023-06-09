using System.Collections.Generic;

namespace ANT.Model
{
    public interface IQueryConstructor
    {
        IEnumerable<KeyValuePair<string, object?>> GetCommandParameters();
        string? Build();
    }
}