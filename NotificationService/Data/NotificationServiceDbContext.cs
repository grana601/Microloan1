using Microsoft.EntityFrameworkCore;
using NotificationService.Models.Entities;

namespace NotificationService.Data;

public class NotificationServiceDbContext : DbContext
{
    public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}
