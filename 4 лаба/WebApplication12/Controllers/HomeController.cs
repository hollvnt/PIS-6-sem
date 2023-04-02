using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication12.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication12.Controllers
{
   public class HomeController : Controller
    {
        private readonly PhonebookContext _сontext;
        public HomeController(PhonebookContext context)
        {
            _сontext = context;
        
        }

        public IActionResult Index()
        {
            List<Entry> phonebook = _сontext.Entries.OrderBy(e => e.LastName).ToList();
            return View(phonebook);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddSave(Entry entry)
        {
            _сontext.Entries.Add(entry);
            _сontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public ActionResult Update(int id)
        {
            Entry entry = _сontext.Entries.FirstOrDefault(e => e.Id == id);
            if(entry != null)
            {
                return View(entry);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]

        public ActionResult UpdateSave(int id, string lastName, string phoneNumber)
        {
            Entry entry = _сontext.Entries.FirstOrDefault(e => e.Id == id);
            if( entry != null)
            {
                entry.LastName = lastName;
                entry.PhoneNumber = phoneNumber;
                _сontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        
        public ActionResult Delete(int id)
        {
            Entry entry = _сontext.Entries.FirstOrDefault(e => e.Id == id);
            if(entry != null) return View(entry);
            return RedirectToAction("Error");
        }

        [HttpPost]

        public ActionResult DeleteSave(int id)
        {
            Entry entry = _сontext.Entries.FirstOrDefault(e => e.Id == id);
            if(entry != null)
            {
                _сontext.Entries.Remove(entry);
                _сontext.SaveChanges();
            }
            return RedirectToAction("Index");
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