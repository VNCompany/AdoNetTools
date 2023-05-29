using System.Reflection;

namespace ANT.Model
{
    public class DBFieldMetadata
    {
        public DBFieldInfo Info { get; }
        public PropertyInfo PropertyInfo { get; }
        public IValueConverter Converter { get; }

        public DBFieldMetadata(DBFieldInfo fieldInfo, PropertyInfo propertyInfo, IValueConverter converter)
        {
            Info = fieldInfo;
            PropertyInfo = propertyInfo;
            Converter = converter;
            
            Info.Freeze();
        }

        public override string ToString() => Info.ToString();
    }
}