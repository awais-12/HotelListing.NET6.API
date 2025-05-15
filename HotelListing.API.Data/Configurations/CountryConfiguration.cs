using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
	{
		
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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

        }
    }
}

