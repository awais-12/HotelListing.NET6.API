using System;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Contracts
{
    public interface ICountryRepository: IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id); 
    }
}

