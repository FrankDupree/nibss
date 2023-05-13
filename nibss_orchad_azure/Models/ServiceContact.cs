using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace nibss_orchad_azure.Models
{
    public class ServiceContact
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [MinLength(2, ErrorMessage = "First Name should be at least 2 characters")]
        [MaxLength(20, ErrorMessage = "First Name should not be more than 20 characters")]
        public string FirstName {get;set;}

        [Required(ErrorMessage = "Please enter your last name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [MinLength(2, ErrorMessage = "Last Name should be at least 2 characters")]
        [MaxLength(20, ErrorMessage = "Last Name should not be more than 20 characters")]
        public string LastName { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please specify your phone number")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Enter a valid phone number")]
        [MinLength(8, ErrorMessage = "Phone number should be at least 8 characters")]
        [MaxLength(14, ErrorMessage = "Invalid length for phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Idea { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Problems { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Uniqueness { get; set; }
    }
}
