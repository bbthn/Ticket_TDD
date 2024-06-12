using Core.Domain.Entities;
using Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistance.EntityConfigurations
{
    public class FlightEntityConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.ToTable("Flights", TicketDbContext.DEFAULT_SCHEMA);

            builder.HasKey(f => f.Id);

            builder.HasMany(f => f.Tickets)
                .WithOne(f => f.Flight)
                .HasForeignKey(t => t.FlightId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(f => f.Departure).IsRequired();

            builder.Property(f => f.Destination).IsRequired();


        }
    }
}
