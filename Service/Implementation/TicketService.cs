using Domain.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _UserManager;
        public TicketService(IRepository<Ticket> Repository, IRepository<Party> PartyRepository, UserManager<AppUser> userManager)
        {
            _Repository = Repository;
            _PartyRepository = PartyRepository;
            _UserManager = userManager;

        }
        public List<Ticket> GetAll()
        {
            return _Repository.GetAll(selector: x => x).ToList();
        }

        public Ticket? GetById(Guid Id)
        {
            return _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
        }

        public Ticket DeleteById(Guid Id)
        {
            var ticket = _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
            return _Repository.Delete(ticket);
        }

        public async Task<Ticket> BuyTicket(int Price, string UserId, Guid PartyId)
        {
            var party = _PartyRepository.Get(selector: x => x, predicate: x => x.Id == PartyId);
            var user = await _UserManager.FindByIdAsync(UserId);
            //Chatgpt says user should have await and BuyTicket should be async
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Party = party,
                User = user,
                PartyId = PartyId,
                UserId = UserId,
                Price = Price
            };
            return _Repository.Insert(ticket);
        }
    }
}
