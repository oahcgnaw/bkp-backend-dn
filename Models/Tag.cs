using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bkpDN.Models;

public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int User_id { get; set; }
    [ForeignKey("User_id")]
    public User User { get; set; } // Navigation property
    
    public string Name { get; set; }
    public string Sign { get; set; }
    
    public DateTime? Deleted_at { get; set; }
    public DateTime Created_at { get; set; } = DateTime.Now;
    public DateTime Updated_at { get; set; } = DateTime.Now;
    
    public Kind Kind { get; set; }
    
    public ICollection<Account> Accounts { get; set; }
    
}

public enum Kind
{
    Income,
    Expenses
}
