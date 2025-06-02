using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Quirk> Quirks { get; set; }
        public DbSet<Villain> Villains { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Soft delete query filter
            modelBuilder.Entity<Hero>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<Quirk>().HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Villain>().HasQueryFilter(v => !v.IsDeleted);
            modelBuilder.Entity<Item>().HasQueryFilter(i => !i.IsDeleted);

            // Relationships
            modelBuilder.Entity<Hero>()
                .HasOne(h => h.Quirk)
                .WithMany(q => q.Heroes)
                .HasForeignKey(h => h.QuirkId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Villain>()
                .HasOne(v => v.Quirk)
                .WithMany(q => q.Villains)
                .HasForeignKey(v => v.QuirkId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
