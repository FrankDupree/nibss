using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.ViewComponents
{

    
    public class PartnersCarouselViewComponent : ViewComponent
    {
        private readonly IOrchardHelper _orchard;

        public PartnersCarouselViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ContentItem partners = await _orchard.GetContentItemByHandleAsync($"alias:partners-carousel");
            return View(partners);
        }
    }
}
