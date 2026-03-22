using NotificationService.Models.DTOs;

namespace NotificationService.Interfaces;

public interface INotificationAppService
{
    Task<Guid> CreateNotificationAsync(CreateNotificationDto dto);
    Task<NotificationDto?> GetNotificationByIdAsync(Guid id);
}
