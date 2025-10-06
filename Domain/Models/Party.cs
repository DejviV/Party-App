using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Party : BaseEntity
    {
        public string Name {  get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
        public string? Description { get; set; }
        public string? PictureURL { get; set; }
        public Guid EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

    }
}
