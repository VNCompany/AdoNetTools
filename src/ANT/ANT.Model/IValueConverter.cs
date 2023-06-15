using System;
using System.Data.Common;

namespace ANT.Model
{
    public interface IValueConverter
    {
        object? ConvertTo(object input, Type fieldType);
        object? ConvertFrom(object? input, Type fieldType);
    }
}