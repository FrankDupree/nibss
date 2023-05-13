using nibss_orchad_azure.Helpers;
using Xunit;

namespace CodeCoverage
{
    public class MailTest
    {
        private readonly Mailer _sut;
        private readonly Mailer _sut1;
        private readonly Mailer _sut2;
        public MailTest()
        {
            _sut = new Mailer(Configuration.GetFakeIconfiguration());
            _sut1 = new Mailer(Configuration.GetFakeIconfigurationNoPort());
            _sut2 = new Mailer(Configuration.GetFakeIconfigurationNoSSL());
        }

        [Fact]
        public  void SendMailShouldReturnTrue()
        {
            var response = _sut.Sendmail("testMsg", "testSub", "website@nibss-plc.com.ng", "jaidadelahuya@gmail.com", null, null);
            Assert.True(response);
        }

        [Fact]
        public void FromIsNull()
        {
            var response = _sut.Sendmail("testMsg", "testSub", null, "jaidadelahuya@gmail.com", null, null);
            Assert.True(response);
        }

        [Fact]
        public void NoPort()
        {
            var response = _sut1.Sendmail("testMsg", "testSub", "jaidadelahuya@gmail.com", "jaidadelahuya@gmail.com", null, null);
            Assert.True(response);
        }

        [Fact]
        public void NoSsl()
        {
            var response = _sut2.Sendmail("testMsg", "testSub", "jaidadelahuya@gmail.com", "jaidadelahuya@gmail.com", null, null);
            Assert.True(!response);
        }

        [Fact]
        public void CopyIsNotNull()
        {
            var response = _sut.Sendmail("testMsg", "testSub", "website@nibss-plc.com.ng", "jaidadelahuya@gmail.com", "jaidadelahuya@gmail.com", null);
            Assert.True(response);
        }

        [Fact]
        public void FileIsNotNullAndNotFound()
        {
            var response = _sut.Sendmail("testMsg", "testSub", "jaidadelahuya@gmail.com", "jaidadelahuya@gmail.com", "jaidadelahuya@gmail.com", "/path");
            Assert.True(!response);
        }
    }
}
