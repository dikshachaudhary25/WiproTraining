using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.API.Data;
using FoodDeliveryApp.API.Models;

namespace FoodDeliveryApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsSoldController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsSoldController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET: api/ProductsSold
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductsSold>>> GetAll()
    {
        return await _context.ProductsSold
            .Include(p => p.Food)
            .Include(p => p.Sale)
            .ToListAsync();
    }

    // ✅ GET: api/ProductsSold/1/5
    [HttpGet("{productId}/{saleId}")]
    public async Task<ActionResult<ProductsSold>> GetById(int productId, int saleId)
    {
        var item = await _context.ProductsSold
            .FindAsync(productId, saleId);

        if (item == null)
            return NotFound();

        return item;
    }

    // ✅ POST: api/ProductsSold
    [HttpPost]
    public async Task<ActionResult<ProductsSold>> Create(ProductsSold model)
    {
        _context.ProductsSold.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { productId = model.ProductId, saleId = model.SaleId },
            model);
    }

    // ✅ PUT: api/ProductsSold/1/5
    [HttpPut("{productId}/{saleId}")]
    public async Task<IActionResult> Update(int productId, int saleId, ProductsSold model)
    {
        if (productId != model.ProductId || saleId != model.SaleId)
            return BadRequest();

        _context.Entry(model).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ProductsSold.Any(e =>
                e.ProductId == productId && e.SaleId == saleId))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // ✅ DELETE: api/ProductsSold/1/5
    [HttpDelete("{productId}/{saleId}")]
    public async Task<IActionResult> Delete(int productId, int saleId)
    {
        var item = await _context.ProductsSold.FindAsync(productId, saleId);

        if (item == null)
            return NotFound();

        _context.ProductsSold.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}