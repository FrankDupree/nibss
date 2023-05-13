using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Account.Helpers
{
    public class NibssController:Controller
    {
        public ActionResult CurrentNibssPage()
        {
            
            string currentUrl = HttpContext?.Request?.GetDisplayUrl();
            return View(currentUrl);
        }
    }
}
