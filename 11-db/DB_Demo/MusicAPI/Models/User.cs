using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicAPI.Models;

public class User
{
    [Key] public int UserId { get; set; }
    [Required] public string Name { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
    [Required] public string PasswordHash { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Vinyl
{
    [Key]
    [Column("record_id")]
     public int RecordId { get; set; }

     [Column("title")]
    [Required] public string Title { get; set; }

     [Column("artist")]
    [Required] public string Artist { get; set; }

     [Column("release_year")]
    public int? ReleaseYear { get; set; }

    [Required] 
    [Column("price")]
    public double Price { get; set; }

     [Column("stock")]
    public int Stock { get; set; } = 0;

     [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Order
{
    [Key] public int OrderId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public double TotalPrice { get; set; }
    [Required] public string Status { get; set; } = "Created";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
