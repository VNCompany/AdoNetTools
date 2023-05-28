using System;
using System.Collections.Generic;

namespace ANT.Model
{
    public interface IANTConfigurator
    {
        IReadOnlyDictionary<Type, string> DBTypes { get; }
        string AutoIncrementDefinition { get; }
    }
}