using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly IOrchardHelper _orchard;
        public NavBarViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ContentItem mainMenu = await _orchard.GetContentItemByHandleAsync($"alias:main-menu");
            return View(mainMenu);
        }

    }
}
