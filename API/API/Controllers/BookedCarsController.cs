using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedCarsController : ControllerBase
    {
        private readonly APIContext _context;

        public BookedCarsController(APIContext context)
        {
            _context = context;
        }

        // GET: api/BookedCars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookedCars>>> GetBookedCars()
        {
            return await _context.BookedCars.ToListAsync();
        }

        // GET: api/BookedCars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookedCars>> GetBookedCars(decimal id)
        {
            var bookedCars = await _context.BookedCars.FindAsync(id);

            if (bookedCars == null)
            {
                return NotFound();
            }

            return bookedCars;
        }

        // GET: api/BookedCars
        [HttpPost("")]
        public async Task<IActionResult> PostBookedCars(BookedCars car)
        {
            _context.BookedCars.Add(car);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // PUT: api/BookedCars
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("")]
        public async Task<IActionResult> PutBookedCars(BookedCars bookedCars)
        {
            if (!BookedCarsExists(bookedCars.CarID))
            {
                return BadRequest("Invalid car!");
            }
            bookedCars.Car = _context.Car.Find(bookedCars.CarID);
            _context.BookedCars.Add(bookedCars);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool BookedCarsExists(decimal id)
        {
            return _context.BookedCars.Any(e => e.ID == id);
        }
    }
}
