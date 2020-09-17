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
    public class ActuatorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActuatorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Actuators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actuator>>> GetActuator()
        {
            return await _context.Actuator.Include(s => s.Sensors).ToListAsync();
        }

        // GET: api/Actuators/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actuator>> GetActuator(int id)
        {
            //var actuator = await _context.Actuator.FindAsync(id);
            var actuator = await _context.Actuator
                .Include(a => a.Sensors)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (actuator == null)
            {
                return NotFound();
            }

            return actuator;
        }

        // PUT: api/Actuators/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActuator(int id, Actuator actuator)
        {
            if (id != actuator.Id)
            {
                return BadRequest();
            }

            _context.Entry(actuator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActuatorExists(id))
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

        // POST: api/Actuators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Actuator>> PostActuator(Actuator actuator)
        {
            _context.Actuator.Add(actuator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActuator", new { id = actuator.Id }, actuator);
        }

        // DELETE: api/Actuators/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Actuator>> DeleteActuator(int id)
        {
            var actuator = await _context.Actuator.FindAsync(id);
            if (actuator == null)
            {
                return NotFound();
            }

            _context.Actuator.Remove(actuator);
            await _context.SaveChangesAsync();

            return actuator;
        }

        private bool ActuatorExists(int id)
        {
            return _context.Actuator.Any(e => e.Id == id);
        }
    }
}
