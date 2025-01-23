using FilmManagementSystem.Data;
using FilmManagementSystem.Models;
using FilmManagementSystem.Schemas;
using FilmManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FilmManagementSystem.Tests
{
    public class FilmServiceTests
    {
        private FilmContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FilmContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new FilmContext(options);
            context.Database.EnsureDeleted();  
            context.Database.EnsureCreated();  

            return context;
        }

        [Fact]
        public async Task GetFilms_ShouldReturnFilms()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            context.Films.Add(new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description for Film 1",  
                Director = "Director 1"                
            });
            context.Films.Add(new Film { 
                Title = "Film 2", 
                Genre = "Action", 
                Rating = 8.5, 
                ReleaseYear = 2021, 
                Description = "Description for Film 2",  
                Director = "Director 2"                
            });
            await context.SaveChangesAsync();

            var result = await service.GetFilms(null, null, "title", "asc");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilmById_ShouldReturnFilm()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            var film = new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description for Film 1", 
                Director = "Director 1"                
            };
            context.Films.Add(film);
            await context.SaveChangesAsync();

            var result = await service.GetFilmById(film.Id);

            Assert.NotNull(result);
            Assert.Equal(film.Title, result?.Title);
        }

        [Fact]
        public async Task AddFilm_ShouldAddFilm()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            var filmSchema = new FilmSchema { 
                Title = "New Film", 
                Genre = "Comedy", 
                Rating = 6.5, 
                ReleaseYear = 2022, 
                Description = "A funny movie",  
                Director = "John Doe"           
            };

            var result = await service.AddFilm(filmSchema);

            Assert.NotNull(result);
            Assert.Equal("New Film", result.Title);
            Assert.Single(context.Films);  
        }

        [Fact]
        public async Task UpdateFilm_ShouldUpdateFilm()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            var film = new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description for Film 1",  
                Director = "Director 1"                
            };
            context.Films.Add(film);
            await context.SaveChangesAsync();

            var updatedFilmSchema = new FilmSchema { 
                Title = "Updated Film", 
                Genre = "Drama", 
                Rating = 8.0, 
                ReleaseYear = 2021, 
                Description = "Updated description",  
                Director = "Updated Director"                
            };
            
            var result = await service.UpdateFilm(film.Id, updatedFilmSchema);

            Assert.True(result);
            var updated = await context.Films.FindAsync(film.Id);
            Assert.Equal("Updated Film", updated?.Title);
        }

        [Fact]
        public async Task DeleteFilm_ShouldDeleteFilm()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            var film = new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description for Film 1",  
                Director = "Director 1"                
            };
            context.Films.Add(film);
            await context.SaveChangesAsync();

            var result = await service.DeleteFilm(film.Id);

            Assert.True(result);
            var deletedFilm = await context.Films.FindAsync(film.Id);
            Assert.Null(deletedFilm);  
        }

        [Fact]
        public async Task GetFilms_ShouldReturnFilteredFilms_WhenGenreIsProvided()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            context.Films.Add(new Film { Title = "Film 1", Genre = "Drama", Rating = 7.8, ReleaseYear = 2020, Description = "Description for Film 1", Director = "Director 1" });
            context.Films.Add(new Film { Title = "Film 2", Genre = "Action", Rating = 8.5, ReleaseYear = 2021, Description = "Description for Film 2", Director = "Director 2" });
            context.Films.Add(new Film { Title = "Film 3", Genre = "Drama", Rating = 6.5, ReleaseYear = 2022, Description = "Description for Film 3", Director = "Director 3" });
            await context.SaveChangesAsync();

            var result = await service.GetFilms("Drama", null, "rating", "desc");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.Title == "Film 1");
            Assert.Contains(result, f => f.Title == "Film 3");
        }

        [Fact]
        public async Task UpdateFilm_ShouldReturnFalse_WhenIdDoesNotMatch()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);
            
            var film = new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description for Film 1",  
                Director = "Director 1"                
            };
            context.Films.Add(film);
            await context.SaveChangesAsync();
            
            var updatedFilmSchema = new FilmSchema { 
                Title = "Updated Film", 
                Genre = "Drama", 
                Rating = 8.0, 
                ReleaseYear = 2021, 
                Description = "Updated description",  
                Director = "Updated Director"                
            };
            
            var result = await service.UpdateFilm(999, updatedFilmSchema);  
            
            Assert.False(result);  
        }

        [Fact]
        public async Task GetFilms_ShouldReturnFilmsSortedByReleaseYear()
        {
            var context = CreateInMemoryDbContext();
            var service = new FilmService(context);

            context.Films.Add(new Film { 
                Title = "Film 1", 
                Genre = "Drama", 
                Rating = 7.8, 
                ReleaseYear = 2020, 
                Description = "Description 1",  
                Director = "Director 1"       
            });
            context.Films.Add(new Film { 
                Title = "Film 2", 
                Genre = "Action", 
                Rating = 8.5, 
                ReleaseYear = 2021, 
                Description = "Description 2",  
                Director = "Director 2"       
            });
            context.Films.Add(new Film { 
                Title = "Film 3", 
                Genre = "Drama", 
                Rating = 6.5, 
                ReleaseYear = 2019, 
                Description = "Description 3",  
                Director = "Director 3"       
            });
            await context.SaveChangesAsync();

            var result = await service.GetFilms(null, null, "releaseyear", "asc");

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal("Film 3", result.First().Title); 
            Assert.Equal("Film 2", result.Last().Title);   
        }
    }
}
