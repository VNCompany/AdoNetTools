namespace ANT.ORM.Models
{
    public interface IDbEntity
    {
        DbEntityMetadata Metadata { get; }
    }
}