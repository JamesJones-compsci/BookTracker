using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace BookTracker.Controllers
{
    [Authorize]
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context) => _context = context;

        // GET /books
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var books = await _context.Books
                .Where(b => b.UserId == userId)
                .Include(b => b.User)
                .ToListAsync();

            return View(books);
        }

        // GET /books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.User)
                .Include(b => b.Reviews) // eager-load reviews if you want them visible
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            return book == null ? NotFound() : View(book);
        }

        // GET /books/create
        [HttpGet("create")]
        public IActionResult Create() => View();

        // POST /books/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Genre,Year")] Book book)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ModelState.AddModelError("", "You must be logged in to add a book.");
                return View(book);
            }

            book.UserId = userId;
            _context.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /books/{id}/edit
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book == null ? NotFound() : View(book);
        }

        // POST /books/{id}/edit
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,Year")] Book book)
        {
            if (id != book.Id) return NotFound();
            if (!ModelState.IsValid) return View(book);

            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBook == null) return NotFound();

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.Year = book.Year;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET /books/{id}/delete
        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            return book == null ? NotFound() : View(book);
        }

        // POST /books/{id}/delete
        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null) _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
