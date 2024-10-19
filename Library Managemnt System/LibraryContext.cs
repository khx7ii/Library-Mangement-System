using Library_Managemnt_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_Managemnt_System
{
    public class LibraryContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book>Book { get; set; }
        public DbSet<Category>Category { get; set; }
        public DbSet<Checkout> Checkout { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Penalty> Penalty { get; set; }

        public LibraryContext():base() { }

        public LibraryContext(DbContextOptions options) : base(options) { }

    }
}
