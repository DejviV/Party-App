using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace Web.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly IAttendeeService _service;
        private readonly UserManager<AppUser> _userManager;

        public AttendeeController(IAttendeeService attendeeService, UserManager<AppUser> userManager)
        {
            _service = attendeeService;
            _userManager = userManager;
        }

        // GET: Attendee
        public IActionResult Index()
        {
            var list = _service.GetAll();
            return View(list);
        }

        // GET: Attendee/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var attendee = _service.GetById(id.Value);
            if (attendee == null) return NotFound();

            // ensure User is populated for the view (service may or may not include it)
            if (attendee.User == null && !string.IsNullOrEmpty(attendee.UserId))
            {
                var user = _userManager.FindByIdAsync(attendee.UserId).GetAwaiter().GetResult();
                attendee.User = user;
            }

            return View(attendee);
        }

        // GET: Attendee/Create
        public IActionResult Create()
        {
            // populate users dropdown (Id, display name)
            var users = _userManager.Users
                .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "Display");
            return View();
        }

        // POST: Attendee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Age,UserId,Id")] Attendee attendee)
        {
            if (!ModelState.IsValid)
            {
                // re-populate users
                var users = _userManager.Users
                    .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                    .ToList();
                ViewData["UserId"] = new SelectList(users, "Id", "Display", attendee.UserId);
                return View(attendee);
            }

            // ensure id (if your BaseEntity doesn't set it automatically)
            if (attendee.Id == Guid.Empty) attendee.Id = Guid.NewGuid();

            _service.Add(attendee);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendee/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var attendee = _service.GetById(id.Value);
            if (attendee == null) return NotFound();

            var users = _userManager.Users
                .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                .ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Display", attendee.UserId);

            return View(attendee);
        }

        // POST: Attendee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Age,UserId,Id")] Attendee attendee)
        {
            if (id != attendee.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var users = _userManager.Users
                    .Select(u => new { u.Id, Display = (u.UserName ?? u.Email) })
                    .ToList();
                ViewData["UserId"] = new SelectList(users, "Id", "Display", attendee.UserId);
                return View(attendee);
            }

            try
            {
                _service.Update(attendee);
            }
            catch (Exception ex)
            {
                // If your service throws a concurrency exception, adapt here.
                // We try to detect if entity still exists:
                if (_service.GetById(attendee.Id) == null)
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendee/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var attendee = _service.GetById(id.Value);
            if (attendee == null) return NotFound();

            // populate attendee.User if missing
            if (attendee.User == null && !string.IsNullOrEmpty(attendee.UserId))
            {
                var user = _userManager.FindByIdAsync(attendee.UserId).GetAwaiter().GetResult();
                attendee.User = user;
            }

            return View(attendee);
        }

        // POST: Attendee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var deleted = _service.DeleteById(id);
            // you can check deleted == null to detect failure if service returns null on missing
            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeExists(Guid id)
        {
            return _service.GetById(id) != null;
        }
    }
}
