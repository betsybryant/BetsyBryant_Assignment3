using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BetsyBryant_Assignment3.Data;
using BetsyBryant_Assignment3.Models;
using Tweetinvi;
using VaderSharp2;

namespace BetsyBryant_Assignment3.Views
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Actor.Include(a => a.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }
            ActorDetailsVM actorDetailsVM = new ActorDetailsVM();
            actorDetailsVM.actor = actor;
            actorDetailsVM.Tweets = new List<ActorTweet>();

            var userClient = new TwitterClient("LxqCHKijPvXLRHmPvFi5f1i3e", "wOUfFpRibUPCSQHHeWHeLKEakYv8F7hkCYUSg6WCd7CvOYukce", "1577350777831555096-IiybvCz67BkU0pJHmt6Igawk89MQ7b", "PM201ITpWffaMcravnnBAQEEre5ojXhZ0rvGAHeBBfXb1");
            var searchResponse = await userClient.SearchV2.SearchTweetsAsync(actor.ActorName);
            var tweets = searchResponse.Tweets;
            var analyzer = new SentimentIntensityAnalyzer();
            foreach (var tweetText in tweets)
            {
                var tweet = new ActorTweet();
                tweet.TweetText = tweetText.Text;
                var results = analyzer.PolarityScores(tweet.TweetText);
                tweet.Sentiment = results.Compound;
                actorDetailsVM.Tweets.Add(tweet);
            }

            return View(actorDetailsVM);

        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "MovieTitle");
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName,Gender,Age,ActorIMDBLink,MovieID")] Actor actor, IFormFile ActorPhoto)
        {
            if (ModelState.IsValid)
            {
                if(ActorPhoto != null && ActorPhoto.Length > 0)
                {
                    var memoryStream = new MemoryStream();
                    await ActorPhoto.CopyToAsync(memoryStream);
                    actor.ActorPhoto = memoryStream.ToArray();
                }
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "MovieTitle", actor.MovieID);
            return View(actor);
        }
        public async Task<IActionResult> GetActorPhoto(int ActorId)
        {
            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.ActorId == ActorId);
            if (actor == null)
            {
                return NotFound();
            }
            var imageData = actor.ActorPhoto;

            return File(imageData, "image/jpg");
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "MovieTitle", actor.MovieID);
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName,Gender,Age,ActorIMDBLink,ActorPhoto,MovieID")] Actor actor)
        {
            if (id != actor.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "MovieId", "MovieTitle", actor.MovieID);
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Actor'  is null.");
            }
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
          return _context.Actor.Any(e => e.ActorId == id);
        }
    }
}
