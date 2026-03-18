using System.ComponentModel.DataAnnotations;

namespace VacationMode.Models;

public class Message
{
    [Key]
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public int PropertyId { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; }

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
    public Property Property { get; set; } = null!;
}