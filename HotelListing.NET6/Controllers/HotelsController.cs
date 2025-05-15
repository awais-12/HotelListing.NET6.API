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
            var hotels = await _hotelRepository.GetAllAsync<HotelDto>();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync<HotelDto>(id);
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult<HotelDto>> Posthotel(CreateHotelDto createHotelDto)
        {

            var hotel= await _hotelRepository.AddAsync<CreateHotelDto, HotelDto>(createHotelDto);
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Puthotel(int id, HotelDto hotelDto)
        {
            
            try
            {
                await _hotelRepository.UpdateAsync(id, hotelDto);
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
            await _hotelRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> hotelExist(int id)
        {
            return await _hotelRepository.ExistAsync(id);
        }
    }
}
