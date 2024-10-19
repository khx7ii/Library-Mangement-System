using Library_Managemnt_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Managemnt_System.Repositories
{
	public class CheckoutRepository : ICheckoutRepository
	{
		LibraryContext context;
		public CheckoutRepository(LibraryContext libraryContext) { context = libraryContext; }
		public void Add(Checkout checkout)
		{
			context.Add(checkout);
		}

		public void Delete(Checkout checkout)
		{
			context.Remove(checkout);
		}

		public List<Checkout> GetAll()
		{
			return context.Checkout.Include(c=>c.Book).Include(c=>c.ApplicationUser).ToList();
		}

        public List<Checkout> GetByBookId(int bookId)
        {
            return context.Checkout.Where(c=>c.BookId == bookId).ToList();
        }

        public Checkout GetById(int id)
        {
            return context.Checkout.FirstOrDefault(c=>c.Id==id);
        }

        public void Save()
		{
			context.SaveChanges();
		}
	}
}
