using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
	{
		
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Stalive",
                    Rating = 4.5,
                    Address = "Lahore",
                    CountryId = 2,
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Shangai",
                    Rating = 4.5,
                    Address = "New York",
                    CountryId = 1,
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Polo",
                    Rating = 4.5,
                    Address = "Iceland",
                    CountryId = 3,
                });
        }
    }
}

