using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Account.ViewModels
{
    public class RegistrationViewModel
    {

        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [MinLength(2, ErrorMessage = "First should be at least 2 characters")]
        [MaxLength(30, ErrorMessage = "Name should not be more than 30 characters")]
        public string Name { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsed", "Account", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [UIHint("Sector")]
        [Required(ErrorMessage = "Please enter your Sector")]
        public string Sector { get; set; }

        [UIHint("Phone")]
        [Required(ErrorMessage = "Please specify your phone number")]
        [Phone]
        [MinLength(11, ErrorMessage = "Invalid Phone Number")]
        [MaxLength(11, ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [UIHint("Organisation")]
        [Required(ErrorMessage = "Please enter your Organisation name")]
        public string Organisation { get; set; }

        [UIHint("Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please Retype your password")]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }

        [DisplayName("I accept the Terms and Conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You have to accept our terms and conditions to proceed")]
        public bool TermsAndConditions { get; set; }
    }
}
