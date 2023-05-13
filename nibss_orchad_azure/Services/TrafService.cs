using System.Net.Http;

using System.Threading.Tasks;

namespace nibss_orchad_azure.Services
{
    public class TrafService
    {
        public async Task<string> GetData(string Url) {
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Url))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                
                }
            }
            return apiResponse;
        }

       
    }
}
