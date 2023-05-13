using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Account.ViewModels
{
    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {



        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}