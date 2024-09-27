using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Models
{
    public class AirLineDBcontext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=AirLineDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Flight-Booking relationship(One-to-Many)
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Bookings)
                .WithOne(b => b.Flight)
                .HasForeignKey(b => b.FlightId);

            // User-Booking relationship(One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.UserId);

            // Booking-Passenger relationship(One-to-Many)
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Passengers)
                .WithOne(p => p.Booking)
                .HasForeignKey(p => p.BookingId);

            // Booking-Payment relationship(One-to-One)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }
    
    
}
