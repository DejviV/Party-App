using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        [NotMapped]
        public Role Role { get; set; }
    }
}
 