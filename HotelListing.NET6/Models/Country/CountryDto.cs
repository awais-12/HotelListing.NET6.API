using System;
using HotelListing.NET6.Models.Hotel;

namespace HotelListing.NET6.Models.Country
{
	public class CountryDto: BaseCountryDto
    {
        public int Id { get; set; }
        public List<HotelDto> Hotels { get; set; }
    }
}

