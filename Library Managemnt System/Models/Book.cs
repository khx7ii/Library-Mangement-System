using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Managemnt_System.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [RegularExpression(@"\w+\.(jpg|png)", ErrorMessage = "Image must Be jpg or png")]
        public string Image { get; set; }
        public string Describtion { get; set; }
        public List<Comment>? Comments { get; set; }
        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

    }
}
