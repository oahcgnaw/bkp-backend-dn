namespace bkpDN.Models;

public class AccountDto
{
    public int Id { get; set; }
    public int User_id { get; set; }
    public int Amount { get; set; }
    public string? Note { get; set; }
    public int Tag_id { get; set; }
    public DateTime Happened_at { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
    public DateTime? Deleted_at { get; set; }
    public Kind Kind { get; set; }
    public Tag Tag { get; set; }
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