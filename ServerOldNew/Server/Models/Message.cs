using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Message
{
    public int Id { get; set; }
    [Required]
    public string? Content { get; set; }
    [Required]
    public int ChatId { get; set; }
    [Required]
    public int SenderId { get; set; }
    [Required]
    public DateTime? CreatedAt { get; set; }
}
