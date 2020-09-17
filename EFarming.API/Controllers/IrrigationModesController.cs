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
    public class IrrigationModesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IrrigationModesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IrrigationModes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IrrigationMode>>> GetIrrigationMode()
        {
            return await _context.IrrigationMode.ToListAsync();
        }

        // GET: api/IrrigationModes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IrrigationMode>> GetIrrigationMode(int id)
        {
            var irrigationMode = await _context.IrrigationMode.FirstOrDefaultAsync(f => f.FarmZoneId == id);

            if (irrigationMode == null)
            {
                return NotFound();
            }

            return irrigationMode;
        }

        // PUT: api/IrrigationModes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIrrigationMode(int id, IrrigationMode irrigationMode)
        {
            if (id != irrigationMode.Id)
            {
                return BadRequest();
            }

            _context.Entry(irrigationMode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IrrigationModeExists(id))
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

        // POST: api/IrrigationModes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IrrigationMode>> PostIrrigationMode(IrrigationMode irrigationMode)
        {
            _context.IrrigationMode.Add(irrigationMode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIrrigationMode", new { id = irrigationMode.Id }, irrigationMode);
        }

        // DELETE: api/IrrigationModes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IrrigationMode>> DeleteIrrigationMode(int id)
        {
            var irrigationMode = await _context.IrrigationMode.FindAsync(id);
            if (irrigationMode == null)
            {
                return NotFound();
            }

            _context.IrrigationMode.Remove(irrigationMode);
            await _context.SaveChangesAsync();

            return irrigationMode;
        }

        private bool IrrigationModeExists(int id)
        {
            return _context.IrrigationMode.Any(e => e.Id == id);
        }
    }
}
