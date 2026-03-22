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
        }
    }
}
