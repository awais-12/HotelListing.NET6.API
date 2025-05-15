using System;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Contracts
{
    public interface ICountryRepository: IGenericRepository<Country>
    {
        Task<CountryDto> GetDetails(int id); 
    }
}

