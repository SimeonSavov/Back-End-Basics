using ItemManagementApp.Services;
using ItemManagementLib.Models;
using ItemManagementLib.Repositories;
using Moq;

namespace ItemManagement.Tests
{
    [TestFixture]
    public class ItemServiceTests
    {
        // Field to hold the mock repository and the service being tested
        private ItemService _itemService;
        private Mock<IItemRepository> _mockedItemRepository;
        
        [SetUp]
        public void Setup()
        {
            // Arrange: Create a mock instance of IItemRepository
            _mockedItemRepository = new Mock<IItemRepository>();

            // Instantiate ItemService with the mocked repository
            _itemService = new ItemService(_mockedItemRepository.Object);
        }

        [Test]
        public void AddItem_ShouldAddItem_IfNameIsValid()
        {
            // Arrange
            var item = new Item { Name = "Test Item" };
            _mockedItemRepository.Setup(x => x.AddItem(It.IsAny<Item>()));

            // Act
            _itemService.AddItem(item.Name);

            // Assert
            _mockedItemRepository.Verify(x => x.AddItem(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void AddItem_ShouldThrowError_IfNameIsInvalid()
        {
            // Arrange
            var invalidName = "";
            _mockedItemRepository
                .Setup(x => x.AddItem(It.IsAny<Item>()))
                .Throws<ArgumentException>();

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _itemService.AddItem(invalidName));
            _mockedItemRepository.Verify(x => x.AddItem(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Arrange
            var items = new List<Item>()
            {
                new Item { Id = 1, Name = "Item1"}
            };
            _mockedItemRepository.Setup(x => x.GetAllItems()).Returns(items);

            // Act
            var result = _itemService.GetAllItems();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            _mockedItemRepository.Verify(x => x.GetAllItems(), Times.Once);
        }

        [Test]
        public void GetItemById_ShouldReturnItemById_IfItemExist()
        {
            // Arrange
            var item = new Item { Name = "Single Item", Id = 1 };
            _mockedItemRepository.Setup(x => x.GetItemById(item.Id)).Returns(item);

            // Act
            var result = _itemService.GetItemById(item.Id);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(item.Name));
            _mockedItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once);
        }

        [Test]
        public void GetItemById_ShouldReturnNull_IfItemDoesNotExist()
        {
            // Arrange
            _mockedItemRepository.Setup(x => x.GetItemById(It.IsAny<int>())).Returns<Item>(null);

            // Act
            var result = _itemService.GetItemById(540);

            // Assert
            Assert.Null(result);
            _mockedItemRepository.Verify(x => x.GetItemById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateItem_ShouldNotUpdateItem_IfItemDoesNotExist()
        {
            // Arrange
            var nonExistingId = 1;
            _mockedItemRepository
                .Setup(x => x.GetItemById(nonExistingId))
                .Returns<Item>(null);

            _mockedItemRepository
                .Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act
            _itemService.UpdateItem(nonExistingId, "Invalid Name");

            // Assert
            _mockedItemRepository.Verify(x => x.GetItemById(nonExistingId), Times.Once);
            _mockedItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public void UpdateItem_ShouldThrowException_IfItemNameIsInvalid()
        {
            // Arrange
            var item = new Item { Name = "ItemName", Id = 1 };
            _mockedItemRepository
                .Setup(x => x.GetItemById(item.Id))
                .Returns(item);

            _mockedItemRepository
                .Setup(x => x.UpdateItem(It.IsAny<Item>()))
                .Throws<ArgumentException>();

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _itemService.UpdateItem(item.Id, ""));

            _mockedItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once);
            _mockedItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void UpdateItem_ShouldUpdateItem_IfItemNameIsValid()
        {
            // Arrange
            var item = new Item { Name = "ItemName", Id = 1 };
            _mockedItemRepository
                .Setup(x => x.GetItemById(item.Id))
                .Returns(item);

            _mockedItemRepository
                .Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act
            _itemService.UpdateItem(item.Id, "Item UPDATED");

            // Assert
            _mockedItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once);
            _mockedItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void DeleteItem_ShouldDeleteItem_WithExistingId()
        {
            // Arrange
            var itemId = 12;
            _mockedItemRepository
                .Setup(x => x.DeleteItem(itemId));

            // Act
            _itemService.DeleteItem(itemId);

            // Assert
            _mockedItemRepository.Verify(x => x.DeleteItem(itemId), Times.Once);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [TestCase("A", true)]
        [TestCase("SampleName", true)]
        [TestCase("NameS", true)]
        public void ValidateItemName_ShouldReturnCorrectAnswer_IfItemNameIsValid(string name, bool isValid)
        {
            // Act
            var result = _itemService.ValidateItemName(name);

            // Assert
            Assert.That(result, Is.EqualTo(isValid));
        }
    }
}