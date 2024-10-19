using Library_Managemnt_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Managemnt_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        LibraryContext context;
        public UserRepository(LibraryContext libraryContext) { context = libraryContext; }
        public async Task<List<Checkout>> GetCheckouts(string id)
        {
            return context.Checkout.Include(c=>c.Book).Where(c=>c.UserId == id).ToList();
        }
    }
}
