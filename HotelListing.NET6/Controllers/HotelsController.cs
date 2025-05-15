using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.NET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);
            if (hotel == null)
                return NotFound();

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> Posthotel(CreateHotelDto createHotelDto)
        {

            var hotel = _mapper.Map<Hotel>(createHotelDto);
            await _hotelRepository.AddAsync(hotel);
            return CreatedAtAction("Gethotel", new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Puthotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
                return BadRequest();

            //_context.Entry(hotel).State = EntityState.Modified;
            var hotel = await _hotelRepository.GetAsync(id);
            if (hotel == null)
                return NotFound();

            _mapper.Map(hotelDto, hotel);

            try
            {
                await _hotelRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await hotelExist(id))
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
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id <= 0)
                return BadRequest();

            await _hotelRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> hotelExist(int id)
        {
            return await _hotelRepository.ExistAsync(id);
        }
    }
}
