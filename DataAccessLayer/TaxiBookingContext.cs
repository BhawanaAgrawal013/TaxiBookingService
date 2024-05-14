namespace DataAccessLayer
{
    public partial class TaxiBookingContext : DbContext
    {
        public TaxiBookingContext(DbContextOptions<TaxiBookingContext> options) : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<VehicleDetail> VehicleDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<BookingStatus> BookingStatuses { get; set; }
        public virtual DbSet<PaymentMode> PaymentModes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<CancellationReason> CancellationReasons { get; set; }
        public virtual DbSet<CancelledBooking> CancelledBookings { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(
                entity =>
                {
                    entity.HasOne(e => e.State).WithMany(e => e.Cities).IsRequired();
                });

            modelBuilder.Entity<Area>(entity => {entity.HasOne(e => e.City).WithMany(e => e.Areas).IsRequired(); });

            modelBuilder.Entity<Location>().HasOne(e => e.Area).WithMany(e => e.Locations).IsRequired();

            modelBuilder.Entity<VehicleDetail>().HasOne(e => e.VehicleType).WithMany(e => e.VehicleDetails).IsRequired();

            modelBuilder.Entity<Driver>(
                e =>
                {
                    e.HasOne(e => e.VehicleDetail).WithOne(e => e.Driver).HasForeignKey<Driver>(e => e.VehicleId);
                    e.HasOne(e => e.User).WithOne(e => e.Driver).HasForeignKey<Driver>(e => e.UserId).IsRequired();
                    e.HasOne(e => e.Location).WithOne(e => e.Driver).HasForeignKey<Driver>(e => e.LocationId).OnDelete(DeleteBehavior.Cascade);
                    e.HasOne(e => e.Created).WithOne(e => e.CreatedDriver).HasForeignKey<Driver>(e => e.CreatedBy);
                });

            modelBuilder.Entity<User>(
                e =>
                {
                    e.HasOne(e => e.Role).WithMany(e => e.Users).IsRequired();
                });

            modelBuilder.Entity<Booking>(
                e =>
                {
                    e.HasOne(e => e.User).WithMany(e => e.Bookings).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(e =>e.Driver).WithMany(e =>e.Bookings).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(e => e.Payment).WithOne(e => e.Booking).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(e => e.Rating).WithOne(e => e.Booking).HasForeignKey<Booking>(e => e.RatingId).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(e => e.Created).WithOne(e => e.Booking).HasForeignKey<Booking>(e => e.CreatedBy);
                    e.HasIndex(e => e.CreatedBy).IsUnique(false);
                });

            modelBuilder.Entity<Rating>(
                e =>
                {
                    e.HasOne(e => e.Driver).WithOne(e => e.Ratings).IsRequired();
                    e.HasOne(e => e.User).WithMany(e => e.Ratings);
                    e.HasOne(e => e.Created).WithOne(e => e.CreatedRating).HasForeignKey<Rating>(e => e.CreatedBy);
                    e.HasIndex(e => e.CreatedBy).IsUnique(false);
                    e.HasIndex(e => e.DriverId).IsUnique(false);
                });

            modelBuilder.Entity<BookingStatus>(
                e =>
                {
                    e.Property(e => e.Status);
                    e.Property(e => e.Id);
                });

            modelBuilder.Entity<PaymentMode>(
                e =>
                {
                    e.Property(e => e.Id);
                    e.Property(e => e.Mode);
                });

            modelBuilder.Entity<CancellationReason>(e =>
            {
                e.Property(e => e.id);
                e.Property(e => e.isCharged);
                e.Property(e => e.Reason);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
