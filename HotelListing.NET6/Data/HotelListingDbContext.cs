using System;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.NET6.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Pakistan",
                    ShortName = "PK"
                },
                new Country
                {
                    Id = 2,
                    Name = "Sudia",
                    ShortName = "SD"
                },
                 new Country
                 {
                     Id = 3,
                     Name = "India",
                     ShortName = "IND"
                 });

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Stalive",
                    Rating = 4.5,
                    Address="Lahore",
                    CountryId = 2,
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Shangai",
                    Rating = 4.5,
                    Address="New York",
                    CountryId = 1,
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Polo",
                    Rating = 4.5,
                    Address="Iceland",
                    CountryId = 3,
                } );
        }
    }

}

