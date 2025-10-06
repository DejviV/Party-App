using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext <AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Attendee> Attenees { get; set; }
        public virtual DbSet<Establishment> Establishments { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add this line to store Role as string
            builder.Entity<AppUser>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }

    }
}
