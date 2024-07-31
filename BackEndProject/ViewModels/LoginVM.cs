using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels
{
    public class LoginVM
    {
        [Required]
        [Display(Name ="Username or Email")]
        public string UserNameOrEmail { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]

        public bool RememberMe { get; set; }
    }
}
