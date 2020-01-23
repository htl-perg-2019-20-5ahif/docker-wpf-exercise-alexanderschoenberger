using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly APIContext _context;

        public CarsController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCar()
        {
            return await _context.Car.ToListAsync();
        }

        // GET: api/Cars/free/5684684684
        [HttpGet("free/{dateTime}")]
        public ActionResult<IEnumerable<Car>> GetFreeCars(long dateTime)
        {
            var bookedCars = _context.BookedCars.ToArray();
            List<Car> bookedCar = new List<Car>();
            for (int i = 0; i < bookedCars.Length; i++)
            {
                var book = bookedCars[i];
                if (book.Date.Value.Ticks == dateTime)
                {
                    bookedCar.Add(_context.Car.Find(book.CarID));
                }
            }
            if (bookedCar.Count != 0)
            {
                var ret = new List<Car>();
                var cars = _context.Car.ToArray();
                for (int i = 0; i < cars.Length; i++)
                {
                    if (!bookedCar.Contains(cars[i]))
                    {
                        ret.Add(cars[i]);
                    }
                }
                return Ok(ret);
            }
            return Ok(_context.Car);
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(decimal id)
        {
            var car = await _context.Car.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(decimal id, Car car)
        {
            if (id != car.ID)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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

        // POST: api/Cars
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Car.Add(car);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarExists(car.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCar", new { id = car.ID }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(decimal id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Car.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        private bool CarExists(decimal id)
        {
            return _context.Car.Any(e => e.ID == id);
        }
    }
}
