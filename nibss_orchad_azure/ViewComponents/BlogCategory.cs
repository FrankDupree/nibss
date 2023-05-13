using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OrchardCore;


namespace nibss_orchad_azure.ViewComponents
{
    public class BlogCategory : ViewComponent
    {
        
        private readonly IOrchardHelper _orchard;
        public BlogCategory(IOrchardHelper orchard)
        {
            _orchard = orchard;
            
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var selection = await _orchard.ContentQueryAsync("AllBlogCategories");
            return View(selection);
        }
    }
}
