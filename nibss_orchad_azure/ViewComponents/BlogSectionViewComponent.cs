using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.ViewComponents
{
    public class BlogSectionViewComponent : ViewComponent
    {

        private readonly IOrchardHelper _orchard;
        public BlogSectionViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ContentItem> blogPosts = await _orchard.GetRecentContentItemsByContentTypeAsync("BlogPost", 3);
            return View(blogPosts);
        }
    }
}
