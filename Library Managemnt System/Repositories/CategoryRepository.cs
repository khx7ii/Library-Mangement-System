using Library_Managemnt_System.Models;
using Library_Managemnt_System.ViewModel;

namespace Library_Managemnt_System.Repositories
{
    public class CategoryRepository:ICategoryRepository
    {
        LibraryContext context;
        public CategoryRepository(LibraryContext libraryContext) { context = libraryContext; }
        public void Add(Category category)
        {
            context.Add(category);
        }

        public void Delete(Category category)
        {
            context.Remove(category);
        }

        public List<Category> GetAll()
        {
            return context.Category.ToList();
        }

        public Category GetById(int id)
        {
            return context.Category.FirstOrDefault(d => d.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Category category)
        {
            context.Update(category);
        }
        public PaginatedListViewModel<Category> GetPaginated(int pageNumber)
        {
            int pageSize = 2;
            var data = context.Category
                              .OrderBy(c => c.Name)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            int totalItems = context.Category.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PaginatedListViewModel<Category>
            {
                Items = data,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }
    }
}
