using Microsoft.AspNetCore.Identity;

namespace Library_Managemnt_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        //public int Id { get; set; }
     //   public string Name { get; set; }
      //  public string Email { get; set; }
      //  public string Password { get; set; }
       
        public List<Checkout> Checkouts { get; set; }
    }
}
