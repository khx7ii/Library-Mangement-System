using Library_Managemnt_System.Models;
using Library_Managemnt_System.ViewModel;

namespace Library_Managemnt_System.Repositories
{
    public interface ICategoryRepository
    {
        public void Add(Category category);
        public void Update(Category category);
        public void Delete(Category category);
        public List<Category> GetAll();

        public Category GetById(int id);

        public void Save();
        public PaginatedListViewModel<Category> GetPaginated(int pageNumber);
    }
}
