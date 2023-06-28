using System;
using System.Data.Common;

namespace ANT.ORM.ValueConverters
{
    public interface IValueConverter
    {
        object? ConvertTo(object? input);
        object? ConvertFrom(object? input);
    }
}