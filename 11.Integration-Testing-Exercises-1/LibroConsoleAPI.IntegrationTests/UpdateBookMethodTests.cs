using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class UpdateBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public UpdateBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
            _dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "New Book Test",
                Author = "Simo",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 200,
                Price = 19.99
            };
            await _bookManager.AddAsync(newBook);
            
            newBook.Title = "Updated Title";
            newBook.Price = 50.99;

            // Act
            await _bookManager.UpdateAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == newBook.Title);
            Assert.Equal("Updated Title", bookInDb.Title);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "New Book Test",
                Author = "Simo Savov",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 500,
                Price = 25.99
            };
            await _bookManager.AddAsync(newBook);


            // Act
            var invalidBook1 = new Book();

            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.UpdateAsync(invalidBook1));

            // Assert
            Assert.Equal("Book is invalid.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidTitle_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test",
                Author = "Simo Savov",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 500,
                Price = 25.99
            };
            await _bookManager.AddAsync(newBook);
            // Act
            newBook.Title = new string('A', 300);
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.UpdateAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidAuthor_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book from myself",
                Author = "Simo Savov",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 500,
                Price = 25.99
            };
            await _bookManager.AddAsync(newBook);
            // Act
            newBook.Author = new string('B', 101);
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.UpdateAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidISBN_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book from myself",
                Author = "Simo Savov",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 500,
                Price = 25.99
            };
            await _bookManager.AddAsync(newBook);
            // Act
            newBook.ISBN = "AAAAAAA";
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _bookManager.UpdateAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }
    }
}
