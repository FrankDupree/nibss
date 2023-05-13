using System.ComponentModel.DataAnnotations;

namespace Account.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [MinLength(2, ErrorMessage = "First Name should be at least 2 characters")]
        [MaxLength(20, ErrorMessage = "First Name should not be more than 20 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [MinLength(2, ErrorMessage = "Surname should be at least 2 characters")]
        [MaxLength(20, ErrorMessage = "Surname should not be more than 20 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please specify your phone number")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Enter a valid phone number")]
        [MinLength(8, ErrorMessage = "Phone number should be at least 8 characters")]
        [MaxLength(14, ErrorMessage = "Invalid length for phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your company name")]
        public string Coy { get; set; }

        [Required(ErrorMessage = "Please enter the subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter your comment")]
        [MaxLength(500, ErrorMessage = "Your message must be no longer than 500 characters")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "You must enter a valid email address")]
        public string Email { get; set; }
    }
}