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
            });
        }
    }
}
