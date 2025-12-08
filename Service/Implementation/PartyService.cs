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
    public class PartyService : IPartyService
    {
        private readonly IRepository<Party> _Repository;
        private readonly IRepository<Establishment> _EstablishmentRepository;
        private readonly IEstablishmentService _EstablishmentService;


        public PartyService (IRepository<Party> Repository, IRepository<Establishment> establishmentRepository, IEstablishmentService establishmentService)
        {
            _Repository = Repository;
            _EstablishmentRepository = establishmentRepository;
            _EstablishmentService = establishmentService;
        }
        public List<Party> GetAll()
        {
            return _Repository.GetAll(selector: x=>x).ToList();
        }

        public Party? GetById(Guid Id)
        {
            return _Repository.Get(selector: x=>x, predicate:x=>x.Id == Id);
        }
        public Party Add(Party party)
        {
            party.Id = Guid.NewGuid();
            return _Repository.Insert(party);
        }

        public Party DeleteById(Guid Id)
        {
            var party = _Repository.Get(selector:x=>x, predicate: x=>x.Id == Id);
            return _Repository.Delete(party);
        }

        public Party Update(Party party)
        {
            return _Repository.Update(party);
        }
        public List<Party> GetByUserId(string userId)
        {
            var establishment = _EstablishmentService.GetByUserId(userId);
          
            return _Repository.GetAll(selector: x => x, predicate: x=>x.EstablishmentId==establishment.Id,
                include: q => q.Include(p => p.Establishment).Include(p => p.Tickets)).ToList();
        }
    }
}
