using System.Reflection;

namespace ANT.Model
{
    public class DBFieldMetadata
    {
        public string FieldName { get; }
        public PropertyInfo PropertyInfo { get; }
        
        public string DBType { get; }
        public bool IsNotNull { get; }
        public bool IsPrimaryKey { get; }

        public DBFieldMetadata(string fieldName, PropertyInfo propertyInfo, string dbType, bool isNotNull, bool isPrimaryKey)
        {
            FieldName = fieldName;
            PropertyInfo = propertyInfo;
            DBType = dbType;
            IsNotNull = isNotNull;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}