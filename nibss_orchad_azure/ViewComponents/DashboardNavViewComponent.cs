using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OrchardCore;

namespace nibss_orchad_azure.ViewComponents
{
    public class DashboardNavViewComponent : ViewComponent
    {
        private readonly IOrchardHelper _orchard;
        public DashboardNavViewComponent(IOrchardHelper orchard)
        {
            _orchard = orchard;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var selection = await _orchard.ContentQueryAsync("AllIndustryStat");
            return View(selection);
        }
    }
}
