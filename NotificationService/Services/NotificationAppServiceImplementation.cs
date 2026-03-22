using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Interfaces;
using NotificationService.Models.DTOs;
using NotificationService.Models.Entities;

namespace NotificationService.Services;

public class NotificationAppServiceImplementation : INotificationAppService
{
    private readonly NotificationServiceDbContext _dbContext;

    public NotificationAppServiceImplementation(NotificationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateNotificationAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Message = dto.Message,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();

        return notification.Id;
    }

    public async Task<NotificationDto?> GetNotificationByIdAsync(Guid id)
    {
        var notification = await _dbContext.Notifications.FindAsync(id);
        if (notification == null) return null;

        return new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            Type = notification.Type
        };
    }
}
