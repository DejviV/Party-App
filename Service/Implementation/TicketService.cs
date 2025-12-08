using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _Repository;
        private readonly IRepository<Party> _PartyRepository;
        public TicketService(IRepository<Ticket> Repository, IRepository<Party> PartyRepository)
        {
            _Repository = Repository;
            _PartyRepository = PartyRepository;
        }
        public List<Ticket> GetAll()
        {
            return _Repository.GetAll(selector: x => x).ToList();
        }

        public Ticket? GetById(Guid Id)
        {
            return _Repository.Get(selector: x => x, predicate: x => x.Id == Id, include: y => y.Include(x => x.Party));
        }

        public Ticket DeleteById(Guid Id)
        {
            var ticket = _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
            return _Repository.Delete(ticket);
        }

        public Ticket BuyTicket(Ticket ticket)
        {
          
            // load party with tickets and establishment included
            var party = _PartyRepository.Get(
                selector: x => x, predicate: x => x.Id == ticket.PartyId,
                include: q => q.Include(p => p.Tickets).Include(p => p.Establishment)
            );

            if (party == null)
                throw new Exception("Party not found.");

            var ticketsCount = party.Tickets?.Count ?? 0;

            if (ticketsCount >= party.Capacity)
                throw new Exception("No more tickets available!");

            if (ticket.Price == 0)
                ticket.Price = party.TicketPrice;

            return _Repository.Insert(ticket);
        }
        
        public List<Ticket> GetAllByUserId(string userId)
        {
            return _Repository.GetAll(selector: x => x, predicate: x => x.UserId == userId).ToList();
        }
    }
}
