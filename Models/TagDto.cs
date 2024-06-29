namespace bkpDN.Models;

public class TagDto
{
    public required int Id { get; set; }
    public required int User_id { get; set; }
    public required string Name { get; set; }
    public required string Sign { get; set; }
    public required Kind Kind { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public required DateTime? Deleted_at { get; set; }
}

public class TagCreationDto
{
    public required Kind Kind { get; set; }
    public required string Sign { get; set; }
    public required string Name { get; set; }
}