using System;
using HotelListing.NET6.Data;

namespace HotelListing.NET6.Contracts
{
    public interface ICountryRepository: IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id); 
    }
}

