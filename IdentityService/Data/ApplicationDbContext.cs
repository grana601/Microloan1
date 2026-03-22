using IdentityService.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<refreshtoken> refreshtokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<refreshtoken>(entity =>
            {
                entity.ToTable("refreshtoken");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.userid).HasColumnName("userid").IsRequired();
                entity.Property(e => e.token).HasColumnName("token").IsRequired();
                entity.Property(e => e.expirydate).HasColumnName("expirydate");
                entity.Property(e => e.isrevoked).HasColumnName("isrevoked");
                entity.Property(e => e.createdat).HasColumnName("createdat");
            });

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
}
