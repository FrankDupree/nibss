using nibss_orchad_azure.Helpers;
using nibss_orchad_azure.Services;
using System.Threading.Tasks;
using Xunit;


namespace CodeCoverage
{
    public class CaptchaTest
    {
        private readonly CaptchaVerificationService _sut2;
        public CaptchaTest()
        {
            _sut2 = new CaptchaVerificationService(Configuration.GetFakeIconfiguration());
        }

        [Fact]
        public async Task OnNotVerifyCaptchaReturnFalse()
        {
            var response = await _sut2.IsCaptchaValid("");
            Assert.False(response);
        }
    }
}
