using FilmManagementSystem.Models;
using FilmManagementSystem.Schemas;
using FilmManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly FilmService _filmService;

        public FilmsController(FilmService filmService)
        {
            _filmService = filmService;
        }

        /// <summary>
        /// Get a filterable, sortable and searchable list of films.
        /// </summary>
        /// <param name="genre">Film genre (optional).</param>
        /// <param name="search">Search query by film title (optional).</param>
        /// <param name="sortBy">Field for sorting (default ‘title’).</param>
        /// <param name="sortOrder">Sort order: ‘asc’ or ‘desc’ (default is ‘asc’).</param>
        /// <returns>List of films.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms(
            [FromQuery] string? genre = null,
            [FromQuery] string? search = null,
            [FromQuery] string sortBy = "title",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var films = await _filmService.GetFilms(genre, search, sortBy, sortOrder);
            return Ok(films);
        }

        /// <summary>
        /// Get detailed information about a film by ID.
        /// </summary>
        /// <param name="id">Film ID.</param>
        /// <returns>Film Details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _filmService.GetFilmById(id);
            if (film == null)
            {
                return NotFound();
            }

            return Ok(film);
        }

        /// <summary>
        /// Add a new film to the system.
        /// </summary>
        /// <param name="filmSchema">The film object to add.</param>
        /// <returns>Added film.</returns>
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(FilmSchema filmSchema)
        {
            var createdFilm = await _filmService.AddFilm(filmSchema);
            return CreatedAtAction(nameof(GetFilm), new { id = createdFilm.Id }, createdFilm);
        }

        /// <summary>
        /// Update film information by ID.
        /// </summary>
        /// <param name="id">Film ID to update.</param>
        /// <param name="filmSchema"></param>
        /// <returns>Status code response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, FilmSchema filmSchema)
        {
            var success = await _filmService.UpdateFilm(id, filmSchema);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete film by ID.
        /// </summary>
        /// <param name="id">Identifier of the film to delete.</param>
        /// <returns>Response with status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var success = await _filmService.DeleteFilm(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
