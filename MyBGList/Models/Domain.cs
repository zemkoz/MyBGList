﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models;

[Table("Domains")]
public class Domain
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;
    
    [MaxLength(200)]
    public string? Notes { get; set; }
    
    [Required]
    public int Flags { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }
    
    public ICollection<BoardGame2Domain>? BoardGames2Domains { get; set; }
}