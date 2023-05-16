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
    }
}