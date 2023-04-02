using System.Collections.Generic;
using System.Diagnostics;
using WebApplication12.Models;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication12.Controllers
{
    public class HomeController : Controller
    {
        private readonly string filePath = @"D:\\3 курс 2 сем\\Программирование internet-серверов\\WebApplication10\\phonebook.json"; 

        public ActionResult Index()
        {
            List<Entry> phonebook = LoadPhonebook();
            return View(phonebook.OrderBy(e => e.LastName).ToList()); // отображение списка справочника, отсортированного по алфавиту
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSave(string lastName, string phoneNumber)
        {
            List<Entry> phonebook = LoadPhonebook();
            int maxId = phonebook.Count > 0 ? phonebook.Max(e => e.Id) : 0;
            phonebook.Add(new Entry { Id = maxId + 1, LastName = lastName, PhoneNumber = phoneNumber });
            SavePhonebook(phonebook);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            List<Entry> phonebook = LoadPhonebook();
            Entry entry = phonebook.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                return View(entry);
            }
            return RedirectToAction("Error");
        }


        [HttpPost]
        public ActionResult UpdateSave(int id, string lastName, string phoneNumber)
        {
            List<Entry> phonebook = LoadPhonebook();
            Entry entry = phonebook.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                entry.LastName = lastName;
                entry.PhoneNumber = phoneNumber;
                SavePhonebook(phonebook);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error");
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            List<Entry> phonebook = LoadPhonebook();
            Entry entry = phonebook.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                return View(entry);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        public ActionResult DeleteSave(int id)
        {
            List<Entry> phonebook = LoadPhonebook();
            List<Entry> entriesToDelete = phonebook.Where(e => e.Id == id).ToList();
            if (entriesToDelete.Any())
            {
                phonebook.RemoveAll(e => e.Id == id);
                SavePhonebook(phonebook);
            }
            return RedirectToAction("Index");
        }


        private List<Entry> LoadPhonebook()
        {
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Entry>>(json)?.OrderBy(e => e.Id).ToList() ?? new List<Entry>(); ;
            }
            return new List<Entry>();
        }

        private void SavePhonebook(List<Entry> phonebook)
        {
            string json = JsonConvert.SerializeObject(phonebook);
            System.IO.File.WriteAllText(filePath, json);
        }

        public IActionResult Error()
        {
            string method = HttpContext.Request.Method;
            string uri = HttpContext.Request.Path;
            string message = $"{method}: {uri} не поддерживается";
            ViewBag.ErrorMessage = message;
            return View();
        }

    }

}

