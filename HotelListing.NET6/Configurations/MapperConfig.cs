using System;
using AutoMapper;
using HotelListing.NET6.Data;
using HotelListing.NET6.Models.Country;
using HotelListing.NET6.Models.Hotel;
using HotelListing.NET6.Models.User;

namespace HotelListing.NET6.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        }
    }
}

