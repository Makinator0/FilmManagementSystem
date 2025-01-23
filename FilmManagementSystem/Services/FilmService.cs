using FilmManagementSystem.Data;
using FilmManagementSystem.Models;
using FilmManagementSystem.Schemas;
using Microsoft.EntityFrameworkCore;

namespace FilmManagementSystem.Services;

public class FilmService
{
    private readonly FilmContext _context;

    public FilmService(FilmContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetFilms(string? genre, string? search, string sortBy, string sortOrder)
    {
        var query = _context.Films.AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(f => f.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(f =>
                f.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                f.Director.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (sortBy.Equals("rating", StringComparison.OrdinalIgnoreCase))
        {
            query = sortOrder == "asc" ? query.OrderBy(f => f.Rating) : query.OrderByDescending(f => f.Rating);

        }
        else if (sortBy.Equals("releaseyear", StringComparison.OrdinalIgnoreCase))
        {
            query = sortOrder == "asc" ? query.OrderBy(f => f.ReleaseYear) : query.OrderByDescending(f => f.ReleaseYear);
        }
        else
        {
            query = sortOrder == "asc" ? query.OrderBy(f => f.Title) : query.OrderByDescending(f => f.Title);
        }

        return await query.ToListAsync();
    }

    public async Task<Film?> GetFilmById(int id)
    {
        return await _context.Films.FindAsync(id);
    }

    public async Task<Film> AddFilm(FilmSchema filmSchema)
    {
        var film = new Film
        {
            Title = filmSchema.Title,
            Genre = filmSchema.Genre,
            Director = filmSchema.Director,
            ReleaseYear = filmSchema.ReleaseYear,
            Rating = filmSchema.Rating,
            Description = filmSchema.Description
        };

        _context.Films.Add(film);
        await _context.SaveChangesAsync();
        return film;  
    }

    public async Task<bool> UpdateFilm(int id, FilmSchema filmSchema)
    {
        
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            return false;
        }
        
        film.Title = filmSchema.Title;
        film.Genre = filmSchema.Genre;
        film.Director = filmSchema.Director;
        film.ReleaseYear = filmSchema.ReleaseYear;
        film.Rating = filmSchema.Rating;
        film.Description = filmSchema.Description;
        
        _context.Entry(film).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteFilm(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            return false;
        }

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();
        return true;
    }
}