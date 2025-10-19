using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Service.Interface;

namespace Web.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly IEstablishmentService _establishmentService;

        public PartyController(IPartyService partyService, IEstablishmentService establishmentService)
        {
            _partyService = partyService;
            _establishmentService = establishmentService;
        }

        // GET: Party
        public IActionResult Index()
        {
            var list = _partyService.GetAll();
            return View(list);
        }

        // GET: Party/Details/5
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
        public IActionResult Create()
        {
            PopulateEstablishmentsSelect();
            return View();
        }

        // POST: Party/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,StartTime,EndTime,Description,PictureURL,EstablishmentId,Id")] Party party)
        {
            if (!ModelState.IsValid)
            {
                PopulateEstablishmentsSelect(party.EstablishmentId);
                return View(party);
            }

            if (party.Id == Guid.Empty) party.Id = Guid.NewGuid();

            _partyService.Add(party);
            return RedirectToAction(nameof(Index));
        }

        // GET: Party/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var party = _partyService.GetById(id.Value);
            if (party == null) return NotFound();

            PopulateEstablishmentsSelect(party.EstablishmentId);
            return View(party);
        }

        // POST: Party/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,StartTime,EndTime,Description,PictureURL,EstablishmentId,Id")] Party party)
        {
            if (id != party.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateEstablishmentsSelect(party.EstablishmentId);
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
        public IActionResult DeleteConfirmed(Guid id)
        {
            _partyService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(Guid id)
        {
            return _partyService.GetById(id) != null;
        }

        private void PopulateEstablishmentsSelect(Guid? selectedId = null)
        {
            var list = _establishmentService.GetAll()
                .Select(e => new { e.Id, Display = (e.Name ?? e.Address) })
                .ToList();
            ViewData["EstablishmentId"] = new SelectList(list, "Id", "Display", selectedId);
        }
    }
}
