using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);

            var attendee = _service.GetByUserId(user.Id);
            if (attendee == null)
                return NotFound();

            return View(attendee);
        }
        //TODO Just in case this is a good idea
        //// Admin: view any attendee by ID
        //[Authorize(Roles = "Admin")]
        //public IActionResult Details(Guid id)
        //{
        //    var attendee = _service.GetById(id);
        //    if (attendee == null) return NotFound();
        //    return View(attendee);
        //}

        //// Attendee: view own profile
        //[Authorize(Roles = "Attendee")]
        //public async Task<IActionResult> MyProfile()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var attendee = _service.GetByUserId(user.Id);
        //    if (attendee == null) return NotFound();
        //    return View("Details", attendee);
        //}


        // GET: Attendee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Age")] Attendee attendee)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            attendee.Id = Guid.NewGuid();
            attendee.UserId = user.Id;
            attendee.User = user;
            attendee.Name = user.Name;
            _service.Add(attendee);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendee/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var attendee = _service.GetById(id.Value);
            if (attendee == null) return NotFound();
            return View(attendee);
        }

        // POST: Attendee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Age,Id")] Attendee attendee)
        {
            if (id != attendee.Id) return NotFound();

                _service.Update(attendee);
            //why can't i just add  var user = await _userManager.GetUserAsync(User); here,
            //then go user.password = attendee.password (lets imagine there is a passowrd in the bind and edit.cshtml)


            return RedirectToAction(nameof(Index));
        }

        // GET: Attendee/Delete/5
        public IActionResult Delete(Guid? id)
        {
            //ask teacher about the delete function and can you delete the whole user with it, or if you choose to delete the
            //profile can you just delete the attendee as well
            if (id == null) return NotFound();

            var attendee = _service.GetById(id.Value);
            if (attendee == null) return NotFound();

            return View(attendee);
        }

        // POST: Attendee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var deleted = _service.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeExists(Guid id)
        {
            return _service.GetById(id) != null;
        }
    }
}
