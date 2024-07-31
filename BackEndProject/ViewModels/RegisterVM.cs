using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels
{
    public class RegisterVM
    {
        [Required,MaxLength(30)]
        public string UserName { get; set; }

        [Required, MaxLength(30)]
        public string FullName { get; set; }

        [Required, EmailAddress,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
