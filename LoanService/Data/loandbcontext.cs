using LoanService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Data;

public class loandbcontext : DbContext
{
    public loandbcontext(DbContextOptions<loandbcontext> options) : base(options) { }

    public DbSet<loans> loans { get; set; } = null!;
    public DbSet<loanapplication> loanapplication { get; set; } = null!;
    public DbSet<loanagreement> loanagreement { get; set; } = null!;
    public DbSet<loanaction> loanaction { get; set; } = null!;
    public DbSet<loaninterest> loaninterest { get; set; } = null!;
    public DbSet<penaltyinterest> penaltyinterest { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<loans>().ToTable("loans");
        builder.Entity<loanapplication>().ToTable("loanapplication");
        builder.Entity<loanagreement>().ToTable("loanagreement");
        builder.Entity<loanaction>().ToTable("loanaction");
        builder.Entity<loaninterest>().ToTable("loaninterest");
        builder.Entity<penaltyinterest>().ToTable("penaltyinterest");

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
