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
        List<Ticket> GetAllByUserId(string userId);
        Ticket? GetById(Guid Id);
        Ticket DeleteById(Guid Id);
        Ticket BuyTicket(Ticket ticket);
    }
}
