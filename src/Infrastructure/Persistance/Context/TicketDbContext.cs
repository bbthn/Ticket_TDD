
using Core.Domain.Entities;
using Infrastructure.Persistance.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistance.Context
{
    public class TicketDbContext:DbContext
    {
        public const string DEFAULT_SCHEMA = "ticketing";

        public TicketDbContext(DbContextOptions dbContextOptions):base(dbContextOptions) { }
        
        DbSet<Ticket> Tickets { get; set; }
        DbSet<Flight> Flights { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if(entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Ticket>(new TicketEntityConfiguration());
            modelBuilder.ApplyConfiguration<Flight>(new FlightEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
