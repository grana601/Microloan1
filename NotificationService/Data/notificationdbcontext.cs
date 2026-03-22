using NotificationService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Data;

public class notificationdbcontext : DbContext
{
    public notificationdbcontext(DbContextOptions<notificationdbcontext> options) : base(options) { }

    public DbSet<communication> communication { get; set; } = null!;
    public DbSet<inappnotification> inappnotification { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<communication>().ToTable("communication");
        modelbuilder.Entity<inappnotification>().ToTable("inappnotification");
    }
}
