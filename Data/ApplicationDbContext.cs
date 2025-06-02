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
            // Unique constraint for Hero Rank
            modelBuilder.Entity<Hero>().HasIndex(h => h.Rank).IsUnique();
        }
    }
}