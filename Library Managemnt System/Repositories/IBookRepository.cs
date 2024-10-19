using Library_Managemnt_System.Models;
using Library_Managemnt_System.ViewModel;

namespace Library_Managemnt_System.Repositories
{
    public interface IBookRepository
    {
        public void Add(Book book);
        public void Update(Book book);
        public void Delete(Book book);
        public List<Book> GetAll();

        public Book GetById(int id);
        public Task< Book> GetByIdWithCategory(int id);
        public void Save();
        public void SaveAsync();
        public PaginatedListViewModel<Book> GetPaginated(int pageNumber);

    }
}
