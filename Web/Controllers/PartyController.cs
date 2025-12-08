using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Implementation;
using Service.Interface;
using System;
using System.Linq;

namespace Web.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly IEstablishmentService _establishmentService;
        private readonly UserManager<AppUser> _userManager;

        public PartyController(IPartyService partyService, IEstablishmentService establishmentService, UserManager<AppUser> userManager)
        {
            _partyService = partyService;
            _establishmentService = establishmentService;
            _userManager = userManager;
        }

        // GET: Party
        //This stays, it is neccesarry for the attendee user
        //Need to give a user the ability to view all parties
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Establishment"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var myParties = _partyService.GetByUserId(user.Id); // your service returns List<Party>
                    return View(myParties);
                }
            }

            // default: show all parties
            var list = _partyService.GetAll();
            return View(list);
        }


        // GET: Party/Details/5
        //This stays, it is neccesarry for the attendee user
        //Need to give a user the ability to view all parties
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var party = _partyService.GetById(id.Value);
            if (party == null) return NotFound();

            // ensure Establishment nav prop is populated for the view
            if (party.Establishment == null && party.EstablishmentId != Guid.Empty)
            {
                party.Establishment = _establishmentService.GetById(party.EstablishmentId);
            }

            return View(party);
        }

        // GET: Party/Create
        //Create controllers have been changed and updated
        public IActionResult Create()
        {
            return View();
        }

        // POST: Party/Create

        [Authorize(Roles = "Establishment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartTime,EndTime,Description,PictureURL,TicketPrice,Capacity")] Party party)
        {
            if (!ModelState.IsValid)
                return View(party);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var establishment = _establishmentService.GetByUserId(user.Id);
            if (establishment == null)
                return BadRequest("No establishment linked to current user.");

            party.Id = Guid.NewGuid();
            party.EstablishmentId = establishment.Id;
            party.Establishment = establishment;

            _partyService.Add(party);
            return RedirectToAction(nameof(Index));
        }


        // GET: Party/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var party = _partyService.GetById(id.Value);
            if (party == null) return NotFound();

            return View(party);
        }

        // POST: Party/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Guid id, [Bind("Name,StartTime,EndTime,Description,PictureURL,EstablishmentId,Id")] Party party)
        {
            if (id != party.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(party);
            }
            try
            {
                _partyService.Update(party);
            }
            catch (Exception)
            {
                if (_partyService.GetById(party.Id) == null)
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Party/Delete/5

        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var party = _partyService.GetById(id.Value);
            if (party == null) return NotFound();

            if (party.Establishment == null && party.EstablishmentId != Guid.Empty)
            {
                party.Establishment = _establishmentService.GetById(party.EstablishmentId);
            }

            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _partyService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(Guid id)
        {
            return _partyService.GetById(id) != null;
        }
    }
}
