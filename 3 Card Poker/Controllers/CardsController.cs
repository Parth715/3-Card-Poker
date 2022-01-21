﻿#nullable disable
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
            foreach(Card card in player.Cards)
            {
                face.Append(card.Face);
                cardnum.Append(card.Number);
            }
            foreach (Card card in dealer.Cards)
            {
                Dface.Append(card.Face);
                Dcardnum.Append(card.Number);
            }
            return NoContent();
        }
        bool HighCard = false;
        bool Pair = false;
        bool Flush = false;
        bool Straight = false;
        bool ThreeOfKind = false;
        bool StraightFlush = false;
        bool RoyalFlush = false;
    }
}
