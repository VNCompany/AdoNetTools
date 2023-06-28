using System.Reflection;

using ANT.ORM.ValueConverters;
using ANT.ORM.Services;
using ANT.ORM.Tools;

namespace ANT.ORM.Models
{
    public class DbFieldMetadata
    {
        private readonly PropertyInfo _propInfo;

        public string PropertyName => _propInfo.Name;
        
        public string Name { get; }
        
        public bool IsPrimaryKey { get; }
        
        public IValueConverter Converter { get; }

        public DbFieldMetadata(PropertyInfo propertyInfo)
        {
            _propInfo = propertyInfo;
            Name = NameConversions.CamelToSnakeNamingStyle(PropertyName);
            Converter = ConvertersService.GetConverter(typeof(DefaultValueConverter));
        }

        public DbFieldMetadata(PropertyInfo propertyInfo, DbFieldAttribute attr)
        {
            _propInfo = propertyInfo;
            Name = attr.Name ?? NameConversions.CamelToSnakeNamingStyle(PropertyName);
            IsPrimaryKey = attr.IsPrimaryKey;
            Converter = ConvertersService.GetConverter(attr.Converter ?? typeof(DefaultValueConverter));
        }

        public object? GetValue(IDbEntity entity, bool useConverter = true)
        {
            object? value = _propInfo.GetValue(entity);
            return useConverter ? Converter.ConvertFrom(value) : value;
        }

        public void SetValue(IDbEntity entity, object? value, bool useConverter = true)
        {
            _propInfo.SetValue(entity, useConverter ? Converter.ConvertTo(value) : value);
        }
    }
}