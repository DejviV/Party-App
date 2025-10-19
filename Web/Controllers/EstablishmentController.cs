using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Service.Interface;
using Microsoft.AspNetCore.Identity;

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

            // populate User nav if missing (view expects .User)
            if (establishment.User == null && !string.IsNullOrEmpty(establishment.UserId))
            {
                var user = _userManager.FindByIdAsync(establishment.UserId).GetAwaiter().GetResult();
                establishment.User = user;
            }

            return View(establishment);
        }

        // GET: Establishment/Create
        public IActionResult Create()
        {
            var users = _userManager.Users
                .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                .ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Display");
            return View();
        }

        // POST: Establishment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Address,Capacity,PictureURL,UserId,Id")] Establishment establishment)
        {
            if (!ModelState.IsValid)
            {
                // repopulate users for the view
                var users = _userManager.Users
                    .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                    .ToList();
                ViewData["UserId"] = new SelectList(users, "Id", "Display", establishment.UserId);
                return View(establishment);
            }

            if (establishment.Id == Guid.Empty) establishment.Id = Guid.NewGuid();

            _service.Add(establishment);
            return RedirectToAction(nameof(Index));
        }

        // GET: Establishment/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var establishment = _service.GetById(id.Value);
            if (establishment == null) return NotFound();

            var users = _userManager.Users
                .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                .ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Display", establishment.UserId);

            return View(establishment);
        }

        // POST: Establishment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Address,Capacity,PictureURL,UserId,Id")] Establishment establishment)
        {
            if (id != establishment.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var users = _userManager.Users
                    .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                    .ToList();
                ViewData["UserId"] = new SelectList(users, "Id", "Display", establishment.UserId);
                return View(establishment);
            }

            try
            {
                _service.Update(establishment);
            }
            catch (Exception)
            {
                // if service provides concurrency exceptions, handle specifically.
                if (_service.GetById(establishment.Id) == null)
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Establishment/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var establishment = _service.GetById(id.Value);
            if (establishment == null) return NotFound();

            if (establishment.User == null && !string.IsNullOrEmpty(establishment.UserId))
            {
                var user = _userManager.FindByIdAsync(establishment.UserId).GetAwaiter().GetResult();
                establishment.User = user;
            }

            return View(establishment);
        }

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
