namespace NotificationService.Models.DTOs;

public class CreateNotificationDto
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
