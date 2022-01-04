using System.ComponentModel.DataAnnotations.Schema;

namespace _3_Card_Poker.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Face { get; set; } = "";
        public int Number { get; set; }
        public string Photo { get; set; } = "";
        [ForeignKey("Players")]
        public int PlayerId { get; set; }

        public Card() { }
    }
}
