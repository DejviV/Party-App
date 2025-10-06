using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket : BaseEntity
    {
        public int Price { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid PartyId { get; set; }
        public Party Party { get; set; }
      
    }
}
