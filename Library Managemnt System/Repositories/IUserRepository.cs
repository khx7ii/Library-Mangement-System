using Library_Managemnt_System.Models;

namespace Library_Managemnt_System.Repositories
{
    public interface IUserRepository
    {
        public  Task <List<Checkout>>GetCheckouts(string id);
    }
}
