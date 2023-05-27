using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HillYatraAPI.Models;

namespace HillYatraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public PlacesController(RepositoryContext context)
        {
            _context = context;
        }

        // GET: api/Places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Places>>> GetPlaces()
        {
            return await _context.Places.ToListAsync();
        }

        // GET: api/Places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Places>> GetPlaces(int id)
        {
            var places = await _context.Places.FindAsync(id);

            if (places == null)
            {
                return NotFound();
            }

            return places;
        }
        // GET: api/Places/5
        [HttpGet("GetPlacesByType")]
        public async Task<ActionResult<Places>> GetPlacesByType([FromQuery]  int? type=null)
        {
            var places = await _context.Places.Where(a=>a.Type==type).ToListAsync();

            if (places == null)
            {
                return NotFound();
            }

            return Ok(places);
        }

        // PUT: api/Places/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaces(int id, Places places)
        {
            if (id != places.Id)
            {
                return BadRequest();
            }
            var place = _context.Places
                .FirstOrDefault(s => s.Id.Equals(id));
            place.Place = places.Place;
            place.Type = places.Type;
            place.UpdatedOn = DateTime.Now;
           
            //_context.Entry(places).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlacesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetPlaces", new { id = places.Id }, place);
            //return Ok(places);
        }

        // POST: api/Places
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Places>> PostPlaces(Places places)
        {
            _context.Places.Add(places);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaces", new { id = places.Id }, places);
        }

        // DELETE: api/Places/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Places>> DeletePlaces(int id)
        {
            var places = await _context.Places.FindAsync(id);
            if (places == null)
            {
                return NotFound();
            }

            _context.Places.Remove(places);
            await _context.SaveChangesAsync();

            return places;
        }

        private bool PlacesExists(int id)
        {
            return _context.Places.Any(e => e.Id == id);
        }
    }
}
