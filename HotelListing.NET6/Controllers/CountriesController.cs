using AutoMapper;
using HotelListing.NET6.Data;
using HotelListing.NET6.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.NET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _context.Countries.ToListAsync();
            return Ok(_mapper.Map<List<GetCountryDto>>(countries));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _context.Countries.Include(x=>x.Hotels).FirstOrDefaultAsync(y=>y.Id==id);
            if (country == null)
                return NotFound();

            return Ok(_mapper.Map<CountryDto>(country));
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {

            var country = _mapper.Map<Country>(createCountryDto);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
                return BadRequest();

            //_context.Entry(country).State = EntityState.Modified;
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound();

            _mapper.Map(updateCountry, country);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CountryExist(id))
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
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound();

            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CountryExist(int id)
        {
            return _context.Countries.Any(x => x.Id.Equals(id));
        }
    }

}
