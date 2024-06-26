namespace bkpDN.Models;

public class TagDto
{
    public int Id { get; set; }
    public int User_id { get; set; }
    public string Name { get; set; }
    public string Sign { get; set; }
    public Kind Kind { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
    public DateTime? Deleted_at { get; set; }
}

public class TagCreationDto
{
    public Kind Kind { get; set; }
    public string Sign { get; set; }
    public string Name { get; set; }
}