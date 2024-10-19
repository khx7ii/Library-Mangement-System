using Library_Managemnt_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Managemnt_System.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        LibraryContext context;
        public CommentRepository(LibraryContext libraryContext) { context = libraryContext; }
        public void Add(Comment comment)
        {
            context.Add(comment);
        }

        public void Delete(Comment comment)
        {
            context.Remove(comment);
        }

        public List<Comment> GetComments(string id)
        {
            return context.Comment.Where(c=>c.UserId==id).ToList();
        }
        public List<Comment> GetCommentsByBook(int id)
        {
            return context.Comment.Where(c=>c.BookId==id).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            context.Update(comment);
        }
    }
}
