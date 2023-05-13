using Microsoft.AspNetCore.Mvc;
using nibss_orchad_azure.Controllers;
using nibss_orchad_azure.Services;
using Xunit;
using OrchardCore.DisplayManagement.Views;
using Microsoft.Extensions.Configuration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;
using nibss_orchad_azure.Drivers;
using Account.Models;

namespace CodeCoverage
{
    public class ErrorControllerTest
    {
        ErrorsController _controller;
        TrafController _trafController;


        [Fact]
        public async void GetTrafNip()
        {
            // Arrange
            TrafService trafService = new TrafService();
            var myConfiguration = new Dictionary<string, string>
            {
                  {"NIP_URL", "https://traf.nibss-plc.com.ng:7443/traf/ajax?command=website&action=detail&order=loadNIP&clientCode=NIBSS&txnSubCat=ALL"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration)
            .Build();
            _trafController = new TrafController(trafService,configuration);

            // act
            //Act
            string response = await _trafController.GetNipData();

            Assert.IsNotNull(response);
           
        }

        [Fact]
        public async void GetTrafPos()
        {
            // Arrange
            TrafService trafService = new TrafService();
            var myConfiguration = new Dictionary<string, string>
            {
                  {"POS_URL", "https://traf.nibss-plc.com.ng:7443/traf/ajax?command=website&action=detail&order=loadPOS&clientCode=NIBSS&txnSubCat=ALL"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration)
            .Build();
            _trafController = new TrafController(trafService, configuration);

            // act
            //Act
            string response = await _trafController.GetPosData();

            Assert.IsNotNull(response);

        }


        [Fact]
        public  void ShouldReturnInternalErrorPage()
        {
            // Arrange
            _controller = new ErrorsController();

            // act
            //Act
            IActionResult result =  _controller.Error500();

            // We cast it to the expected response type
            ViewResult okResult = result as ViewResult;

            Assert.IsNotNull(result);
            if (okResult != null) Assert.AreEqual("~/views/page500.cshtml", okResult.ViewName);
        }

        [Fact]
        public void ShouldReturnNotFoundPage()
        {
            // Arrange
            _controller = new ErrorsController();

            // act
            //Act
            IActionResult result = _controller.HandlePageNotFound();

            // We cast it to the expected response type
            ViewResult okResult = result as ViewResult;

            Assert.IsNotNull(result);
            if (okResult != null) Assert.AreEqual("~/views/pageNotFound.cshtml", okResult.ViewName);
        }

        [Fact]
        public void EditProfile() {

            UserProfileDisplayDriver userProfileDisplayDriver = new UserProfileDisplayDriver();
             //Mock<BuildEditorContext> context = new Mock<BuildEditorContext>();
            UserProfile userProfile = new UserProfile();
            userProfile.Organisation = "org";
            userProfile.Name = "name";
            userProfile.Sector = "sector";
            IDisplayResult result = userProfileDisplayDriver.Edit(userProfile, null);

            Assert.NotNull(result);
        }

        

        [Fact]
        public async void UpdateAsync()
        {

            UserProfileDisplayDriver userProfileDisplayDriver = new UserProfileDisplayDriver();
            IUpdateModel updater = new MockUpdateModel();
            BuildEditorContext context = new BuildEditorContext(null,null,true,null,null,null,updater);
            UserProfile userProfile = new UserProfile();
            userProfile.Organisation = "org";
            userProfile.Name = "name";
            userProfile.Sector = "sector";
            IDisplayResult result = await userProfileDisplayDriver.UpdateAsync(userProfile, context);

            Assert.NotNull(result);
        }

    }
}
