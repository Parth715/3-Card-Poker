using _3_Card_Poker.Models;
using Microsoft.EntityFrameworkCore;

namespace _3_Card_Poker
{
    public class PokerDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }

        public PokerDbContext(DbContextOptions<PokerDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Player>(p =>
            {
                p.ToTable("Players");
                p.HasKey(p => p.Id);
                p.Property(p => p.Name);
                p.Property(p => p.Balance);
                p.Property(p => p.Outcome);
            });
            builder.Entity<Card>(p =>
            {
                p.ToTable("Cards");
                p.HasKey(p => p.Id);
                p.Property(p => p.Face);
                p.Property(p => p.Number);
                p.Property(p => p.Photo);
            });
        }
    }
}
