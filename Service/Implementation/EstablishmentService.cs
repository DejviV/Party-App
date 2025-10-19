using Domain.Models;
using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IRepository<Establishment> _Repository;
        public EstablishmentService(IRepository<Establishment> repository)
        {
            _Repository = repository;
        }
        public List<Establishment> GetAll()
        {
            return _Repository.GetAll(selector: x => x).ToList();
        }

        public Establishment? GetById(Guid Id)
        {
            return _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
        }
        public Establishment Add(Establishment establishment)
        {
            establishment.Id = Guid.NewGuid();
            return _Repository.Insert(establishment);
        }

        public Establishment DeleteById(Guid Id)
        {
            var establishment = _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
            return _Repository.Delete(establishment);
        }

        public Establishment Update(Establishment establishment)
        {
            return _Repository.Update(establishment);
        }
    }
}
