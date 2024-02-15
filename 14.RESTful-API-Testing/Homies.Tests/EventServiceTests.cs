using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Homies.Services;
using Microsoft.EntityFrameworkCore;

namespace Homies.Tests
{
    [TestFixture]
    internal class EventServiceTests
    {
        private HomiesDbContext _dbContext;
        private EventService _eventService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HomiesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use unique database name to avoid conflicts
                .Options;
            _dbContext = new HomiesDbContext(options);

            _eventService = new EventService(_dbContext);
        }

        [Test]
        public async Task AddEventAsync_ShouldAddEvent_WhenValidEventModelAndUserId()
        {
            // Step 1: Arrange - Set up the initial conditions for the test
            // Create a new event model with test data
            var eventModel = new EventFormModel
            {
                Name = "Test Event",
                Description = "Test Description",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2)
            };
            // Define a user ID for testing purposes
            string userId = "testUserId";

            // Step 2: Act - Perform the action being tested
            // Call the service method to add the event
            await _eventService.AddEventAsync(eventModel, userId);

            // Step 3: Assert - Verify the outcome of the action
            // Retrieve the added event from the database
            var eventInDb = await _dbContext.Events.FirstOrDefaultAsync(x => x.Name == eventModel.Name && x.OrganiserId == userId);

            // Assert that the added event is not null, indicating it was successfully added
            Assert.IsNotNull(eventInDb);

            // Assert that the description of the added event matches the description provided in the event model
            Assert.That(eventInDb.Description, Is.EqualTo(eventModel.Description));
            Assert.That(eventInDb.Start, Is.EqualTo(eventModel.Start));
            Assert.That(eventInDb.End, Is.EqualTo(eventModel.End));
        }


        [Test]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            // Step 1: Arrange - Set up the initial conditions for the test
            // Create two event models with test data
            var firstEventModel = new EventFormModel
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2)
            };

            var secondEventModel = new EventFormModel
            {
                Name = "The Second Event of the day",
                Description = "Description for the second test event",
                Start = DateTime.Now.AddDays(2),
                End = DateTime.Now.AddDays(3)
            };

            // Define a user ID for testing purposes
            string firstUserId = "firstUserId";
            string secondUserId = "secondUserId";

            // Call the service method to add the event
            await _eventService.AddEventAsync(firstEventModel, firstUserId);
            await _eventService.AddEventAsync(secondEventModel, secondUserId);

            // Step 2: Act - Perform the action being tested
            // Add the two events to the database using the event service
            var result = await _eventService.GetAllEventsAsync();

            // Step 3: Act - Retrieve the count of events from the database
            var countOfEvents = _dbContext.Events.Count();

            // Step 4: Assert - Verify the outcome of the action
            // Assert that the count of events in the database is equal to the expected count (2)
            Assert.That(result.Count(), Is.EqualTo(countOfEvents));
        }

        [Test]
        public async Task GetEventDetailAsync_ShouldReturnAllEventDetails()
        {
            // Arrange
            var firstEventModel = new EventFormModel
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = 2,
            };

            await _eventService.AddEventAsync(firstEventModel, "nonExistingUserId");

            var eventInDb = await _dbContext.Events.FirstAsync();

            // Act
            var result = await _eventService.GetEventDetailsAsync(eventInDb.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(firstEventModel.Name));
            Assert.That(result.Description, Is.EqualTo(firstEventModel.Description));
        }

        [Test]
        public async Task GetEventForEditAsync_ShouldGetEvent_IfPresentInTheDatabase()
        {
            // Arrange
            var firstEventModel = new EventFormModel
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = 2,
            };

            await _eventService.AddEventAsync(firstEventModel, "nonExistingUserId");

            var eventInDb = await _dbContext.Events.FirstAsync();

            // Act
            var result = await _eventService.GetEventForEditAsync(eventInDb.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(firstEventModel.Name));
            Assert.That(result.Description, Is.EqualTo(firstEventModel.Description));
        }

        [Test]
        public async Task GetEventForEditAsync_ShouldReturnNull_IfEventIsNotFound()
        {
            // Act
            var result = await _eventService.GetEventForEditAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEventOrganizerIdAsync_ShouldReturnOrganizerId_IfExisting()
        {
            // Arrange
            var firstEventModel = new EventFormModel
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = 2,
            };

            const string userId = "user-id";

            await _eventService.AddEventAsync(firstEventModel, userId);

            var eventInDb = await _dbContext.Events.FirstAsync();

            // Act
            var result = await _eventService.GetEventOrganizerIdAsync(eventInDb.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(userId));
        }

        [Test]
        public async Task GetEventOrganizerIdAsync_ShouldReturnNull_IfEventIsNotExisting()
        {
            // Act
            var result = await _eventService.GetEventOrganizerIdAsync(20);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserJoinedEventsAsync_ShouldReturnAllJoinedUsers()
        {
            // Arrange
            const string userId = "user-id";

            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };

            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = userId,
            };

            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();


            await _dbContext.EventsParticipants.AddAsync(new EventParticipant
            {
                EventId = testEvent.Id,
                HelperId = userId,
            });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.GetUserJoinedEventsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));

            var eventParticipation = result.First();
            Assert.That(eventParticipation.Id, Is.EqualTo(testEvent.Id));
            Assert.That(eventParticipation.Name, Is.EqualTo(testEvent.Name));
            Assert.That(eventParticipation.Type, Is.EqualTo(testType.Name)); 
        }

        [Test]
        public async Task JoinEventAsync_ShouldReturnFalse_IfEventDoesNotExist()
        {
            // Act
            var result = await _eventService.JoinEventAsync(20, "does not exist");

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task JoinEventAsync_ShouldReturnFalse_IfUserIsAlreadyPartOfTheEvent()
        {
            // Arrange
            const string userId = "user-id";

            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = userId,
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            // Add user to the event
            await _dbContext.EventsParticipants.AddAsync(new EventParticipant
            {
                EventId = testEvent.Id,
                HelperId = userId,
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.JoinEventAsync(testEvent.Id, userId);

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task JoinEventAsync_ShouldReturnTrue_IfTheUserIsAddedToTheEvent()
        {
            // Arrange
            const string userId = "user-id";

            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = "organizer-id",
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.JoinEventAsync(testEvent.Id, userId);

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task LeaveEventAsync_ShouldReturnFalse_IfWeTryToLeaveAnEventWeAreNotPartOf()
        {
            // Act
            var result = await _eventService.LeaveEventAsync(7, "user-id");

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task LeaveEventAsync_ShouldReturnTrue_IfWeLeaveTheEvent()
        {
            // Arrange
            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = "organizer-id",
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            var userId = "new-participant";
            await _eventService.JoinEventAsync(testEvent.Id, userId);

            // Act
            var result = await _eventService.LeaveEventAsync(testEvent.Id, userId);

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task UpdateEventAsync_ShouldReturnFalse_IfEventDoesNotExist()
        {
            // Act
            var result = await _eventService.UpdateEventAsync(12, new EventFormModel { }, "user-id");

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task UpdateEventAsync_ShouldReturnFalse_IfTheOrganizerOfTheEventIsDifferent()
        {
            // Arrange
            const string firstUserId = "first-user-id";
            const string secondUserId = "second-user-id";

            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = firstUserId,
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.UpdateEventAsync(testEvent.Id, new EventFormModel { }, secondUserId);

            // Assert
            Assert.False(result);
        }
        
        [Test]
        public async Task UpdateEventAsync_ShouldReturnTrue_IfWeUpdateTheEventSuccessfully()
        {
            // Arrange
            const string firstUserId = "first-user-id";

            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = firstUserId,
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.UpdateEventAsync(
                testEvent.Id, 
                new EventFormModel 
                { 
                    Name = "UPDATED NAME!",
                    Description = testEvent.Description,
                    Start = testEvent.Start,
                    End = testEvent.End,
                    TypeId = testType.Id,
                }, 
                firstUserId);

            // Assert
            Assert.True(result);

            var eventFromDb = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == testEvent.Id);
            Assert.IsNotNull(eventFromDb);
            Assert.That(eventFromDb.Name, Is.EqualTo("UPDATED NAME!"));
        }

        [Test]
        public async Task GetAllTypesAsync_ShouldReturnAllTypes()
        {
            // Arrange
            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.GetAllTypesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));

            var singleType = result.First();
            Assert.That(singleType.Name, Is.EqualTo(testType.Name));
        }

        [Test]
        public async Task IsUserJoinedEventAsync_ShouldReturnFalse_IfEventDoesNotExist()
        {
            // Act
            var result = await _eventService.IsUserJoinedEventAsync(99, "current user");

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task IsUserJoinedEventAsync_ShouldReturnFalse_IfUserDoesNotExisting()
        {
            // Arrange
            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = "first-user-id",
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _eventService.IsUserJoinedEventAsync(testEvent.Id, "does not exist");

            // Assert
            Assert.False(result);
        }

        [Test]
        public async Task IsUserJoinedEventAsync_ShouldReturnTrue_UserIsInTheEvent()
        {
            // Arrange
            // Add an event type to the database
            var testType = new Data.Models.Type
            {
                Name = "TestType"
            };
            await _dbContext.Types.AddAsync(testType);
            await _dbContext.SaveChangesAsync();

            // Add an event to the database
            var testEvent = new Event
            {
                Name = "The First Event",
                Description = "Description for the First Event",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2),
                TypeId = testType.Id,
                OrganiserId = "first-user-id",
            };
            await _dbContext.Events.AddAsync(testEvent);
            await _dbContext.SaveChangesAsync();

            await _eventService.JoinEventAsync(testEvent.Id, "joined-id");

            // Act
            var result = await _eventService.IsUserJoinedEventAsync(testEvent.Id, "joined-id");

            // Assert
            Assert.True(result);
        }
    }
}
