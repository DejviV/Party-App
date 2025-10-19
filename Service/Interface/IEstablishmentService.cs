using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IEstablishmentService
    {
        List<Establishment> GetAll();
        Establishment? GetById(Guid Id);
        Establishment Update(Establishment establishment);
        Establishment DeleteById(Guid Id);
        Establishment Add(Establishment establishment);
    }
}
