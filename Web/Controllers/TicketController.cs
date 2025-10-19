using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;
using Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IPartyService _partyService;
        private readonly UserManager<AppUser> _userManager;

        public TicketController(ITicketService ticketService, IPartyService partyService, UserManager<AppUser> userManager)
        {
            _ticketService = ticketService;
            _partyService = partyService;
            _userManager = userManager;
        }

        // GET: Ticket
        public IActionResult Index()
        {
            var list = _ticketService.GetAll();
            return View(list);
        }

        // GET: Ticket/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var ticket = _ticketService.GetById(id.Value);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // GET: Ticket/Create
        //Used to be here XD

        
        // Where Edit used to be
        //Other edit too
        

        // GET: Ticket/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var ticket = _ticketService.GetById(id.Value);
            if (ticket == null) return NotFound();

            if (ticket.Party == null && ticket.PartyId != Guid.Empty)
            {
                ticket.Party = _partyService.GetById(ticket.PartyId);
            }

            if (ticket.User == null && !string.IsNullOrEmpty(ticket.UserId))
            {
                ticket.User = _userManager.FindByIdAsync(ticket.UserId).GetAwaiter().GetResult();
            }

            return View(ticket);
        }


        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _ticketService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _ticketService.GetById(id) != null;
        }

        
        // TODO: make a buy controller
    }
}
