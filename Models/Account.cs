using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Note { get; set; }
    
    public int Tag_id { get; set; }
    [ForeignKey("Tag_id")]
    public Tag Tag { get; set; }
    
    public Kind Kind { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    
}
