using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAttendeeService
    {
        List<Attendee> GetAll();
        Attendee? GetById(Guid Id);
        Attendee Update(Attendee attendee);
        Attendee DeleteById(Guid Id);
        Attendee Add(Attendee attendee);
    }
}
