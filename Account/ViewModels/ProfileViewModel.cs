using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Account.ViewModels
{
    public class ProfileViewModel
    {


        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
        [Required]
        public string JobTitle { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote("CheckEmailIsUsed", "MemberEdit", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MemberType { get; set; }



        public int Sector { get; set; }

        [Url(ErrorMessage = "This is not a valid URL")]
        public string LinkedIn { get; set; }
        public string Organisation { get; set; }

        [Url(ErrorMessage = "This is not a valid URL")]
        public string CompanyUrl { get; set; }


    }
}