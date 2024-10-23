using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models;

[Table("BoardGames_Domains")]
public class BoardGame2Domain
{
    [Key]
    [Required]
    public int BoardGameId { get; set; }

    [Key]
    [Required]
    public int DomainId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
    
    public BoardGame? BoardGame { get; set; }
    public Domain? Domain { get; set; }
}