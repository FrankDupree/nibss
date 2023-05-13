using Microsoft.AspNetCore.Mvc;

namespace nibss_orchad_azure.Controllers
{
    public class ErrorsController:Controller
    {
        [Route("Error/500")]
        public IActionResult Error500()
        {
            return View("~/views/page500.cshtml");
        }

        [Route("Error/404")]
        public IActionResult HandlePageNotFound()
        {
            return View("~/views/pageNotFound.cshtml");
        }
    }
}
