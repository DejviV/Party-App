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

            builder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany()           // AppUser does not necessarily have a collection of Attendees
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Establishment>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Party -> Establishment: keep cascade (delete party deletes its tickets)
            builder.Entity<Party>()
                .HasOne(p => p.Establishment)
                .WithMany(e => e.Parties)
                .HasForeignKey(p => p.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket -> Party: cascade when party deleted
            builder.Entity<Ticket>()
                .HasOne(t => t.Party)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
        
        }

    }
}
