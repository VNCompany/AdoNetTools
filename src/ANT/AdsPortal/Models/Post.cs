namespace AdsPortal.Models;

public class Post
{
    public uint Id { get; set; }
    public uint UserId { get; set; }
    public uint CatId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string PictureFileName { get; set; }

    public Post(uint id, uint userId, uint catId, string name, string description, 
        string price, string location, DateTime date, string pictureFileName)
    {
        Id = id;
        UserId = userId;
        CatId = catId;
        Name = name;
        Description = description;
        Price = price;
        Location = location;
        Date = date;
        PictureFileName = pictureFileName;
    }
}