namespace bkpDN.Models;

public class AccountDto
{
    public required int Id { get; set; }
    public required int User_id { get; set; }
    public required int Amount { get; set; }
    public required string? Note { get; set; }
    public required int Tag_id { get; set; }
    public required DateTime Happened_at { get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public required DateTime? Deleted_at { get; set; }
    public required Kind Kind { get; set; }
    public required Tag Tag { get; set; }
}

public class AccCreationDto
{
    public int Amount { get; set; }
    public DateTimeOffset Happened_at { get; set; }
    public Kind Kind { get; set; }
    public int Tag_id { get; set; }
}


public enum GroupByOption
{
    happened_at,
    tag_id
}