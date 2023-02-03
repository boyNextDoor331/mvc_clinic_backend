using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
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

            using (var fileStream = new StreamWriter("Users.bin", true))
            {
                fileStream.WriteLineAsync(JsonConvert.SerializeObject(user));
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
            using (var fileStreamRead = new StreamReader("Users.bin"))
            {
                while (!fileStreamRead.EndOfStream)
                {
                    var stream = fileStreamRead.ReadLine();
                    if (!string.IsNullOrEmpty(stream))
                    {
                        list.Add(JsonConvert.DeserializeObject<Client>(stream));
                    }
                }
            }
            using (var fileStreamClear = new FileStream("Users.bin", FileMode.Truncate)) 
            { 

            }
            
            list.Remove(list.Find(client => client.Id == id));

            using (var fileStreamWrite = new StreamWriter("Users.bin", true))
            {
                foreach(var client in list)
                {
                    fileStreamWrite.WriteLineAsync(JsonConvert.SerializeObject(client));
                }
            }
            return View("SuccessDeleting");
        }
    }
}
