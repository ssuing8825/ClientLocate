using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientLocate.Models;

namespace ClientLocate.Controllers
{   
    public class PeopleController : Controller
    {
		private readonly IPersonRepository personRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public PeopleController() : this(new PersonRepository())
        {
        }

        public PeopleController(IPersonRepository personRepository)
        {
			this.personRepository = personRepository;
        }

        //
        // GET: /People/

        public ViewResult Index()
        {
            return View(personRepository.AllIncluding(person => person.Addresses, person => person.Phones));
        }

        //
        // GET: /People/Details/5

        public ViewResult Details(int id)
        {
            return View(personRepository.Find(id));
        }

        //
        // GET: /People/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /People/Create

        [HttpPost]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid) {
                personRepository.InsertOrUpdate(person);
                personRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /People/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(personRepository.Find(id));
        }

        //
        // POST: /People/Edit/5

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid) {
                personRepository.InsertOrUpdate(person);
                personRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /People/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(personRepository.Find(id));
        }

        //
        // POST: /People/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            personRepository.Delete(id);
            personRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

