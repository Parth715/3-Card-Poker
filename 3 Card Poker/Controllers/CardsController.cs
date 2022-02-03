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
        [HttpPut("reset/{deck}")]//resets games to default values
        public async Task<ActionResult> Reset(Player deck)
        {
            for (var i = 1; i <= 55; i++)
            {
                var blank = await _context.Cards.FindAsync(i);
                blank.PlayerId = deck.Id;
                await _context.SaveChangesAsync();
            }
            var player = await _context.Players.FindAsync(1);
            player.Outcome = "";
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Start")]//assigns player card, blank cards to dealer
        public async Task<ActionResult> StartGame()
        {
            for(var i = 53; i <=55; i++)
            {
                var blank = await _context.Cards.FindAsync(i);
                blank.PlayerId = 3;
                await _context.SaveChangesAsync();
            }
            

            Random random = new Random();
            for(var i = 1; i <=3; i+=0)//assigns 3 different cards to player
            {
                var card = random.Next(1, 53);
                var play = await _context.Cards.FindAsync(card);
                if (play.PlayerId != 1)
                {
                    play.PlayerId = 1;
                    await _context.SaveChangesAsync();
                    i++;
                }
            }
            return NoContent();
        }

        [HttpPut("check/{dealer}")] //dealer gets 3 cards
        public async Task<ActionResult<Player>> Check(Player dealer)
        {
            var player = await _context.Players.Include(x => x.Cards).SingleOrDefaultAsync(x => x.Id == 1);
            var deck = await _context.Players.FindAsync(2);
            Random random = new Random();
            foreach(var card in dealer.Cards)
            {
                card.PlayerId = deck.Id;
                await _context.SaveChangesAsync();
            }
            dealer = await _context.Players.FindAsync(3);
            for (var i = 1; i <= 3; i += 0) //assigns dealer 3 different cards
            {
                var card = random.Next(1, 53);
                var play = await _context.Cards.FindAsync(card);
                if (play.PlayerId != 1 && play.PlayerId != dealer.Id)
                {
                    play.PlayerId = dealer.Id;
                    await _context.SaveChangesAsync();
                    i += 1;
                }
            }
            dealer = await _context.Players.FindAsync(dealer.Id);
            int HighCard = 0;
            bool Pair = false;
            bool Flush = false;
            bool Straight = false;
            bool ThreeOfKind = false;
            bool StraightFlush = false;
            int DHighCard = 0;
            bool DPair = false;
            bool DFlush = false;
            bool DStraight = false;
            bool DThreeOfKind = false;
            bool DStraightFlush = false;
            var outcome = "";
            var Doutcome = "";
            var hand = 0;
            var dealer_hand = 0;
            List<string> face = new List<string>();
            List<int> cardnum = new List<int>();
            List<string> Dface = new List<string>();
            List<int> Dcardnum = new List<int>();
            foreach (Card card in player.Cards)
            {
                face.Add(card.Face);
                cardnum.Add(card.Number);
            }
            foreach (Card card in dealer.Cards)
            {
                Dface.Add(card.Face);
                Dcardnum.Add(card.Number);
            }
            cardnum.Sort();
            //First if statement checks for straight, the second one is a straight flush
            if (cardnum[1]-cardnum[0] == 1 && cardnum[2]-cardnum[1]==1)
            {
                hand = 3;
                Straight = true;
                outcome = "a straight";
                if (face[0] == face[1] && face[0] == face[2])
                {
                    hand = 5;
                    StraightFlush = true;
                    outcome = "a straight flush";
                }
            }
            //This if statement checks if we have pair and then a 3 of a kind
            if (cardnum[0] == cardnum[1] || cardnum[0] == cardnum[2] || cardnum[1] == cardnum[2])
            {
                hand = 1;
                Pair = true;
                outcome = "a pair";
                if (cardnum[0] == cardnum[1] && cardnum[0] == cardnum[2])
                {
                    hand = 4;
                    ThreeOfKind = true;
                    outcome = "3 of a kind";
                }
            }
            //this statement checks if we have a flush
            if (Equals(face[0], face[1]) && Equals(face[0], face[2]))
            {
                hand = 2;
                Flush = true;
                outcome = "a flush";
            }
            //if we don't have one of the previous hands then it will default to high card
            if (Pair == false && Flush == false && Straight == false && ThreeOfKind == false && StraightFlush == false)
            {
                hand = 0;
                HighCard = cardnum[2];
                outcome = " better high card";
            }
            //First if statement checks for straight, the second one is a straight flush FOR THE DEALER
            Dcardnum.Sort();
            if (Dcardnum[1] - Dcardnum[0] == 1 && Dcardnum[2] - Dcardnum[1] == 1)
            {
                dealer_hand = 3;
                DStraight = true;
                Doutcome = "a straight";
                if (Dface[0] == Dface[1] && Dface[0] == Dface[2])
                {
                    dealer_hand = 5;
                    DStraightFlush = true;
                    Doutcome = "a straight flush";
                }
            }
            //This if statement checks if we have pair and then a 3 of a kind
            if (Dcardnum[0] == Dcardnum[1] || Dcardnum[0] == Dcardnum[2] || Dcardnum[1] == Dcardnum[2])
            {
                dealer_hand = 1;
                DPair = true;
                Doutcome = "a pair";
                if (Dcardnum[0] == Dcardnum[1] && Dcardnum[0] == Dcardnum[2])
                {
                    dealer_hand = 4;
                    DThreeOfKind = true;
                    Doutcome = "3 of a kind";
                }
            }
            //this statement checks if we have a flush
            if (Equals(Dface[0], Dface[1]) && Equals(Dface[0], Dface[2]))
            {
                dealer_hand = 2;
                DFlush = true;
                Doutcome = "a flush";
            }
            //if we don't have one of the previous hands then it will default to high card
            if (DPair == false && DFlush == false && DStraight == false && DThreeOfKind == false && DStraightFlush == false)
            {
                dealer_hand = 0;
                DHighCard = Dcardnum[2];
                Doutcome = "a better high card";
            }
            //Assigns the winner
            if (dealer_hand == hand)
            {
                if (Dcardnum[2] < cardnum[2])
                {

                    player.Outcome = $"You WON, you had {outcome}";
                    await _context.SaveChangesAsync();
                }
                if (Dcardnum[2] > cardnum[2])
                {
                    player.Outcome = $"You lost, dealer had {Doutcome}";
                    await _context.SaveChangesAsync();
                }
                if (Dcardnum[1] < cardnum[1]){
                    
                    player.Outcome = $"You WON, you had {outcome}";
                    await _context.SaveChangesAsync();
                }
                if (Dcardnum[1] > cardnum[1]){
                    player.Outcome = $"You lost, dealer had {Doutcome}";
                    await _context.SaveChangesAsync();
                }
                if (DHighCard < HighCard)
                {
                    player.Outcome = $"You WON, you had {outcome}";
                    await _context.SaveChangesAsync();
                }
                if(DHighCard > HighCard)
                {
                    player.Outcome = $"You lost, dealer had {Doutcome}";
                    await _context.SaveChangesAsync();
                }
            }
            if (dealer_hand < hand)
            {
                player.Outcome = $"You WON, you had {outcome}";
                await _context.SaveChangesAsync();
            }
            if (dealer_hand > hand)
            {
                player.Outcome = $"You lost, dealer had {Doutcome}";
                await _context.SaveChangesAsync();
            }
            if(Pair = true && DPair == true) 
            {
                if (cardnum[1] < Dcardnum[1])
                {
                    player.Outcome = $"You lost, dealer had {Doutcome}";
                }
                if(cardnum[1] > Dcardnum[1])
                {
                    player.Outcome = $"You WON, you had {outcome}";
                }
            }
            
            return await _context.Players.FindAsync(dealer.Id);

        }

    }
}
