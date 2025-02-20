using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookAPI.Data.Repositories;
using BookAPI.Dtos;
using BookAPI.Models;
namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var books = await _bookRepository.GetAllBooksAsync(page, pageSize);

            return Ok(books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailDto>> GetBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
                return NotFound();

            int yearsSincePublished = DateTime.Now.Year - book.PublicationYear;

            return Ok(new BookDetailDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.AuthorName,
                PublicationYear = book.PublicationYear,
                ViewsCount = book.ViewsCount,
                PopularityScore = book.CalculatePopularityScore(),
                YearsSincePublished = yearsSincePublished
            });
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook(BookCreateDto bookDto)
        {
            if (await _bookRepository.BookExistsAsync(bookDto.Title))
                return BadRequest("A book with the same title already exists");

            var book = new Book
            {
                Title = bookDto.Title,
                AuthorName = bookDto.AuthorName,
                PublicationYear = bookDto.PublicationYear
            };

            var createdBook = await _bookRepository.AddBookAsync(book);

            return CreatedAtAction(
                nameof(GetBook),
                new { id = createdBook.Id },
                new BookDto { Id = createdBook.Id, Title = createdBook.Title }
            );
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<BookDto>>> AddBooks(BulkBooksDto booksDto)
        {
            if (booksDto.Books == null || !booksDto.Books.Any())
                return BadRequest("No books provided");

            var existingTitles = new List<string>();

            foreach (var bookDto in booksDto.Books)
            {
                if (await _bookRepository.BookExistsAsync(bookDto.Title))
                    existingTitles.Add(bookDto.Title);
            }

            if (existingTitles.Any())
                return BadRequest($"Books with the following titles already exist: {string.Join(", ", existingTitles)}");

            var books = booksDto.Books.Select(b => new Book
            {
                Title = b.Title,
                AuthorName = b.AuthorName,
                PublicationYear = b.PublicationYear
            });

            var createdBooks = await _bookRepository.AddBooksAsync(books);

            return Ok(createdBooks.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title
            }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookUpdateDto bookDto)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
                return NotFound();

            if (!string.IsNullOrEmpty(bookDto.Title) && bookDto.Title != book.Title)
            {
                if (await _bookRepository.BookExistsAsync(bookDto.Title))
                    return BadRequest("A book with the same title already exists");

                book.Title = bookDto.Title;
            }

            if (!string.IsNullOrEmpty(bookDto.AuthorName))
                book.AuthorName = bookDto.AuthorName;

            if (bookDto.PublicationYear.HasValue)
                book.PublicationYear = bookDto.PublicationYear.Value;

            await _bookRepository.UpdateBookAsync(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.SoftDeleteBookAsync(id);

            if (book == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBooks(BulkDeleteDto deleteDto)
        {
            if (deleteDto.BookIds == null || !deleteDto.BookIds.Any())
                return BadRequest("No book IDs provided");

            var result = await _bookRepository.SoftDeleteBooksAsync(deleteDto.BookIds);

            if (!result)
                return NotFound("No books were found to delete");

            return NoContent();
        }
    }
}
