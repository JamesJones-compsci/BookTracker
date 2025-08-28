using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookTracker.Controllers
{
    [Authorize]
    [Route("books/{bookId}/reviews")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context) => _context = context;

        // GET /books/{bookId}/reviews
        [HttpGet("")]
        public async Task<IActionResult> Index(int bookId)
        {
            var reviews = _context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.User)
                .Include(r => r.Book);

            ViewBag.BookId = bookId;
            ViewBag.BookTitle = await _context.Books
                .Where(b => b.Id == bookId)
                .Select(b => b.Title)
                .FirstOrDefaultAsync();

            return View(await reviews.ToListAsync());
        }

        // GET /books/{bookId}/reviews/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int bookId, int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);

            return review == null ? NotFound() : View(review);
        }

        // GET /books/{bookId}/reviews/create
        [HttpGet("create")]
        public IActionResult Create(int bookId)
        {
            return View(new Review { BookId = bookId });
        }

        // POST /books/{bookId}/reviews/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, [Bind("Rating,Comment,BookId")] Review review)
        {
            if (!ModelState.IsValid) return View(review);

            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            review.CreatedAt = DateTime.Now;

            _context.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { bookId = review.BookId });
        }

        // GET /books/{bookId}/reviews/{id}/edit
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int bookId, int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            return review == null ? NotFound() : View(review);
        }

        // POST /books/{bookId}/reviews/{id}/edit
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int bookId, int id, [Bind("Id,Rating,Comment,BookId")] Review review)
        {
            if (id != review.Id) return NotFound();
            if (!ModelState.IsValid) return View(review);

            var existingReview = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (existingReview == null) return NotFound();

            review.UserId = existingReview.UserId;
            review.CreatedAt = existingReview.CreatedAt;

            _context.Update(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { bookId = review.BookId });
        }

        // GET /books/{bookId}/reviews/{id}/delete
        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(int bookId, int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);

            return review == null ? NotFound() : View(review);
        }

        // POST /books/{bookId}/reviews/{id}/delete
        [HttpPost("{id}/delete"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookId, int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { bookId });
        }

        private bool ReviewExists(int id) => _context.Reviews.Any(r => r.Id == id);
    }
}
