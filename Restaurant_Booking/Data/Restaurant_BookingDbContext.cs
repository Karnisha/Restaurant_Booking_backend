using Microsoft.EntityFrameworkCore;
using Restaurant_Booking.Models;
using System;
using System.Security.Policy;

namespace Restaurant_Booking.Data
{
    public class Restaurant_BookingDbContext : DbContext
    {
        public Restaurant_BookingDbContext(DbContextOptions<Restaurant_BookingDbContext> options) : base(options)
        {

        }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        public DbSet<Customer> Customer{ get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Admin> Admin { get; set; }
    }
}
