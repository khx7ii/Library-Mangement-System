using Library_Managemnt_System.Models;

namespace Library_Managemnt_System.Repositories
{
    public interface ICommentRepository
    {
        public void Add(Comment comment);
        public void Update(Comment comment);
        public void Delete(Comment comment);
        public List<Comment> GetComments(string id);
        public List<Comment> GetCommentsByBook(int id);
        public void Save();
    }
}
