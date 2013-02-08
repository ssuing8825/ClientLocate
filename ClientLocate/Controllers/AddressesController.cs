using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientLocate.Models;

namespace ClientLocate.Controllers
{   
    public class AddressesController : Controller
    {
		private readonly IPersonRepository personRepository;
		private readonly IAddressRepository addressRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public AddressesController() : this(new PersonRepository(), new AddressRepository())
        {
        }

        public AddressesController(IPersonRepository personRepository, IAddressRepository addressRepository)
        {
			this.personRepository = personRepository;
			this.addressRepository = addressRepository;
        }

        //
        // GET: /Addresses/

        public ViewResult Index()
        {
            return View(addressRepository.AllIncluding(address => address.Person));
        }

        //
        // GET: /Addresses/Details/5

        public ViewResult Details(int id)
        {
            return View(addressRepository.Find(id));
        }

        //
        // GET: /Addresses/Create

        public ActionResult Create()
        {
			ViewBag.PossiblePeople = personRepository.All;
            return View();
        } 

        //
        // POST: /Addresses/Create

        [HttpPost]
        public ActionResult Create(Address address)
        {
            if (ModelState.IsValid) {
                addressRepository.InsertOrUpdate(address);
                addressRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossiblePeople = personRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Addresses/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossiblePeople = personRepository.All;
             return View(addressRepository.Find(id));
        }

        //
        // POST: /Addresses/Edit/5

        [HttpPost]
        public ActionResult Edit(Address address)
        {
            if (ModelState.IsValid) {
                addressRepository.InsertOrUpdate(address);
                addressRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossiblePeople = personRepository.All;
				return View();
			}
        }

        //
        // GET: /Addresses/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(addressRepository.Find(id));
        }

        //
        // POST: /Addresses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            addressRepository.Delete(id);
            addressRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

