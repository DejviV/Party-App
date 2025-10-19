using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Party : BaseEntity
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        public DateTime StartTime {  get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string? Description { get; set; }
        public string? PictureURL { get; set; }
        [Required]
        public Guid EstablishmentId { get; set; }
        [Required]
        public Establishment Establishment { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [Required]
        public int Capacity { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
