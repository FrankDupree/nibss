using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        private readonly IOrchardHelper _orchard;

        public FooterViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = null)
        {
            IEnumerable<ContentItem> blogPosts;
            if (!string.IsNullOrEmpty(viewName))
            {
                blogPosts = await _orchard.GetRecentContentItemsByContentTypeAsync("BlogPost", 4);
                return View(viewName, blogPosts);
            }
            else {
                blogPosts = await _orchard.GetRecentContentItemsByContentTypeAsync("BlogPost", 3);
                return View(blogPosts);
            }
            
        }
    }
}
