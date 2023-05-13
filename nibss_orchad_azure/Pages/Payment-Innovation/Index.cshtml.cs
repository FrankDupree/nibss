using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nibss_orchad_azure.Models;
using nibss_orchad_azure.Services;
using OrchardCore.Email;

namespace nibss_orchad_azure.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly CaptchaVerificationService _verificationService;
        public string CaptchaClientKey { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }
        public IndexModel(IConfiguration configuration, CaptchaVerificationService verificationService)
        {
            _configuration = configuration;
            _verificationService = verificationService;
            CaptchaClientKey = _configuration["Captcha:ClientKey"];
        }

        public IActionResult OnGet()
        {
            ViewData["confirmation"] = "init";
            return Page();
        }
      

        [BindProperty]
        public ServiceContact ServiceContact { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // validate input
            var requestIsValid = await _verificationService.IsCaptchaValid(CaptchaResponse);

            bool success = false;
            string marketingEmail = _configuration.GetValue<string>("MarkEmail");
            var _smtpService = this.HttpContext.RequestServices.GetRequiredService<ISmtpService>();
            if (ModelState.IsValid && requestIsValid)
            {
                var message = new MailMessage()
                {
                    To = marketingEmail,
                    Subject = "Customer Enquiry: Payment-Innovation",
                    Body = @$"
                        Name:    {ServiceContact.FirstName} {ServiceContact.LastName }
                        Description : {ServiceContact.Idea}
                        Problems:   {ServiceContact.Problems}
                        uniqueness : {ServiceContact.Uniqueness} 
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