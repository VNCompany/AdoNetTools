using System;

namespace ANT.ORM.ValueConverters
{
    public class DefaultValueConverter : IValueConverter
    {
        public object? ConvertTo(object? input) => input;
        public object? ConvertFrom(object? input) => input;
    }
}