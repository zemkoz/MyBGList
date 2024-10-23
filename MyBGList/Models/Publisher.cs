using System.ComponentModel.DataAnnotations;

namespace MyBGList.Models;

public class Publisher
{
    [Key]
    public int Id { get; set; }

    [Required] 
    [MaxLength(200)] 
    public string Name { get; set; } = null!;
    
    [Required]
    public DateTime CreatedDate { get; set; }
    
    [Required]
    public DateTime LastModifiedDate { get; set; }
    
    public ICollection<BoardGame>? BoardGames { get; set; }
}