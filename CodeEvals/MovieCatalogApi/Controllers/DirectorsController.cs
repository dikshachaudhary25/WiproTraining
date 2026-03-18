using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Data;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DirectorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
        {
            return await _context.Directors.ToListAsync();
        }

        // GET: directors/{id}/movies
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByDirector(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null)
                return NotFound();

            return director.Movies;
        }
    }
}