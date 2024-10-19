using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Managemnt_System.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]

        public string UserId { get; set; }

        public string userName { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public string Describtion { get; set; }

        public DateTime date { get; set; }

        public Book Book { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
