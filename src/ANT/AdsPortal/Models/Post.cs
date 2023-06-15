using ANT;

namespace AdsPortal.Models;

public class Post : DBEntity
{
    [DBPrimaryKey] public uint Id { get; set; }
    public uint UserId { get; set; }
    public uint CatId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Price { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime Date { get; set; }

    [DBIgnore] public User Owner { get; set; } = null!;
    [DBIgnore] public List<string>? Pictures { get; set; }
    [DBIgnore] public string MainPicture => Pictures is { Count: > 0 } ? Pictures[0] : "noimage.jpg";

    public Post() { }
}