using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendee : BaseEntity
    {
        public string Name { get; set; }
        public int Age {  get; set; }
        public List<Ticket>? Tickets { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
