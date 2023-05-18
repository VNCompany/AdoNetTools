using System;
using System.Data.Common;

namespace ANT.ValueConverters
{
    public class DefaultValueConverter : IValueConverter
    {
        public object? ConvertTo(DbDataReader dataReader, string fieldName, Type fieldType)
        {
            int ordinal = dataReader.GetOrdinal(fieldName);
            
            Type dataType = dataReader.GetFieldType(ordinal);
            if (!fieldType.IsAssignableFrom(dataType) && dataType != typeof(DBNull))
                throw new InvalidCastException($"The field type ({dataType.Name}) and property type ({fieldType}) are different");
            object? dataValue = dataReader.GetValue(ordinal);
            
            return dataValue != DBNull.Value ? dataValue : null;
        }

        public object? ConvertFrom(object? fieldValue, Type fieldType) => fieldValue;
    }
}