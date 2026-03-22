using CustomerService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data;

public class customerdbcontext : DbContext
{
    public customerdbcontext(DbContextOptions<customerdbcontext> options) : base(options) { }

    public DbSet<customers> customers { get; set; } = null!;
    public DbSet<customeraddress> customeraddress { get; set; } = null!;
    public DbSet<customerbank> customerbank { get; set; } = null!;
    public DbSet<customeremployment> customeremployment { get; set; } = null!;
    public DbSet<customerdebt> customerdebt { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<customers>().ToTable("customers");
        builder.Entity<customeraddress>().ToTable("customeraddress");
        builder.Entity<customerbank>().ToTable("customerbank");
        builder.Entity<customeremployment>().ToTable("customeremployment");
        builder.Entity<customerdebt>().ToTable("customerdebt");

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
