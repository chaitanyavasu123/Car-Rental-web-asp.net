using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.Models;

namespace DataAccessLayer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Notification>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            // Specify the column type for RentalPrice
            modelBuilder.Entity<Car>()
                .Property(c => c.RentalPrice)
                .HasColumnType("decimal(18, 2)"); // Adjust precision and scale as needed
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { email = "user1@gmail.com", Password = "User1" },
                new User { email="user2@gmail.com", Password="User2"}
                // Add more user data as needed
            );

            modelBuilder.Entity<Admin>().HasData(
                new Admin { email = "admin1@admin.com", Password = "Admin1" }
                // Add more admin data as needed
            );
            modelBuilder.Entity<Car>().HasData(
         new Car { Id = 1, Maker = "Toyota", Model = "Camry", RentalPrice = 50.00m, IsAvailable = true },
    new Car { Id = 2, Maker = "Honda", Model = "Civic", RentalPrice = 45.00m, IsAvailable = true },
    new Car { Id = 3, Maker = "Toyota", Model = "Camry", RentalPrice = 55.00m, IsAvailable = true },
    new Car { Id = 4, Maker = "Honda", Model = "Civic", RentalPrice = 50.00m, IsAvailable = true },
    new Car { Id = 5, Maker = "Ford", Model = "Fusion", RentalPrice = 60.00m, IsAvailable = true },
    new Car { Id = 6, Maker = "Nissan", Model = "Altima", RentalPrice = 55.00m, IsAvailable = true },
    new Car { Id = 7, Maker = "Toyota", Model = "Corolla", RentalPrice = 45.00m, IsAvailable = true },
    new Car { Id = 8, Maker = "Honda", Model = "Accord", RentalPrice = 65.00m, IsAvailable = true },
    new Car { Id = 9, Maker = "Ford", Model = "Escape", RentalPrice = 70.00m, IsAvailable = true },
    new Car { Id = 10, Maker = "Nissan", Model = "Maxima", RentalPrice = 75.00m, IsAvailable = true }
    // Add more car data as needed
    );
        }

    }
}
