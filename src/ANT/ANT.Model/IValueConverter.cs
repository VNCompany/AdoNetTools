using System;
using System.Data.Common;

namespace ANT.Model
{
    public interface IValueConverter
    {
        object? ConvertTo(DbDataReader dataReader, string fieldName, Type fieldType);
        object ConvertFrom(object? fieldValue, Type fieldType);
    }
}