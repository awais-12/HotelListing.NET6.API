using System;
using HotelListing.NET6.Contracts;
using HotelListing.NET6.Data;

namespace HotelListing.NET6.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(HotelListingDbContext context) : base(context)
        {
        }
    }
}

