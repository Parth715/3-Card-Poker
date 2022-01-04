using System.ComponentModel.DataAnnotations.Schema;

namespace _3_Card_Poker.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public virtual IEnumerable<Card> Cards { get; set; }
        public Player() { }
    }
}
