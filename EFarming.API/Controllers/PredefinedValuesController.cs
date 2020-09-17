using EFarming.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFarming.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredefinedValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PredefinedValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PredefinedValues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredefinedValues>>> GetPredefinedValues()
        {
            return await _context.PredefinedValues.ToListAsync();
        }

        // GET: api/PredefinedValues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PredefinedValues>> GetPredefinedValues(int id)
        {
            var predefinedValues = await _context.PredefinedValues.FindAsync(id);

            if (predefinedValues == null)
            {
                return NotFound();
            }

            return predefinedValues;
        }

        // PUT: api/PredefinedValues/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPredefinedValues(int id, PredefinedValues predefinedValues)
        {
            if (id != predefinedValues.Id)
            {
                return BadRequest();
            }

            _context.Entry(predefinedValues).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredefinedValuesExists(id))
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

        // POST: api/PredefinedValues
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PredefinedValues>> PostPredefinedValues(PredefinedValues predefinedValues)
        {
            _context.PredefinedValues.Add(predefinedValues);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPredefinedValues", new { id = predefinedValues.Id }, predefinedValues);
        }

        // DELETE: api/PredefinedValues/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PredefinedValues>> DeletePredefinedValues(int id)
        {
            var predefinedValues = await _context.PredefinedValues.FindAsync(id);
            if (predefinedValues == null)
            {
                return NotFound();
            }

            _context.PredefinedValues.Remove(predefinedValues);
            await _context.SaveChangesAsync();

            return predefinedValues;
        }

        private bool PredefinedValuesExists(int id)
        {
            return _context.PredefinedValues.Any(e => e.Id == id);
        }
    }
}
