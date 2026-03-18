using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodApp.MVC.Data;
using FoodApp.MVC.Models;

namespace FoodApp.MVC.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly AppDbContext _context;

        public CartsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ SHOW ONLY LOGGED-IN USER CART
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var carts = _context.Carts
                .Where(c => c.CustomerId == userId)
                .Include(c => c.Food);

            return View(await carts.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cart == null) return NotFound();

            return View(cart);
        }

        // ✅ LOAD CATEGORY DROPDOWN
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // ✅ SAVE CART FOR LOGGED-IN USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoodId,Qty,Price,TotalAmount")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                cart.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(cart);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View(cart);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoodId,Qty,Price,TotalAmount")] Cart cart)
        {
            if (id != cart.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    cart.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View(cart);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cart == null) return NotFound();

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart != null)
                _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        // ✅ CATEGORY → FOOD (AJAX)
        public JsonResult FetchFoods(int cid)
        {
            var foods = _context.Foods
                .Where(f => f.CategoryId == cid)
                .Select(f => new
                {
                    id = f.Id,
                    text = f.Name
                })
                .ToList();

            return Json(foods);
        }

        // ✅ FOOD → PRICE (AJAX)
        public JsonResult GetFoodPrice(int foodId)
        {
            var food = _context.Foods.Find(foodId);
            return Json(food?.Price);
        }
    }
}