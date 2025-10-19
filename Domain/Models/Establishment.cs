using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Establishment : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string? PictureURL { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<Party> Parties { get; set; }
    }
}
