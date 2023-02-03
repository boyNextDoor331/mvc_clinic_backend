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

            return View("CreateClient", user);
        }

        [HttpGet]
        public IActionResult Find()
        {
            return View("FindClient");
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

        [HttpGet]
        public IActionResult Delete()
        {
            return View("DeletingForm");
        }


        [HttpPost]
        public IActionResult DeleteSpecific([FromForm] string id)
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
            list.Remove(list.Find(client => client.Id == id));

            using (var fileStream = new FileStream("Users.bin", FileMode.Open, FileAccess.Write, FileShare.None))
            {
                foreach(var client in list)
                {
                    byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(client + "\n"));
                    fileStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            return Ok("Nice");
        }
    }
}
