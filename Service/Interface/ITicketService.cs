using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAll();
        Ticket? GetById(Guid Id);
        Ticket DeleteById(Guid Id);
        Task<Ticket> BuyTicket(int Price, string userId, Guid partyId);
    }
}
