using ContactsConsoleAPI.Business;
using ContactsConsoleAPI.Business.Contracts;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;
using ContactsConsoleAPI.DataAccess.Contrackts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestContactDbContext dbContext;
        private IContactManager contactManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestContactDbContext();
            this.contactManager = new ContactManager(new ContactRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //Positive test
        [Test]
        public async Task AddContactAsync_ShouldAddNewContact()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            // Act
            await contactManager.AddAsync(newContact);

            // Assert
            var dbContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);

            Assert.NotNull(dbContact);
            Assert.That(dbContact.FirstName, Is.EqualTo(newContact.FirstName));
            Assert.That(dbContact.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(dbContact.Phone, Is.EqualTo(newContact.Phone));
            Assert.That(dbContact.Email, Is.EqualTo(newContact.Email));
            Assert.That(dbContact.Address, Is.EqualTo(newContact.Address));
            Assert.That(dbContact.Contact_ULID, Is.EqualTo(newContact.Contact_ULID));
        }

        //Negative test
        [Test]
        public async Task AddContactAsync_TryToAddContactWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "invalid_Mail", //invalid email
                Gender = "Male",
                Phone = "0889933779"
            };

            // Act and Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await contactManager.AddAsync(newContact));

            var actual = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid contact!"));
        }

        [Test]
        public async Task DeleteContactAsync_WithValidULID_ShouldRemoveContactFromDb()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            await contactManager.AddAsync(newContact);

            // Act
            await contactManager.DeleteAsync(newContact.Contact_ULID);

            // Assert
            var itemInDb = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Contact_ULID == newContact.Contact_ULID);

            Assert.IsNull(itemInDb);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public async Task DeleteContactAsync_TryToDeleteWithNullOrWhiteSpaceULID_ShouldThrowException(string invalidULID)
        {
            // Act and Assert
            var exception = Assert.Throws<ArgumentException>(() => contactManager.DeleteAsync(invalidULID));

            Assert.That(exception.Message, Is.EqualTo("ULID cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenContactsExist_ShouldReturnAllContacts()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            var secondNewContact = new Contact()
            {
                FirstName = "SecondTestFirstName",
                LastName = "SecondTestLastName",
                Address = "Anything for testing address yep",
                Contact_ULID = "1ABC23456YY", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Female",
                Phone = "0889933669"
            };
            await contactManager.AddAsync(newContact);
            await contactManager.AddAsync(secondNewContact);

            // Act
            var result = await contactManager.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));

            var itemInDb = result.First();
            Assert.That(itemInDb.Contact_ULID, Is.EqualTo(newContact.Contact_ULID));
            Assert.That(itemInDb.FirstName, Is.EqualTo(newContact.FirstName));
            Assert.That(itemInDb.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(itemInDb.Address, Is.EqualTo(newContact.Address));
            Assert.That(itemInDb.Email, Is.EqualTo(newContact.Email));
            Assert.That(itemInDb.Gender, Is.EqualTo(newContact.Gender));
            Assert.That(itemInDb.Phone, Is.EqualTo(newContact.Phone));
        }

        [Test]
        public async Task GetAllAsync_WhenNoContactsExist_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => contactManager.GetAllAsync());

            Assert.That(exception.Message, Is.EqualTo("No contact found."));
        }

        [Test]
        public async Task SearchByFirstNameAsync_WithExistingFirstName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            var secondNewContact = new Contact()
            {
                FirstName = "SecondTestFirstName",
                LastName = "SecondTestLastName",
                Address = "Anything for testing address yep",
                Contact_ULID = "1ABC23456YY", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Female",
                Phone = "0889933669"
            };
            await contactManager.AddAsync(newContact);
            await contactManager.AddAsync(secondNewContact);

            // Act
            var result = await contactManager.SearchByFirstNameAsync(newContact.FirstName);

            // Assert
            Assert.IsNotNull(result);

            var firstContact = result.First();
            Assert.That(firstContact.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(firstContact.Address, Is.EqualTo(newContact.Address));
            Assert.That(firstContact.Contact_ULID, Is.EqualTo(newContact.Contact_ULID));
            Assert.That(firstContact.Email, Is.EqualTo(newContact.Email));
            Assert.That(firstContact.Gender, Is.EqualTo(newContact.Gender));
            Assert.That(firstContact.Phone, Is.EqualTo(newContact.Phone));

        }

        [Test]
        public async Task SearchByFirstNameAsync_WithNonExistingFirstName_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => contactManager.SearchByFirstNameAsync("Non-Existing-Name"));

            Assert.That(exception.Message, Is.EqualTo("No contact found with the given first name."));
        }

        [Test]
        public async Task SearchByLastNameAsync_WithExistingLastName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            var secondNewContact = new Contact()
            {
                FirstName = "SecondTestFirstName",
                LastName = "SecondTestLastName",
                Address = "Anything for testing address yep",
                Contact_ULID = "1ABC23456YY", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Female",
                Phone = "0889933669"
            };
            await contactManager.AddAsync(newContact);
            await contactManager.AddAsync(secondNewContact);

            // Act
            var result = await contactManager.SearchByLastNameAsync(secondNewContact.LastName);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));

            var secondContact = result.First();
            Assert.That(secondNewContact.FirstName, Is.EqualTo(secondNewContact.FirstName));
            Assert.That(secondNewContact.Address, Is.EqualTo(secondNewContact.Address));
            Assert.That(secondNewContact.Contact_ULID, Is.EqualTo(secondNewContact.Contact_ULID));
            Assert.That(secondNewContact.Email, Is.EqualTo(secondNewContact.Email));
            Assert.That(secondNewContact.Gender, Is.EqualTo(secondNewContact.Gender));
            Assert.That(secondNewContact.Phone, Is.EqualTo(secondNewContact.Phone));
        }

        [Test]
        public async Task SearchByLastNameAsync_WithNonExistingLastName_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => contactManager.SearchByLastNameAsync("Non-Existing-Last-Name"));

            Assert.That(exception.Message, Is.EqualTo("No contact found with the given last name."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidULID_ShouldReturnContact()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            var secondNewContact = new Contact()
            {
                FirstName = "SecondTestFirstName",
                LastName = "SecondTestLastName",
                Address = "Anything for testing address yep",
                Contact_ULID = "1ABC23456YY", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Female",
                Phone = "0889933669"
            };
            await contactManager.AddAsync(newContact);
            await contactManager.AddAsync(secondNewContact);

            // Act
            var result = await contactManager.GetSpecificAsync(newContact.Contact_ULID);

            // Assert
            Assert.IsNotNull(result);

            var item = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Contact_ULID == newContact.Contact_ULID);
            Assert.That(item.FirstName, Is.EqualTo(newContact.FirstName));
            Assert.That(item.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(item.Address, Is.EqualTo(newContact.Address));
            Assert.That(item.Email, Is.EqualTo(newContact.Email));
            Assert.That(item.Gender, Is.EqualTo(newContact.Gender));
            Assert.That(item.Phone, Is.EqualTo(newContact.Phone));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidULID_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidULID = "InvalidULID";
            // Act and Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => contactManager.GetSpecificAsync(invalidULID));

            Assert.That(exception.Message, Is.EqualTo($"No contact found with ULID: {invalidULID}"));
        }

        [Test]
        public async Task UpdateAsync_WithValidContact_ShouldUpdateContact()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };
            var secondNewContact = new Contact()
            {
                FirstName = "SecondTestFirstName",
                LastName = "SecondTestLastName",
                Address = "Anything for testing address yep",
                Contact_ULID = "1ABC23456YY", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Female",
                Phone = "0889933669"
            };
            await contactManager.AddAsync(newContact);
            await contactManager.AddAsync(secondNewContact);

            var updatedContact = newContact;
            updatedContact.FirstName = "UPDATED Contact Name with new one";
            updatedContact.LastName = "UPDATED Contact Last Name";

            // Act
            await contactManager.UpdateAsync(updatedContact);

            // Assert
            var itemInDb = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Contact_ULID == updatedContact.Contact_ULID);
            Assert.NotNull(itemInDb);
            Assert.That(itemInDb.FirstName, Is.EqualTo(updatedContact.FirstName));
            Assert.That(itemInDb.LastName, Is.EqualTo(updatedContact.LastName));
            Assert.That(itemInDb.Address, Is.EqualTo(updatedContact.Address));
            Assert.That(itemInDb.Contact_ULID, Is.EqualTo(updatedContact.Contact_ULID));
            Assert.That(itemInDb.Email, Is.EqualTo(updatedContact.Email));
            Assert.That(itemInDb.Gender, Is.EqualTo(updatedContact.Gender));
            Assert.That(itemInDb.Phone, Is.EqualTo(updatedContact.Phone));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidContact_ShouldThrowValidationException()
        {
            // Act and Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() => contactManager.UpdateAsync(new Contact()));

            Assert.That(exception.Message, Is.EqualTo("Invalid contact!"));
        }
    }
}
