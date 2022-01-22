#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3_Card_Poker;
using _3_Card_Poker.Models;

namespace _3_Card_Poker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly PokerDbContext _context;

        public CardsController(PokerDbContext context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards.ToListAsync();
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }
        
        [HttpPut("Start")]
        public async Task<ActionResult> StartGame()
        {
            Random random = new Random();
            for(var i = 1; i <=3; i++)
            {
                var card = random.Next(2, 54);
                var play = await _context.Cards.FindAsync(card);
                play.PlayerId = 1;
                await _context.SaveChangesAsync();
            }
            
            return NoContent();
        }

        [HttpPut("check/{player}")]
        public async Task<ActionResult> Check(Player player)
        {
            Random random = new Random();
            for (var i = 1; i <= 3; i++)
            {
                var card = random.Next(2, 54);
                var play = await _context.Cards.FindAsync(card);
                play.PlayerId = player.Id;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        [HttpPut("Hit/{player}")]
        public async Task<ActionResult> Hit(Player player, Player dealer)
        {
            bool HighCard = false;
            bool Pair = false;
            bool Flush = false;
            bool Straight = false;
            bool ThreeOfKind = false;
            bool StraightFlush = false;
            bool DHighCard = false;
            bool DPair = false;
            bool DFlush = false;
            bool DStraight = false;
            bool DThreeOfKind = false;
            bool DStraightFlush = false;
            var hand = 0;
            var dealer_hand = 0;
            List<string> face = new List<string>();
            List<int> cardnum = new List<int>();
            List<string> Dface = new List<string>();
            List<int> Dcardnum = new List<int>();
            Random random = new Random();
            for (var i = 1; i <= 3; i++)
            {
                var card = random.Next(2, 54);
                var play = await _context.Cards.FindAsync(card);
                play.PlayerId = dealer.Id;
                await _context.SaveChangesAsync();
            }
            foreach (Card card in player.Cards)
            {
                face.Append(card.Face);
                cardnum.Append(card.Number);
            }
            foreach (Card card in dealer.Cards)
            {
                Dface.Append(card.Face);
                Dcardnum.Append(card.Number);
            }
            //First if statement checks for straight, the second one is a straight flush
            if ((cardnum[1] + cardnum[2] + cardnum[3]) / 3 == cardnum[2])
            {
                hand = 3;
                Straight = true;
                if(face[1] == face[2] && face[1] == face[3])
                {
                    hand = 5;
                    StraightFlush = true;
                }
            }
            //This if statement checks if we have pair and then a 3 of a kind
            if(cardnum[1] == cardnum[2] || cardnum[1] == cardnum[3] || cardnum[2] == cardnum[3])
            {
                hand = 1;
                Pair = true;
                if(cardnum[1] == cardnum[2] && cardnum[1] == cardnum[3])
                {
                    hand = 4;
                    ThreeOfKind = true;
                }
                            }
            //this statement checks if we have a flush
            if(Equals(face[1], face[2]) && Equals(face[1], face[3]))
            {
                hand = 2;
                Flush = true;
            }
            //if we don't have one of the previous hands then it will default to high card
            if(Pair == false && Flush == false && Straight == false && ThreeOfKind == false && StraightFlush == false)
            {
                hand = 0;
                HighCard = true;
            }
            //First if statement checks for straight, the second one is a straight flush FOR THE DEALER
            if ((Dcardnum[1] + Dcardnum[2] + Dcardnum[3]) / 3 == Dcardnum[2])
            {
                dealer_hand = 3;
                DStraight = true;
                if (Dface[1] == Dface[2] && Dface[1] == Dface[3])
                {
                    dealer_hand = 5;
                    DStraightFlush = true;
                }
            }
            //This if statement checks if we have pair and then a 3 of a kind
            if (Dcardnum[1] == Dcardnum[2] || Dcardnum[1] == Dcardnum[3] || Dcardnum[2] == Dcardnum[3])
            {
                dealer_hand = 1;
                DPair = true;
                if (Dcardnum[1] == Dcardnum[2] && Dcardnum[1] == Dcardnum[3])
                {
                    dealer_hand = 4;
                    DThreeOfKind = true;
                }
            }
            //this statement checks if we have a flush
            if (Equals(face[1], face[2]) && Equals(face[1], face[3]))
            {
                dealer_hand = 2;
                DFlush = true;
            }
            //if we don't have one of the previous hands then it will default to high card
            if (DPair == false && DFlush == false && DStraight == false && DThreeOfKind == false && DStraightFlush == false)
            {
                dealer_hand = 0;
                DHighCard = true;
            }
            //Assigns the winner
            if(dealer_hand < hand)
            {
                player.Outcome = "You WON, you had the better hand!";
            }
            if (dealer_hand > hand)
            {
                player.Outcome = "You lost, dealer had the better hand!";
            }
            _context.SaveChanges();
            return NoContent();
        }
        
    }
}
