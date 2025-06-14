﻿namespace HotelListing.API.Core.Models.Hotel
{
    public abstract class BaseHotelDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }
    }
}

