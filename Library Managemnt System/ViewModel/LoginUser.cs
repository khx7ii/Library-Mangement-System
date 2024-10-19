using System.ComponentModel.DataAnnotations;

namespace Library_Managemnt_System.ViewModel
{
	public class LoginUser
	{
		[Required(ErrorMessage = "The Username field is required.")]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]		
		public bool RememberMe { get; set; }
	}
}
