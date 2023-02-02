using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace Clinic.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View("RegistrationForm");
        }


        [HttpPost] 
        public IActionResult Create(
            [FromForm] string name, [FromForm] string surname,
            [FromForm] int age, [FromForm] string gender,
            [FromForm] string email)
        {
            var user = new Client(name, surname, age, gender, email);

            using (var fileStream = new FileStream("Users.bin", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(user)+"\n");
                fileStream.WriteAsync(buffer, 0, buffer.Length);
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Find()
        {
            return View("FindClientS");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<Client>();
            using (var fileStream = new StreamReader("Users.bin"))
            {
                while (!fileStream.EndOfStream)
                {
                    var stream = fileStream.ReadLine();
                    if(!string.IsNullOrEmpty(stream))
                    {
                        list.Add(JsonConvert.DeserializeObject<Client>(stream));
                    }
                }
            }
            return View("AllClientsTable", list);
        }

        [HttpPost]
        public IActionResult GetSpecific([FromForm] string name, [FromForm] string surname)
        {
            var list = new List<Client>();
            using (var fileStream = new StreamReader("Users.bin"))
            {
                while (!fileStream.EndOfStream)
                {
                    var stream = fileStream.ReadLine();
                    if (!string.IsNullOrEmpty(stream))
                    {
                        list.Add(JsonConvert.DeserializeObject<Client>(stream));
                    }
                }
            }
            return View("ClientInformation", list.Find(client => client.IsClientNeeded(name, surname)));
        }


    }
}
