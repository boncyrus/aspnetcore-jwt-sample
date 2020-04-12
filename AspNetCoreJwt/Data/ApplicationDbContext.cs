using AspNetCoreJwt.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreJwt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TokenEntity> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TokenEntity>().HasKey(x => x.Id);
            builder.Entity<TokenEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.Entity<UserEntity>().HasKey(x => x.Id);
            builder.Entity<UserEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<UserEntity>().HasMany(x => x.RefreshTokens).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            base.OnModelCreating(builder);
        }
    }
}