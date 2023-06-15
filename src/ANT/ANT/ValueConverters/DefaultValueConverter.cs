using System;
using System.Data.Common;

using ANT.Model;

namespace ANT.ValueConverters
{
    public class DefaultValueConverter : IValueConverter
    {
        public virtual object? ConvertTo(object input, Type _)
            => input != DBNull.Value ? input : null;

        public virtual object? ConvertFrom(object? input, Type _) => input;
    }
}