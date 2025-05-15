using AutoMapper;
using HHotelListing.API.Core.Models;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.NET6.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countrtyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountryRepository countrtyRepository, IMapper mapper, ILogger<CountriesController> logger)
        {
            _countrtyRepository = countrtyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countrtyRepository.GetAllAsync<GetCountryDto>();
            return Ok(countries);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCounteriesResult = await _countrtyRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCounteriesResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countrtyRepository.GetDetails(id);
            return Ok(country);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountryDto)
        {

            var country = await _countrtyRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
                return BadRequest();

            try
            {
                await _countrtyRepository.UpdateAsync(id, updateCountry);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await CountryExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
        
            await _countrtyRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExist(int id)
        {
            return await _countrtyRepository.ExistAsync(id);
        }
    }

}
