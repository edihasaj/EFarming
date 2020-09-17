using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFarming.API;
using EFarming.Models;

namespace EFarming.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmZonesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FarmZonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FarmZones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FarmZone>>> GetFarmZone()
        {
            return await _context.FarmZone
                .Include(f => f.Farm)
                .Include(a => a.Actuators)
                .ThenInclude(s => s.Sensors)
                .ThenInclude(sd => sd.Data)
                .ToListAsync();
        }

        // GET: api/FarmZones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FarmZone>> GetFarmZone(int id)
        {
            var farmZone = await _context.FarmZone
                .Include(a => a.Actuators)
                .ThenInclude(s => s.Sensors)
                .ThenInclude(sd => sd.Data)
                .FirstOrDefaultAsync(f => f.ZoneId == id);

            if (farmZone == null)
            {
                return NotFound();
            }

            return farmZone;
        }

        // PUT: api/FarmZones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFarmZone(int id, FarmZone farmZone)
        {
            if (id != farmZone.ZoneId)
            {
                return BadRequest();
            }

            _context.Entry(farmZone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FarmZoneExists(id))
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

        // POST: api/FarmZones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FarmZone>> PostFarmZone(FarmZone farmZone)
        {
            _context.FarmZone.Add(farmZone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFarmZone", new { id = farmZone.ZoneId }, farmZone);
        }

        // DELETE: api/FarmZones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FarmZone>> DeleteFarmZone(int id)
        {
            var farmZone = await _context.FarmZone.FindAsync(id);
            if (farmZone == null)
            {
                return NotFound();
            }

            _context.FarmZone.Remove(farmZone);
            await _context.SaveChangesAsync();

            return farmZone;
        }

        private bool FarmZoneExists(int id)
        {
            return _context.FarmZone.Any(e => e.ZoneId == id);
        }
    }
}
