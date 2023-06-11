using System.Collections.Generic;

namespace ANT.Model
{
    public interface IQueryConstructor
    {
        string ParametersPrefix { get; set; }
        IEnumerable<KeyValuePair<string, object?>> GetCommandParameters();
        string? Build();
    }
}