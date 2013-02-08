using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientLocate.Models;

namespace ClientLocate.Controllers
{   
    public class PhonesController : Controller
    {
		private readonly IPersonRepository personRepository;
		private readonly IPhoneRepository phoneRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public PhonesController() : this(new PersonRepository(), new PhoneRepository())
        {
        }

        public PhonesController(IPersonRepository personRepository, IPhoneRepository phoneRepository)
        {
			this.personRepository = personRepository;
			this.phoneRepository = phoneRepository;
        }

        //
        // GET: /Phones/

        public ViewResult Index()
        {
            return View(phoneRepository.AllIncluding());
        }

        //
        // GET: /Phones/Details/5

        public ViewResult Details(int id)
        {
            return View(phoneRepository.Find(id));
        }

        //
        // GET: /Phones/Create

        public ActionResult Create()
        {
			ViewBag.PossiblePeople = personRepository.All;
            return View();
        } 

        //
        // POST: /Phones/Create

        [HttpPost]
        public ActionResult Create(Phone phone)
        {
            if (ModelState.IsValid) {
                phoneRepository.InsertOrUpdate(phone);
                phoneRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossiblePeople = personRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Phones/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossiblePeople = personRepository.All;
             return View(phoneRepository.Find(id));
        }

        //
        // POST: /Phones/Edit/5

        [HttpPost]
        public ActionResult Edit(Phone phone)
        {
            if (ModelState.IsValid) {
                phoneRepository.InsertOrUpdate(phone);
                phoneRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossiblePeople = personRepository.All;
				return View();
			}
        }

        //
        // GET: /Phones/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(phoneRepository.Find(id));
        }

        //
        // POST: /Phones/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            phoneRepository.Delete(id);
            phoneRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

