using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nibss_orchad_azure.Services;

namespace nibss_orchad_azure.Controllers
{
    public class TrafController : Controller
    {
        private readonly TrafService TrafService;
        private readonly IConfiguration _configuration;
        public TrafController(TrafService trafService, IConfiguration configuration)
        {
            TrafService = trafService;
            _configuration = configuration;
        }

        [HttpGet("traf/pos")]
        [ValidateAntiForgeryToken]
        public async Task<string> GetPosData() {
            return await TrafService.GetData(_configuration["POS_URL"]);
        }

        [HttpGet("traf/nip")]
        [ValidateAntiForgeryToken]
        public async Task<string> GetNipData()
        {
            return await TrafService.GetData(_configuration["NIP_URL"]);
        }
    }
}
