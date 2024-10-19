using System.ComponentModel.DataAnnotations;

namespace Library_Managemnt_System.ViewModel
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } 


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        public string PhoneNumber { get; set; }

        
    }
}
