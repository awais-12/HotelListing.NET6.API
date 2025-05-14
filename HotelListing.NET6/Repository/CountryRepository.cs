using System;
using HotelListing.NET6.Contracts;
using HotelListing.NET6.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.NET6.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _context;

        public CountryRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(x => x.Hotels).FirstOrDefaultAsync(y => y.Id == id);
        }
    }
}

