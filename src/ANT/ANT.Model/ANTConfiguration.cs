using System;
using System.Diagnostics.CodeAnalysis;

namespace ANT.Model
{
    public static class ANTConfiguration
    {
        public static IANTConfigurator? Configurator = null;

        public static IANTConfigurator GetConfiguration() =>
            Configurator ?? throw new InvalidOperationException("AdoNetTools isn't configured");
    }
}