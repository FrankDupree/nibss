using Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nibss_orchad_azure.Services;
using OrchardCore.Email;
using System.Threading.Tasks;

namespace nibss_orchad_azure.Pages.Contact
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly CaptchaVerificationService _verificationService;
        public string CaptchaClientKey { get; set; } 
        [BindProperty(Name = "g-Recaptcha-Response")]
        public string CaptchaResponse { get; set; }
        public IndexModel(IConfiguration configuration, CaptchaVerificationService verificationService)
        {
            _configuration = configuration;
            _verificationService = verificationService;
            CaptchaClientKey = _configuration["Captcha:ClientKey"];
        }
        public void OnGet()
        {
            ViewData["confirmation"] = "init";
        }

        [BindProperty]
        public ContactViewModel ContactViewModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
       
            // validate input
            var requestIsValid = await _verificationService.IsCaptchaValid(CaptchaResponse);

            bool success = false;
            string MarkEmail = _configuration.GetValue<string>("MarkEmail");
            var _smtpService = this.HttpContext.RequestServices.GetRequiredService<ISmtpService>();
            if (ModelState.IsValid && requestIsValid)
            {
                var message = new MailMessage()
                {
                    To = MarkEmail,
                    Subject = "Customer Enquiry: Contact Us",
                    Body = @$"
                        Name:    {ContactViewModel.FirstName} {ContactViewModel.Surname }
                        Email:   {ContactViewModel.Email}
                        Company: {ContactViewModel.Coy}
                        Phone:   {ContactViewModel.Phone}
                        Message: {ContactViewModel.Comment} 
                    ",
                    IsBodyHtml = true
                };
                var result = await _smtpService.SendAsync(message);
                success = result.Succeeded;
            }

            if (success)
            {
                ViewData["confirmation"] = "true";
            }
            else
            {
               
               ViewData["confirmation"] = "false";
            }
            return Page();
        }
    }
}
