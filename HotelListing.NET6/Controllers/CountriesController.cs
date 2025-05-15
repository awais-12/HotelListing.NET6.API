using AutoMapper;
using HotelListing.NET6.Contracts;
using HotelListing.NET6.Data;
using HotelListing.NET6.Exceptions;
using HotelListing.NET6.Models;
using HotelListing.NET6.Models.Country;
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
            var countries = await _countrtyRepository.GetAllAsync();
            return Ok(_mapper.Map<List<GetCountryDto>>(countries));
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
            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id); //Global Exception
                //_logger.LogWarning($"No Found Country {nameof(GetCountry)} with id : {id}");
                //return NotFound();
            }

            return Ok(_mapper.Map<CountryDto>(country));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {

            var country = _mapper.Map<Country>(createCountryDto);
            await _countrtyRepository.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
                return BadRequest();

            //_context.Entry(country).State = EntityState.Modified;
            var country = await _countrtyRepository.GetAsync(id);
            if (country == null)
                throw new NotFoundException(nameof(PutCountry), id);

            _mapper.Map(updateCountry, country);

            try
            {
                await _countrtyRepository.UpdateAsync(country);
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
            if (id <= 0)
                return BadRequest();

            await _countrtyRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExist(int id)
        {
            return await _countrtyRepository.ExistAsync(id);
        }
    }

}
