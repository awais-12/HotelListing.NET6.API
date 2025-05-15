using System;
using HotelListing.API.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : IdentityDbContext<ApiUser>
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
                
        }
        
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.ApplyConfiguration(new RoleConfiguration());

        //    modelBuilder.Entity<Country>().HasData(
        //        new Country
        //        {
        //            Id = 1,
        //            Name = "Pakistan",
        //            ShortName = "PK"
        //        },
        //        new Country
        //        {
        //            Id = 2,
        //            Name = "Sudia",
        //            ShortName = "SD"
        //        },
        //         new Country
        //         {
        //             Id = 3,
        //             Name = "India",
        //             ShortName = "IND"
        //         });

        //    modelBuilder.Entity<Hotel>().HasData(
        //        new Hotel
        //        {
        //            Id = 1,
        //            Name = "Stalive",
        //            Rating = 4.5,
        //            Address = "Lahore",
        //            CountryId = 2,
        //        },
        //        new Hotel
        //        {
        //            Id = 2,
        //            Name = "Shangai",
        //            Rating = 4.5,
        //            Address = "New York",
        //            CountryId = 1,
        //        },
        //        new Hotel
        //        {
        //            Id = 3,
        //            Name = "Polo",
        //            Rating = 4.5,
        //            Address = "Iceland",
        //            CountryId = 3,
        //        });
        //}
    }
    public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
    {
        public HotelListingDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelListingDbContext>();
            var conn = config.GetConnectionString("HotelListingDbConnection");///ConnectionString

            optionsBuilder.UseNpgsql(conn); // PostgreSQL here

            return new HotelListingDbContext(optionsBuilder.Options);
        }
    }
}

