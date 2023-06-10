using System;
using System.Data.Common;

using ANT.Model;

namespace ANT.ValueConverters
{
    public class DefaultValueConverter : IValueConverter
    {
        public virtual object? ConvertTo(DbDataReader dataReader, string fieldName, Type _)
        {
            object? dataValue = dataReader.GetValue(dataReader.GetOrdinal(fieldName));
            return dataValue != DBNull.Value ? dataValue : null;
        }

        public virtual object? ConvertFrom(object? fieldValue, Type _) => fieldValue;
    }
}