namespace ANT
{
    public class DBPrimaryKeyAttribute : DBNotNullAttribute
    {
        public DBPrimaryKeyAttribute()
        {
            Info.IsPrimaryKey = true;
        }

        public DBPrimaryKeyAttribute(bool isAutoIncrement) : this()
        {
            Info.IsAuto = isAutoIncrement;
        }

        public DBPrimaryKeyAttribute(string fieldName, bool isAutoIncrement = false, string? dbType = null) : base(
            fieldName, dbType)
        {
            Info.IsAuto = isAutoIncrement;
        }
    }
}