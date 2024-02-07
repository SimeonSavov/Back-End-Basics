using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class AddBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public AddBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
            _dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567000000",
                YearPublished = 2021,
                Genre = "Modern",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal(newBook.Title, bookInDb.Title);
            Assert.Equal(newBook.Author, bookInDb.Author);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithAllInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = new string('B', 300),
                Author = new string('A', 300),
                ISBN = "invalid",
                YearPublished = 2025,
                Genre = new string('C', 51),
                Pages = 0,
                Price = 0
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(invalidBook));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithEmptyTitle_ShouldThrowException()
        {
            // Arrange
            var bookWithEmptyTitle = new Book
            {
                Title = "",
                Author = "John Doe",
                ISBN = "1111111111123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithEmptyTitle));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithEmptyAuthor_ShouldThrowException()
        {
            // Arrange
            var bookWithEmptyAuthor = new Book
            {
                Title = "My new Book",
                Author = "",
                ISBN = "5245678082103",
                YearPublished = 2020,
                Genre = "Action",
                Pages = 100,
                Price = 7.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithEmptyAuthor));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithEmptyISBN_ShouldThrowException()
        {
            // Arrange
            var bookWithEmptyISBN = new Book
            {
                Title = "Newest Book Ever",
                Author = "Favourite Author",
                ISBN = "",
                YearPublished = 2019,
                Genre = "Fantasy",
                Pages = 1000,
                Price = 2.99
            };

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithEmptyISBN));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidYearBelowTheLowerBoundary_ShouldThrowException()
        {
            // Arrange
            var bookWithInvalidYearBelowTheLowerBoundary = new Book
            {
                Title = "NewYork",
                Author = "FavAuthor",
                ISBN = "1234123412345",
                YearPublished = 1699,
                Genre = "Fantasy",
                Pages = 2000,
                Price = 50.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithInvalidYearBelowTheLowerBoundary));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidYearOverTheMaxBoundary_ShouldThrowException()
        {
            // Arrange
            var bookWithInvalidYearOverTheMaxBoundary = new Book
            {
                Title = "Florida",
                Author = "SimoSav",
                ISBN = "1234111412345",
                YearPublished = 2025,
                Genre = "Fantasy",
                Pages = 3000,
                Price = 50.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithInvalidYearOverTheMaxBoundary));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithEmptyGenreField_ShouldThrowException()
        {
            // Arrange
            var bookWithEmptyGenre = new Book
            {
                Title = "NeYork",
                Author = "FaAuthor",
                ISBN = "1234121112345",
                YearPublished = 1700,
                Genre = "",
                Pages = 5000,
                Price = 50.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithEmptyGenre));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithGenreOverTheMaxBoundary_ShouldThrowException()
        {
            // Arrange
            var bookWithGenreOverTheMaxBoundary = new Book
            {
                Title = "Miami",
                Author = "FaAuthor",
                ISBN = "1734121112345",
                YearPublished = 1900,
                Genre = new string('A', 51),
                Pages = 5000,
                Price = 50.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithGenreOverTheMaxBoundary));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithNegativePages_ShouldThrowException()
        {
            // Arrange
            var bookWithNegativePages = new Book
            {
                Title = "Shumniq Grad",
                Author = "SimeonS",
                ISBN = "1734122222345",
                YearPublished = 1900,
                Genre = "SciFi",
                Pages = -1,
                Price = 50.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithNegativePages));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithZeroPages_ShouldThrowException()
        {
            // Arrange
            var bookWithZeroPages = new Book
            {
                Title = "Shumen Grad",
                Author = "Savov",
                ISBN = "1734122222355",
                YearPublished = 1900,
                Genre = "SciFi",
                Pages = 0,
                Price = 100.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithZeroPages));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithNegativePrice_ShouldThrowException()
        {
            // Arrange
            var bookWithNegativePrice = new Book
            {
                Title = "Shumen Grad",
                Author = "Savov",
                ISBN = "1734122222355",
                YearPublished = 2000,
                Genre = "Fantasy",
                Pages = 200,
                Price = -100.00
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithNegativePrice));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithZeroPrice_ShouldThrowException()
        {
            // Arrange
            var bookWithZeroPrice = new Book
            {
                Title = "Shumen Grad",
                Author = "Savov",
                ISBN = "1734122222355",
                YearPublished = 2000,
                Genre = "Fantasy",
                Pages = 200,
                Price = 0
            };
            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.AddAsync(bookWithZeroPrice));

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Equal("Book is invalid.", exception.Result.Message);
            Assert.Null(bookInDb);
        }
    }
}
