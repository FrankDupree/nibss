using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using nibss_orchad_azure.Models;
using nibss_orchad_azure.Services;
using OrchardCore;
using OrchardCore.Email;
using System.Threading.Tasks;

namespace nibss_orchad_azure.Pages.Trainings
{
    public class EnrollModel : PageModel
    {
        private readonly IOrchardHelper _orchard;
        private readonly IConfiguration _configuration;
        private readonly CaptchaVerificationService _verificationService;
        public string CaptchaClientKey { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }

        public EnrollModel(IOrchardHelper orchard, IConfiguration configuration, CaptchaVerificationService verificationService)
        {
            _orchard = orchard;
            _configuration = configuration;
            _verificationService = verificationService;
            CaptchaClientKey = _configuration["Captcha:ClientKey"];
        }

        public string Slug { get; set; }
        public void OnGet(string slug)
        {
            ViewData["confirmation"] = "init";
            Slug = slug;
        }

        [BindProperty]
        public Enroll Enroll { get; set; }

        [BindProperty]
        public string Training { get; set; }
        public async Task<IActionResult> OnPostAsync(string tid)
        {
            // validate input
            var requestIsValid = await _verificationService.IsCaptchaValid(CaptchaResponse);

            bool success = false;
            string MarkEmail = _configuration.GetValue<string>("MarkEmail");
            var training = await _orchard.GetContentItemByIdAsync(tid);
            var _smtpService = this.HttpContext.RequestServices.GetRequiredService<ISmtpService>();
            if (training != null && ModelState.IsValid && requestIsValid)
            {
            
                    var message = new MailMessage()
                    {
                        To = MarkEmail,
                        Subject = "Custome Enquiry: Training",
                        Body = @$"
                        Name:    {Enroll.FirstName} {Enroll.LastName }
                        Email:   {Enroll.Email}
                        Phone:   {Enroll.Phone}
                        Training Id: {tid} 
                        Training Name: {training.DisplayText}
                    ",
                        IsBodyHtml = true
                    };
                    var result = await _smtpService.SendAsync(message);
                    success = result.Succeeded;

                    if (success)
                    {
                        ViewData["confirmation"] = "true";
                    }
                    else
                    {
                        ViewData["confirmation"] = "false";
                    }

                TempData["data"] = JsonConvert.SerializeObject(training);
                return Page();

            }


            return new RedirectToPageResult("/trainings");
        }
    }
}
