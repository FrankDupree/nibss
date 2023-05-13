using Account.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using nibss_orchad_azure.Controllers;
using OrchardCore;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace CodeCoverage
{
    public class JobControllerTest
    {
        JobsController _controller;
        private Mock<IOrchardHelper> _orchardHelper;
        private Mock<IConfiguration> _configuration;


        [Fact]
        public async Task SubmitCV_Should_ReturnFileNotSelected_If_CV_IsBlank()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
          
            _configuration = new Mock<IConfiguration>();


            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);

            //Instantiate with a incomplete model
            var model = new Applicant
            {
                FirsName = "Paul",
                Surname = null
            };

            // act
            //Act
            IActionResult result = await _controller.SubmitCv(model, null);

            // We cast it to the expected response type
            ContentResult okResult = result as ContentResult;

            Assert.IsNotNull(result);
            if (okResult != null) Assert.AreEqual("file not selected", okResult.Content);
        }

        [Fact]
        public void  Index()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
         
            _configuration = new Mock<IConfiguration>();


            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);

            //Act
            IActionResult result = _controller.Index();

            Assert.IsNotNull(result);
            
        }

        [Fact]
        public void GetDegreess_Should_Return_Degree()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
            new Mock<IContentManager>();
            _configuration = new Mock<IConfiguration>();

            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);

            string contentPath = Path.Combine("wwwroot", "content");
            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
                string basPath = Configuration.getBasePath("Resources");
                File.Copy(Path.Combine(basPath, "degrees.csv"), Path.Combine(contentPath, "degrees.csv"));
            }

            //Act
            IActionResult result =  _controller.GetDegrees();

            // We cast it to the expected response type
            OkObjectResult okResult = result as OkObjectResult;

            var model = okResult?.Value as List<KeyValuePair<string, int>>;

            Assert.IsNotNull(result);
            if (model != null) Assert.AreEqual(4, model.Count);
        }


        [Fact]
        public async Task SubmitCV_Should_ReturnSuccessFalse_If_ModelState_IsBad()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
           
            _configuration = new Mock<IConfiguration>();


            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);

            // Arrange.
            var fileMock = new Mock<IFormFile>();
            string basPath = Configuration.getBasePath("Resources");
            var physicalFile = new FileInfo(Path.Combine(basPath, "ams.docx"));
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            var file = fileMock.Object;

            //Instantiate with a incomplete model
            var model = new Applicant
            {
                FirsName = "Paul",
                Surname = null
            };

            //Lets add a bad modelState
            _controller.ModelState.AddModelError("FirstName", "Required");

            //Act
            IActionResult result = await _controller.SubmitCv(model, file);

            // We cast it to the expected response type
            JsonResult okResult = result as JsonResult;
            if (okResult != null)
            {
                var dynData = (dynamic)okResult.Value;
                Assert.IsNotNull(result);
                Assert.AreEqual(false, dynData.GetType().GetProperty("Success").GetValue(dynData, null));
            }
        }

        [Fact]
        public async Task CvIsNull()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
          
            _configuration = new Mock<IConfiguration>();


            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);


            var model = new Applicant
            {
                FirsName = "Paul",
                Surname = "Ade",
                Degree = "Bsc",
                Email = "paul@gmail.com",
                Phone = "08122233301",
                Role = "Coder",
                Cv = ""
            };


            //Act
            IActionResult result = await _controller.SubmitCv(model, null);

            // We cast it to the expected response type
            ContentResult okResult = result as ContentResult;
            if (okResult != null)
            {
                
             
                Assert.AreEqual( okResult.Content, "file not selected");
            }
        }

        [Fact]
        public async Task CvHasUnaccpetedExtension()
        {
            _orchardHelper = new Mock<IOrchardHelper>();
           
            _configuration = new Mock<IConfiguration>();


            // Arrange
            _controller = new JobsController(_configuration.Object, _orchardHelper.Object);

            // Arrange.
            var fileMock = new Mock<IFormFile>();
            string basPath = Configuration.getBasePath("Resources");
            var physicalFile = new FileInfo(Path.Combine(basPath, "degrees.csv"));
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            var file = fileMock.Object;

            var model = new Applicant
            {
                FirsName = "Paul",
                Surname = "Ade",
                Degree = "Bsc",
                Email = "paul@gmail.com",
                Phone = "08122233301",
                Role = "Coder",
                Cv = ""
            };


            //Act
            IActionResult result = await _controller.SubmitCv(model, file);

            // We cast it to the expected response type
            // We cast it to the expected response type
            JsonResult okResult = result as JsonResult;
            if (okResult != null)
            {
                var dynData = (dynamic)okResult.Value;
                Assert.IsNotNull(result);
                Assert.AreEqual(false, dynData.GetType().GetProperty("Success").GetValue(dynData, null));
            }
        }

        [Fact]
        public async Task SubmitSuccess()
        {
            _orchardHelper = new Mock<IOrchardHelper>();



            // Arrange
            _controller = new JobsController(Configuration.GetFakeIconfiguration(), _orchardHelper.Object);

            // Arrange.
            var fileMock = new Mock<IFormFile>();
            string basPath = Configuration.getBasePath("Resources");
            var physicalFile = new FileInfo(Path.Combine(basPath, "ams.docx"));
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            var file = fileMock.Object;

            var model = new Applicant
            {
                FirsName = "Paul",
                Surname = "Ade",
                Degree = "Bsc",
                Email = "paul@gmail.com",
                Phone = "08122233301",
                Role = "Coder",
                Cv = ""
            };


            //Act
            IActionResult result = await _controller.SubmitCv(model, file);

            // We cast it to the expected response type
            // We cast it to the expected response type
            JsonResult okResult = result as JsonResult;
            if (okResult != null)
            {
                var dynData = (dynamic)okResult.Value;
                Assert.IsNotNull(result);
                Assert.AreEqual(true, dynData.GetType().GetProperty("Success").GetValue(dynData, null));
            }
        }
    }
}
