using Library_Managemnt_System.Models;
using Library_Managemnt_System.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Library_Managemnt_System.Repositories
{
    public class BookRepository : IBookRepository
    {
        LibraryContext context;
        public BookRepository(LibraryContext libraryContext) { context = libraryContext; }
        public void Add(Book book)
        {
            context.Add(book);
        }

        public void Delete(Book book)
        {
            context.Remove(book);
        }

        public List<Book> GetAll()
        {
            return context.Book.Include(c=>c.Category).ToList();
        }

        public Book GetById(int id)
        {
            return context.Book.Include(c => c.Category).FirstOrDefault(d => d.Id == id);
        }
        public async Task<Book> GetByIdWithCategory(int id)
        {
            return await context.Book.Include(c=>c.Category).FirstOrDefaultAsync(d => d.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }

		public async void SaveAsync()
		{
			await context.SaveChangesAsync();
		}

		public void Update(Book book)
        {
            context.Update(book);
        }
        public PaginatedListViewModel<Book> GetPaginated(int pageNumber)
        {
            int pageSize = 2;
            var data = context.Book
                              .OrderBy(c => c.Name)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            int totalItems = context.Book.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PaginatedListViewModel<Book>
            {
                Items = data,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }
    }
}
