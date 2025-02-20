using BookAPI.Models;

namespace BookAPI.Data.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(int page, int pageSize);
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> GetBookByTitleAsync(string title);
        Task<bool> BookExistsAsync(string title);
        Task<Book> AddBookAsync(Book book);
        Task<IEnumerable<Book>> AddBooksAsync(IEnumerable<Book> books);
        Task<Book> UpdateBookAsync(Book book);
        Task<Book> SoftDeleteBookAsync(int id);
        Task<bool> SoftDeleteBooksAsync(IEnumerable<int> ids);
        Task<int> SaveChangesAsync();
    }
}
