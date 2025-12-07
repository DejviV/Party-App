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
            var party = _PartyRepository.Get(selector: x=>x, predicate: x=>x.Id==ticket.PartyId);

            //if (party.TicketsSold >= party.Establishment.Capacity)
            //Breaking news, you dont know how to do async
            if (party.Tickets.Count >= party.Establishment.Capacity)
                throw new Exception("No more tickets available!");

            return _Repository.Insert(ticket);
        }

        public List<Ticket> GetAllByUserId(string userId)
        {
            return _Repository.GetAll(selector: x => x, predicate: x => x.UserId == userId).ToList();
        }
    }
}
