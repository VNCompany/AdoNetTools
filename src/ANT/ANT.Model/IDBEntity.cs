namespace ANT.Model
{
    public interface IDBEntity
    {
        DBEntityMetadata? Metadata { get; }
        
        // Getting collection of fields
    }
}