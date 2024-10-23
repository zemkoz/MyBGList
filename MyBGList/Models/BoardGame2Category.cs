using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models;

[Table("BoardGames_Categories")]
public class BoardGame2Category
{
    [Key]
    [Required]
    public int BoardGameId { get; set; }

    [Key]
    [Required]
    public int CategoryId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
    
    public BoardGame? BoardGame { get; set; }
    public Category? Category { get; set; }
}