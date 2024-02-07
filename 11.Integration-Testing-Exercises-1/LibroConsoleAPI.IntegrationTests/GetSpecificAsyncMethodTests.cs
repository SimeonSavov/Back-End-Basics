using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class GetSpecificAsyncMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public GetSpecificAsyncMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
            _dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var result = await this._bookManager.GetSpecificAsync("9780385487256");

            // Assert
            Assert.Equal("9780385487256", result.ISBN);
            Assert.Equal("Harper Lee", result.Author);
            Assert.Equal(1960, result.YearPublished);
        }

        [Fact]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var invalidISBN = "1111111111111";
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _bookManager.GetSpecificAsync(invalidISBN));

            // Assert
            Assert.Equal($"No book found with ISBN: {invalidISBN}", exception.Message);
        }

        [Fact]
        public async Task GetSpecificAsync_WithEmptyIsbn_ShouldThrowArgumentException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _bookManager.GetSpecificAsync(""));

            // Assert
            Assert.Equal($"ISBN cannot be empty.", exception.Message);
        }
    }
}
