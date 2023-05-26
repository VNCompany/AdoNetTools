using System;
using System.Data.Common;

using ANT.Model;

namespace ANT.ValueConverters
{
    public class DefaultValueConverter : IValueConverter
    {
        public static readonly DefaultValueConverter GetObject = new DefaultValueConverter();
        
        public virtual object? ConvertTo(DbDataReader dataReader, string fieldName, Type fieldType)
        {
            object? dataValue = dataReader.GetValue(dataReader.GetOrdinal(fieldName));
            return dataValue != DBNull.Value ? dataValue : null;
        }

        public virtual object ConvertFrom(object? fieldValue, Type fieldType) => fieldValue ?? DBNull.Value;
    }
}