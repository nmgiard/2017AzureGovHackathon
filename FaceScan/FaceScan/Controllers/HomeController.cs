using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FaceScan.Models;
using FaceAPIWrapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FaceScan.Controllers
{
    public class HomeController : Controller
    {

        
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private string _apikey;
        private string _apiuri;
        private string _groupId;

        public string Apikey { get => _apikey; }
        public string Apiuri { get => _apiuri; }
        public string GroupId { get => _groupId; }

        public HomeController(IHostingEnvironment environment, IConfigurationRoot config )
        {
            _environment = environment;
            _configuration = config;
            _apikey = _configuration.GetValue<string>("FaceAPIKey");
            _apiuri = _configuration.GetValue<string>("FaceAPIEndPoint");
            _groupId = _configuration.GetValue<string>("FaceGroupId");
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Person(string personId)
        {
            ViewData["Message"] = "Selected person";
            if (personId != null) {

                
                var users = await DocumentDBRepository<User>.GetItemsAsync(d => d.Domain == "myorg.net" && d.PersonId == personId);

                if (users != null && users.Count() > 0)
                {
                    return View(users.FirstOrDefault());
                }
            } 
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Welcome to a demonstration of the Azure Cognitive Face API";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Information";

            return View();
        }

        public async Task<IActionResult> Import()
        {
            var users = await DocumentDBRepository<User>.GetItemsAsync(d => d.Domain == "myorg.net");

            FaceAPIClient client = new FaceAPIClient( Apikey, Apiuri, GroupId );
            client.CreatePersonGroup("FaceScanDirectory");

            var t = Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(5.0));
                return 42;
            });
            t.Wait();

            foreach (User user in users)
            {

                try
                {
                    var pgr = client.CreatePerson(string.Format("{0} {1}", user.First, user.Last), JsonConvert.SerializeObject(user));

                    if (pgr.PersonId != null)
                    {
                        user.PersonId = pgr.PersonId.ToString();
                        await DocumentDBRepository<User>.UpdateItemAsync(user.Id, user);
                        string[] uri = new string[1];
                        uri[0] = user.UserPhoto;
                        client.AddFacesToPerson(pgr.PersonId, user.UPN, uri);
                    }

                    var w = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10.0));
                        return 42;
                    });
                    w.Wait();
                }
                catch (Exception e) {
                    
                }

            }
            return View(users);

        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {

            Guid person;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    FaceAPIClient client = new FaceAPIClient(Apikey, Apiuri, GroupId);
                    person = client.IdentifyFaceFromStream(formFile.OpenReadStream());
                }
            }

            if( person == null || person.Equals(Guid.Empty))
            {
                ViewData["Results"] = "NO";
                return View("Index");
            }
            return RedirectToAction("Person", "Home", new { personId = person });
        }

        [HttpPost]
        public JsonResult UploadImg(string imageData)
        {
            Guid person;

            if (imageData == null || imageData.Length == 0)
            {
                return Json(new { personId = "" });
            }
            else
            {
                
                byte[] data = Convert.FromBase64String(imageData);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    FaceAPIClient client = new FaceAPIClient(Apikey, Apiuri, GroupId);
                    person = client.IdentifyFaceFromStream(ms);
                }
            }

            if (person == null || person.Equals(Guid.Empty))
            {
                return Json(new { personId = "" });
            }
            return Json(new { personId = person.ToString() });

        }

        [HttpPost]
        public JsonResult UploadImg2(string imageData)
        {

            string groupid = "facescandir2017";
            Guid person;

            if (imageData == null || imageData.Length == 0)
            {
                return Json(new { personId = ""});
            }
            else
            {

                byte[] data = Convert.FromBase64String(imageData);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    FaceAPIClient client = new FaceAPIClient(Apikey, Apiuri, GroupId);
                    person = client.IdentifyFaceFromStream(ms);
                }
            }

            if (person == null || person.Equals(Guid.Empty))
            {
                return Json(new { personId = "" });
            }
            return Json(new { personId = person.ToString() });

        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
