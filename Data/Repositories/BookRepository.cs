using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(int page, int pageSize)
        {
            return await _context.Books
                .OrderByDescending(b => b.ViewsCount)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                book.ViewsCount++;
                await _context.SaveChangesAsync();
            }

            return book;
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> BookExistsAsync(string title)
        {
            return await _context.Books
                .AnyAsync(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<IEnumerable<Book>> AddBooksAsync(IEnumerable<Book> books)
        {
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();
            return books;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> SoftDeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                book.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return book;
        }

        public async Task<bool> SoftDeleteBooksAsync(IEnumerable<int> ids)
        {
            var books = await _context.Books
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();

            foreach (var book in books)
            {
                book.IsDeleted = true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
