using Library_Managemnt_System.Models;
using Library_Managemnt_System.Repositories;
using Library_Managemnt_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library_Managemnt_System.Controllers
{
    public class CommentController : Controller
    {
        ICommentRepository commentRepository;
        IBookRepository bookRepository;
        public CommentController(ICommentRepository commentRepository, IBookRepository bookRepository)
        {
            this.commentRepository = commentRepository;
            this.bookRepository = bookRepository;
        }
       
        [HttpPost]
        public IActionResult SaveComment(BookViewModel book)
        {
            if (User.Identity.IsAuthenticated)
            {
                Comment commentdb = new Comment();
                Claim UserClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userid = UserClaim.Value;               
                commentdb.Describtion = book.Comment;
                commentdb.BookId = book.Id;
                commentdb.UserId = userid;
                commentdb.userName = User.Identity.Name;
                commentdb.date=DateTime.Now;
                commentRepository.Add(commentdb);
                commentRepository.Save();
                ViewBag.CreateDate = DateTime.Now;
            }
            
            return RedirectToAction("GetComments");
        }
        public IActionResult GetComments() 
        {
            Claim UserClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = UserClaim.Value;
            var comments = commentRepository.GetComments(userid);
            Dictionary<int, string> books = new Dictionary<int, string>();
            foreach (var comment in comments)
            {
                books[comment.BookId] = bookRepository.GetById(comment.BookId).Name;
            }
            ViewBag.Books = books;
            return View("Comment",comments);
        }
        
    }
}
