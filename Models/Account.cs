using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bkpDN.Models;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int User_id { get; set; }
    [ForeignKey("User_id")]
    public User User { get; set; }
    
    public int Amount { get; set; }
    public string? Note { get; set; }
    
    public int Tag_id { get; set; }
    [ForeignKey("Tag_id")]
    [JsonIgnore]
    public Tag Tag { get; set; }
    
    public Kind Kind { get; set; }
    public DateTime Happened_at { get; set; } = DateTime.UtcNow;
    public DateTime Created_at { get; set; } = DateTime.UtcNow;
    public DateTime Updated_at { get; set; } = DateTime.UtcNow;
    public DateTime? Deleted_at { get; set; }
    
    
}
