using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using LibroConsoleAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
    {
        private TestLibroDbContext dbContext;
        private BookManager bookManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestLibroDbContext();
            this.bookManager = new BookManager(new BookRepository(this.dbContext));
            dbContext.Database.EnsureDeleted();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBook()
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

            // Act
            await bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.AreEqual(newBook.Title, bookInDb.Title);
            Assert.AreEqual(newBook.Author, bookInDb.Author);
        }

        [Test]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = new string('B', 300),
                Author = new string('A', 300),
                ISBN = "invalidISBN",
                YearPublished = 2025,
                Genre = new string('C', 51),
                Pages = 0,
                Price = 0
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => this.bookManager.AddAsync(invalidBook));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Book is invalid."));
        }

        [Test]
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
            this.bookManager.AddAsync(newBook);
            // Act
            this.bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDb = await this.dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDb);
        }

        [Test]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Book",
                Author = "Doe",
                ISBN = "1111111111111",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };
            this.bookManager.AddAsync(newBook);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await bookManager.DeleteAsync(""));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(dbContext, bookManager);

            // Act
            var result = await bookManager.GetAllAsync();

            // Assert
            Assert.AreEqual(10, result.Count());

        }

        [Test]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var newBook = new Book();

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await bookManager.GetAllAsync());

            // Assert
            Assert.AreEqual("No books found.", exception.Message);
            
        }

        [Test]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(dbContext, bookManager);

            // Act
            var result = await bookManager.SearchByTitleAsync("1984");

            // Assert
            Assert.That(result.Single().Title, Is.EqualTo("1984"));
        }

        [Test]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(dbContext, bookManager);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await bookManager.SearchByTitleAsync("Home Alone 2"));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No books found with the given title fragment."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(dbContext, bookManager);

            // Act
            var result = await bookManager.GetSpecificAsync("9780312857753");

            // Assert
            Assert.That(result.ISBN, Is.EqualTo("9780312857753"));
            Assert.That(result.Title, Is.EqualTo("1984"));
            Assert.That(result.Author, Is.EqualTo("George Orwell"));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(dbContext, bookManager);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await bookManager.GetSpecificAsync("1112223334445"));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No book found with ISBN: 1112223334445"));
        }

        [Test]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "New Book",
                Author = "Simeon",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 200,
                Price = 19.99
            };
            await bookManager.AddAsync(newBook);

            newBook.Title = "Fantasy";
            newBook.Author = "Simeon Savov Jr";

            // Act
            await bookManager.UpdateAsync(newBook);

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync();
            Assert.That(bookInDb.Title, Is.EqualTo("Fantasy"));

        }

        [Test]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Book",
                Author = "Simeon",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Personal Growth",
                Pages = 200,
                Price = 19.99
            };
            await bookManager.AddAsync(newBook);
            // Act
            var invalidBook1 = new Book();

            var exception = Assert.ThrowsAsync<ValidationException>(async () => await bookManager.UpdateAsync(invalidBook1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Book is invalid."));
        }
    }
}
