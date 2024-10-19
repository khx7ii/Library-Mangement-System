using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Managemnt_System.Models
{
    public class Checkout
    {
        public int Id { get; set; }
       
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public int BookId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double Penalty { get; set; }
        public bool ActiveFlag { get; set; }

        public Book Book { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
