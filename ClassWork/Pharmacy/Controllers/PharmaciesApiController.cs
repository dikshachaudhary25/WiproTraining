using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesApiController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public PharmaciesApiController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/PharmaciesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<global::Pharmacy.Models.Pharmacy>>> GetPharmacies()
        {
            return await _context.Pharmacies.ToListAsync();
        }

        // GET: api/PharmaciesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<global::Pharmacy.Models.Pharmacy>> GetPharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null)
            {
                return NotFound();
            }

            return pharmacy;
        }

        // PUT: api/PharmaciesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPharmacy(int id, global::Pharmacy.Models.Pharmacy pharmacy)
        {
            if (id != pharmacy.Id)
            {
                return BadRequest();
            }

            _context.Entry(pharmacy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PharmacyExists(id))
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

        // POST: api/PharmaciesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<global::Pharmacy.Models.Pharmacy>> PostPharmacy(global::Pharmacy.Models.Pharmacy pharmacy)
        {
            _context.Pharmacies.Add(pharmacy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPharmacy", new { id = pharmacy.Id }, pharmacy);
        }

        // DELETE: api/PharmaciesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PharmacyExists(int id)
        {
            return _context.Pharmacies.Any(e => e.Id == id);
        }
    }
}
