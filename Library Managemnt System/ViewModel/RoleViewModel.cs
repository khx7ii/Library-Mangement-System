using System.ComponentModel.DataAnnotations;

namespace Library_Managemnt_System.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
