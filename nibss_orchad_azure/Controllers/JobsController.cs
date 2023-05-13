using Account.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using nibss_orchad_azure.Helpers;
using OrchardCore;
using OrchardCore.ContentManagement;
using Newtonsoft.Json;
using nibss_orchad_azure.Models;

namespace nibss_orchad_azure.Controllers
{
    public class JobsController: Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOrchardHelper _orchardHelper;

        public JobsController(IConfiguration configuration, IOrchardHelper orchardHelper)
        {
            _configuration = configuration;
            _orchardHelper = orchardHelper;
        }

        [HttpGet("jobs")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("careers/init")]
        public async Task<IActionResult> Init()
        {
            var result = await _orchardHelper.QueryAsync("Openings");
            List<JobViewModel> jobs = new List<JobViewModel>();
            foreach (ContentItem data in result)
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(JsonConvert.SerializeObject(data));
                var job = new JobViewModel
                {
                    DeadLine = myDeserializedClass.JobOpenings.DeadLine.Value,
                    Role = myDeserializedClass.JobOpenings.Role.Text,
                    MinQ = myDeserializedClass.JobOpenings.MinQ.Text,
                    Description = myDeserializedClass.JobOpenings.Description.Html,
                    Location = myDeserializedClass.JobOpenings.Location.Text,
                    Id = myDeserializedClass.ContentItemId,
                    Experience = Int32.Parse(myDeserializedClass.JobOpenings.Experience.Text)
                };

                jobs.Add(job);
            }

            return Ok(jobs);
        }

        [HttpGet("careers/GetDegrees")]
        public IActionResult GetDegrees()
        {
            string folderPath = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot", "content");
            var lines = System.IO.File.ReadLines(Path.Combine(folderPath,"degrees.csv")).Select(a => a.Split(';'));
            IDictionary<string, int> hashtable = new SortedDictionary<string, int>();



            foreach (var item in lines)
            {
                var data = item[0].Split(',');
                hashtable.Add(data[1], Int32.Parse(data[0]));
            }
            var hashtablz = hashtable.OrderBy(p => p.Value).ToList();
            return Ok(hashtablz);
        }



        [HttpPost("careers/submitcv")]
        public async Task<IActionResult> SubmitCv(Applicant model, IFormFile cv)
        {
            bool success = false;
            if (ModelState.IsValid)
            {
                if (cv == null || cv.Length == 0)
                    return Content("file not selected");

                string extension = Path.GetExtension(cv.FileName)?.ToLower();

                if (extension != ".pdf" && extension != ".doc" && extension != ".docx" && extension != ".rtf" && extension != ".txt")
                {
                    ModelState.AddModelError("uploadError", "Supported file extensions: pdf, doc, docx, rtf, txt");
                    return Json(new { Success = success });
                }

                var newFileName = Guid.NewGuid().ToString() + "_" + cv.FileName;

                var path = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot", "cv",
                  newFileName);
                
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await cv.CopyToAsync(stream);
                }

                Mailer mail = new Mailer(_configuration);

                

                //TDOD: map all job details to applicant including degree
                var result = mail.Sendmail(
                    $@"
                        Name: {model.FirsName} {model.Surname}
                        Email: {model.Email}
                        Phone: {model.Phone}
                        Job Role: {model.Role}
                        Qualification: {model.Degree}
                    ",
                    $"C.V SUBMISSION - {model.Email}",
                    _configuration["smtp_user"],
                    _configuration["HrEmail"],
                    null,
                    path
                    );


                success = result;
            }
            return Json(new { Success = success });
        }
    }
}
