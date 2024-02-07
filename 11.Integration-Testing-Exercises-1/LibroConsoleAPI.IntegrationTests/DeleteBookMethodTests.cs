using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class DeleteBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public DeleteBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
            _dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };
            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDb);
        }

        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            var newBook = new Book
            {
                Title = "This is new book",
                Author = "Simeon Savov",
                ISBN = "1234567890123",
                YearPublished = 2020,
                Genre = "Personal Growth",
                Pages = 150,
                Price = 29.99
            };
            await _bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _bookManager.DeleteAsync(""));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithWhiteSpaceISBN_ShouldThrowException()
        {
            var newBook = new Book
            {
                Title = "This is  book",
                Author = "Simeon Savov",
                ISBN = "1234567890123",
                YearPublished = 2020,
                Genre = "Personal Growth",
                Pages = 150,
                Price = 29.99
            };
            await _bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _bookManager.DeleteAsync("      "));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithNullISBN_ShouldThrowException()
        {
            var newBook = new Book
            {
                Title = "This is  book",
                Author = "Simeon Savov",
                ISBN = "1234567890123",
                YearPublished = 2020,
                Genre = "Personal Growth",
                Pages = 150,
                Price = 29.99
            };
            await _bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _bookManager.DeleteAsync(null));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }
    }
}
