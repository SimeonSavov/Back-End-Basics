using MongoDB.Driver;
using MoviesLibraryAPI.Controllers;
using MoviesLibraryAPI.Controllers.Contracts;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;
using MoviesLibraryAPI.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace MoviesLibraryAPI.XUnitTests
{
    public class XUnitIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly MoviesLibraryXUnitTestDbContext _dbContext;
        private readonly IMoviesLibraryController _controller;
        private readonly IMoviesRepository _repository;

        public XUnitIntegrationTests(DatabaseFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _repository = new MoviesRepository(_dbContext.Movies);
            _controller = new MoviesLibraryController(_repository);
        }

        [Fact]
        public async Task AddMovieAsync_WhenValidMovieProvided_ShouldAddToDatabase()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };

            // Act
            await _controller.AddAsync(movie);

            // Assert
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == "Test Movie").FirstOrDefaultAsync();
            Assert.NotNull(resultMovie);
            Assert.Equal("Test Movie", resultMovie.Title);
            Assert.Equal("Test Director", resultMovie.Director);
            Assert.Equal(2022, resultMovie.YearReleased);
            Assert.Equal("Action", resultMovie.Genre);
            Assert.Equal(120, resultMovie.Duration);
            Assert.Equal(7.5, resultMovie.Rating);
        }

        [Fact]
        public async Task AddMovieAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            var invalidMovie = new Movie
            {
                // Provide an invalid movie object, e.g., without a title or other required fields
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(invalidMovie));
            Assert.Equal("Movie is not valid.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenValidTitleProvided_ShouldDeleteMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Movie of the Year",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };
            await _controller.AddAsync(movie);

            // Act
            await _controller.DeleteAsync(movie.Title);

            // Assert
            // The movie should no longer exist in the database
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == movie.Title).FirstOrDefaultAsync();
            Assert.Null(resultMovie);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public async Task DeleteAsync_WhenTitleIsNull_ShouldThrowArgumentException(string invalidName)
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(() => _controller.DeleteAsync(invalidName));
        }

        [Fact]
        public async Task DeleteAsync_WhenTitleDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteAsync("Movie With No Title found"));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoMoviesExist_ShouldReturnEmptyList()
        {
            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WhenMoviesExist_ShouldReturnAllMovies()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Movie One",
                Director = "Simeon Savov",
                YearReleased = 2000,
                Genre = "Action",
                Duration = 200,
                Rating = 10.0
            };

            var secondMovie = new Movie
            {
                Title = "Second Film",
                Director = "Simeon",
                YearReleased = 2022,
                Genre = "Personal Growth",
                Duration = 100,
                Rating = 9.0
            };

            await _dbContext.Movies.InsertManyAsync(new[] { movie, secondMovie });

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            // Ensure that all movies are returned
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            
        }

        [Fact]
        public async Task GetByTitle_WhenTitleExists_ShouldReturnMatchingMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Favourite Movie",
                Director = "Simeon Savov",
                YearReleased = 2000,
                Genre = "Action",
                Duration = 200,
                Rating = 10.0
            };

            var secondMovie = new Movie
            {
                Title = "Get Rich",
                Director = "Simeon",
                YearReleased = 2022,
                Genre = "Personal Growth",
                Duration = 100,
                Rating = 9.0
            };

            await _dbContext.Movies.InsertManyAsync(new[] { movie, secondMovie });

            // Act
            var result = await _controller.GetByTitle(secondMovie.Title);

            // Assert
            Assert.Null(result);
            Assert.Equal(secondMovie.Title, result.Title);
            Assert.Equal(secondMovie.Director, result.Director);
            Assert.Equal(secondMovie.YearReleased, result.YearReleased);
            Assert.Equal(secondMovie.Genre, result.Genre);
            Assert.Equal(secondMovie.Duration, result.Duration);
            Assert.Equal(secondMovie.Rating, result.Rating);

        }

        [Fact]
        public async Task GetByTitle_WhenTitleDoesNotExist_ShouldReturnNull()
        {
            // Act
            var resultMovie = await _controller.GetByTitle("Title does not exist");
            // Assert
            Assert.Null(resultMovie);
        }


        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenTitleFragmentExists_ShouldReturnMatchingMovies()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Total War",
                Director = "Simeon Savov",
                YearReleased = 1980,
                Genre = "Action",
                Duration = 200,
                Rating = 10.0
            };

            var secondMovie = new Movie
            {
                Title = "Fifty Cent",
                Director = "Simeon",
                YearReleased = 2010,
                Genre = "Personal Growth",
                Duration = 100,
                Rating = 9.0
            };

            await _dbContext.Movies.InsertManyAsync(new[] { movie, secondMovie });

            // Act
            var result = await _controller.SearchByTitleFragmentAsync("Cent");

            // Assert // Should return one matching movie
            Assert.Single(result);

            var movieResult = result.First();
            Assert.Equal(secondMovie.Title, movieResult.Title);
            Assert.Equal(secondMovie.Director, movieResult.Director);
            Assert.Equal(secondMovie.YearReleased, movieResult.YearReleased);
            Assert.Equal(secondMovie.Genre, movieResult.Genre);
            Assert.Equal(secondMovie.Duration, movieResult.Duration);
            Assert.Equal(secondMovie.Rating, movieResult.Rating);
        }

        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenNoMatchingTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.SearchByTitleFragmentAsync("Twenty One"));
        }

        [Fact]
        public async Task UpdateAsync_WhenValidMovieProvided_ShouldUpdateMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Djumandji",
                Director = "Simeon Savov",
                YearReleased = 1999,
                Genre = "Personal Growth",
                Duration = 200,
                Rating = 10.0
            };

            var secondMovie = new Movie
            {
                Title = "Brothers",
                Director = "Simeon",
                YearReleased = 2010,
                Genre = "Personal Growth",
                Duration = 100,
                Rating = 9.0
            };

            await _dbContext.Movies.InsertManyAsync(new[] { movie, secondMovie });

            // Modify the movie
            movie.Title = "The Rock";
            movie.Duration = 240;

            // Act
            await _controller.UpdateAsync(movie);

            // Assert
            var movieInDb = await _dbContext.Movies.Find(m => m.Title == movie.Title).FirstOrDefaultAsync();
            Assert.NotNull(movieInDb);
            Assert.Equal(movie.Title, movieInDb.Title);
            Assert.Equal(movie.Director, movieInDb.Director);
            Assert.Equal(movie.YearReleased, movieInDb.YearReleased);
            Assert.Equal(movie.Genre, movieInDb.Genre);
            Assert.Equal(movie.Duration, movieInDb.Duration);
            Assert.Equal(movie.Rating, movieInDb.Rating);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            // Movie without required fields
            var invalidMovie = new Movie
            {
                Rating = 10.0
            };

            // Act and Assert
            Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateAsync(invalidMovie));
        }
    }
}
