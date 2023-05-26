namespace ANT
{
    public class DBNotNullAttribute : DBFieldAttribute
    {
        public DBNotNullAttribute()
        {
            Info.IsNotNull = true;
        }
        
        public DBNotNullAttribute(string fieldName, string? dbType = null) : base(fieldName, dbType)
        {
            Info.IsNotNull = true;
        }
    }
}