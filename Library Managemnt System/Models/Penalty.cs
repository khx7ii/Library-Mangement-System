using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Managemnt_System.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        [ForeignKey("Checkout")]
        public int CheckoutId { get; set; }
        public double Amount { get; set; }
        public int OverDueDays { get; set; }

        public Checkout Checkout { get; set; }
    }
}
