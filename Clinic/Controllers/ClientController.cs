using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace Clinic.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpPost] 
        public IActionResult CreateClient(
            [FromForm] string name, [FromForm] string surname,
            [FromForm] int age, [FromForm] string gender,
            [FromForm] string email)
        {
            var user = new Client(name, surname, age, gender, email);

            using (var fileStream = new FileStream("users.bin", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(user));
                fileStream.WriteAsync(buffer, 0, buffer.Length);
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var list = new List<Client>();
            using (var fileStream = new StreamReader("users.bin"))
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
            return View(list);
        }

    }
}
