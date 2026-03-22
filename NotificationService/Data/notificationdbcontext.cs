using NotificationService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Data;

public class notificationdbcontext : DbContext
{
    public notificationdbcontext(DbContextOptions<notificationdbcontext> options) : base(options) { }

    public DbSet<notifications> notifications { get; set; } = null!;
    public DbSet<notificationlog> notificationlog { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<notifications>().ToTable("notifications");
        builder.Entity<notificationlog>().ToTable("notificationlog");

        // Iterate over all entities and convert tables, columns, constraints and indexes to lowercase
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var currentTableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(currentTableName))
            {
                entityType.SetTableName(currentTableName.ToLower());
            }

            foreach (var property in entityType.GetProperties())
            {
                var currentColumnName = property.GetColumnName(Microsoft.EntityFrameworkCore.Metadata.StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema()));
                if (string.IsNullOrEmpty(currentColumnName))
                {
                    currentColumnName = property.GetColumnName(); // Fallback
                }

                if (!string.IsNullOrEmpty(currentColumnName))
                {
                    property.SetColumnName(currentColumnName.ToLower());
                }
            }

            foreach (var key in entityType.GetKeys())
            {
                key.SetName(key.GetName()?.ToLower());
            }

            foreach (var fk in entityType.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName()?.ToLower());
            }

            foreach (var index in entityType.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToLower());
            }
        }
    }
}
