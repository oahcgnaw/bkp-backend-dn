using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bkpDN.Models;

public class ValidationCode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public required string User_email { get; set; }
    
    [MaxLength(6)]
    public required string Validation_code { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
}