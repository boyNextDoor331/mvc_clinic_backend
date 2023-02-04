﻿using Clinic.Engine;
using Clinic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Clinic.Controllers
{
    public class ClientController : Controller
    {
        private const string FileName = "Users";
        private const string BackupName = "BackupUsers";

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

            DataManager.Write(user, FileName);

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
            DataManager.Read(list, FileName);
            return View("AllClientsTable", list);
        }

        [HttpPost]
        public IActionResult GetSpecific([FromForm] string name, [FromForm] string surname)
        {
            var list = new List<Client>();
            DataManager.Read(list, FileName);
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
            DataManager.Read(list, FileName);
            try
            {
                DataManager.Truncate(FileName);
            }
            finally
            {
                DataManager.Write(list, BackupName);
            }
            list.Remove(list.Find(client => client.IsClientNeeded(id)));
            DataManager.Write(list, FileName);
            return View("SuccessDeleting");
        }
    }
}
