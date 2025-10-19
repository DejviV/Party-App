using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPartyService
    {
        List<Party> GetAll();
        Party? GetById(Guid Id);
        Party Update(Party party);
        Party DeleteById(Guid Id);
        Party Add(Party party);
    }

}
