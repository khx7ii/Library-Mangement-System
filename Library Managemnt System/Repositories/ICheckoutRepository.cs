using Library_Managemnt_System.Models;

namespace Library_Managemnt_System.Repositories
{
	public interface ICheckoutRepository
	{
		public void Add(Checkout checkout);
		public void Delete(Checkout checkout);
		public List<Checkout> GetByBookId(int bookId);

		public Checkout GetById(int id);
		public List<Checkout> GetAll();

		public void Save();
	}
}
