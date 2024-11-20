using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PublicTransportNavigator.Migrations;
using PublicTransportNavigator.Models;

//Add - Migration InitialCreate
//Update-Database


namespace PublicTransportNavigator
{
    public class PublicTransportNavigatorContext(DbContextOptions<PublicTransportNavigatorContext> options) : DbContext(options)
    {
        public DbSet<Bus> Buses { get; set; } 
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<BusSeat> BusSeats { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<Seat> SeatTypes { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFavouriteBusStop> UserFavouriteBusStops { get; set; }
        public DbSet<UserTravel> UserTravels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.TravelHistory)
                .WithOne(ut => ut.User);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
            modelBuilder.Entity<User>()
                .HasMany(u => u.Discounts)
                .WithMany(d => d.DiscountUsers)
                .UsingEntity<Dictionary<string, object>>("user_discounts",
                    j => j.HasOne<Discount>()
                        .WithMany()
                        .HasForeignKey("discount_id"),
                    j => j.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id"));

            modelBuilder.Entity<Bus>()
                .HasOne(b => b.FirstBusStop)
                .WithMany()
                .HasForeignKey(b => b.FirstBusStopId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.LastBusStop)
                .WithMany()
                .HasForeignKey(b => b.LastBusStopId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.Type)
                .WithMany()
                .HasForeignKey(b => b.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Timetable>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Timetable>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Timetables)
                .HasForeignKey(t => t.BusId);
            modelBuilder.Entity<Timetable>()
                .HasOne(t => t.BusStop)
                .WithMany(bs => bs.Timetables)
                .HasForeignKey(t => t.BusStopId);

            modelBuilder.Entity<UserFavouriteBusStop>()
                .HasKey(f =>  f.Id);
            modelBuilder.Entity<UserFavouriteBusStop>()
                .HasOne(f => f.BusStop)
                .WithMany()
                .HasForeignKey(f => f.BusStopId);
            modelBuilder.Entity<UserFavouriteBusStop>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<BusSeat>()
                .HasKey(bs => bs.Id);
            modelBuilder.Entity<BusSeat>()
                .HasOne(bs => bs.Bus)
                .WithMany(b => b.BusSeats)
                .HasForeignKey(bs => bs.BusId);
            modelBuilder.Entity<BusSeat>()
                .HasOne(bs => bs.SeatType)
                .WithMany()
                .HasForeignKey(bs => bs.SeatTypeId);

            modelBuilder.Entity<UserTravel>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<UserTravel>()
                .HasOne(h => h.TicketType)
                .WithMany()
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserTravel>()
                .HasOne(h => h.User)
                .WithMany(u => u.TravelHistory)
                .HasForeignKey(h => h.UserId);
            modelBuilder.Entity<UserTravel>()
                .HasMany(ut => ut.ReservedSeats)
                .WithOne(rs => rs.UserTravel);

            modelBuilder.Entity<ReservedSeat>()
                .HasKey(rs => rs.Id);
            modelBuilder.Entity<ReservedSeat>()
                .HasOne(rs => rs.BusSeat)
                .WithMany()
                .HasForeignKey(rs => rs.BusSeatId);
            modelBuilder.Entity<ReservedSeat>()
                .HasOne(rs => rs.TimeIn)
                .WithMany()
                .HasForeignKey(rs => rs.TimeInId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ReservedSeat>()
                .HasOne(rs => rs.TimeOff)
                .WithMany()
                .HasForeignKey(rs => rs.TimeOffId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ReservedSeat>()
                .HasOne(rs => rs.UserTravel)
                .WithMany(ut => ut.ReservedSeats)
                .HasForeignKey(rs => rs.UserTravelId);

            modelBuilder.Entity<Seat>()
                .Property(s => s.SeatType)
                .HasConversion<string>();

            modelBuilder.Entity<BusType>()
                .Property(bt => bt.Type)
                .HasConversion<string>();

        }
        public DbSet<PublicTransportNavigator.Models.BusType> BusType { get; set; } = default!;
    }
}
