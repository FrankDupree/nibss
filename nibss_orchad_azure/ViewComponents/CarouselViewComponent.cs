using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.ViewComponents
{
    
    public class CarouselViewComponent : ViewComponent
    {
        private readonly IOrchardHelper _orchard;

        public CarouselViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ContentItem carousel = await _orchard.GetContentItemByHandleAsync($"alias:carousel");
            return View(carousel);
        }
    }
    
}
