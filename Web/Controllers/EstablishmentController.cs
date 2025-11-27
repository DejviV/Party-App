using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class EstablishmentController : Controller
    {
        private readonly IEstablishmentService _service;
        private readonly UserManager<AppUser> _userManager;

        public EstablishmentController(IEstablishmentService establishmentService, UserManager<AppUser> userManager)
        {
            _service = establishmentService;
            _userManager = userManager;
        }

        // GET: Establishment
        public IActionResult Index()
        {
            var list = _service.GetAll();
            return View(list);
        }

        // GET: Establishment/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var establishment = _service.GetById(id.Value);
            if (establishment == null) return NotFound();

            return View(establishment);
        }

        // GET: Establishment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Establishment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async  Task<IActionResult> Create([Bind("Address,Capacity,PictureURL,Id")] Establishment establishment)
        {
            //if (!ModelState.IsValid)
            //{
            //    establishment.Id = Guid.NewGuid();
            //    establishment.UserId = user.Id;
            //    establishment.User = user;
            //    establishment.Name = user.Name;
            //    _service.Add(establishment);
            //    
            //} Ask teacher which one is correct and why
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            establishment.Id = Guid.NewGuid();
            establishment.UserId = user.Id;
            establishment.User = user;
            establishment.Name = user.Name;

            _service.Add(establishment);
            //Najdi kako da gi napravish ovie RedirectToAction da pokazuvaat kon
            //site zabavi shto se organizirani od strana na establishemnt
            return RedirectToAction(nameof(Index));
            //This is the return part that was generated while the other one is the one you copied from attendee,
            //check that one too its about the if (!ModelState.IsValid)
        }

        // GET: Establishment/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var establishment = _service.GetById(id.Value);
            if (establishment == null) return NotFound();

            return View(establishment);
        }

        // POST: Establishment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Guid id, [Bind("Name,Address,Capacity,PictureURL,Id")] Establishment establishment)
        {
            if (id != establishment.Id) return NotFound();

            //Tuka imashe try/catch generirano, no bidejki samo user-ot bi mozel da smeni neshto za svojot establishment
            //ne gledam prichna za concurency
                _service.Update(establishment);

            return RedirectToAction(nameof(Index));
        }

        // GET: Establishment/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var establishment = _service.GetById(id.Value);
            if (establishment == null) return NotFound();

            return View(establishment);
        }
        //Delete e komplicirano bidejki bi sakal so brishenje na establishment da moze da se izbrishe i user-ot
        //ili obratno, no ne znam kolku e toa mozno

        // POST: Establishment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EstablishmentExists(Guid id)
        {
            return _service.GetById(id) != null;
        }
    }
}
