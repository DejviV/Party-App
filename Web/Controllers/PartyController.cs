using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Index()
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        
        public IActionResult Create([Bind("Name,StartTime,EndTime,Description,PictureURL")] Party party)
        {
            if (!ModelState.IsValid)
            { 
                return View(party);
            }
           var establishment = _establishmentService.GetByUserId(_userManager.GetUserId(User));
            party.Id = Guid.NewGuid();
            party.Establishment = establishment;
            party.EstablishmentId = establishment.Id;

            _partyService.Add(party);
            //Ovde treba da vrati na establishment Front page shto bi
            //bilo site zabavi shto se organizirani od nivna strana
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
