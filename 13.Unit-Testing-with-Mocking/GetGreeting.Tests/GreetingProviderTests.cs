using GetGreeting;
using Moq;

namespace GetGreeting.Tests
{
    public class Tests
    {
        private GreetingProvider _greetingProvider;
        private Mock<ITimeProvider> _mockedTimeProvider;

        [SetUp]
        public void Setup()
        {
            _mockedTimeProvider = new Mock<ITimeProvider>();
            _greetingProvider = new GreetingProvider(_mockedTimeProvider.Object);
        }

        [Test]
        public void GetGreeting_ShouldReturnAMorningMessage_WhenItIsMorningHour()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2002, 2, 2, 9, 0, 0));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good morning!"));

        }

        [Test]
        public void GetGreeting_ShouldReturnAAfternoonMessage_WhenItIsAfternoonHour()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2020, 3, 3, 13, 0, 0));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good afternoon!"));          

        }

        [Test]
        public void GetGreeting_ShouldReturnAEveningMessage_WhenItIsEveningHour()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2022, 1, 21, 19, 0, 0));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good evening!"));

        }

        [Test]
        public void GetGreeting_ShouldReturnANightMessage_WhenItIsNightHour()
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2019, 1, 1, 23, 0, 0));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good night!"));

        }

        [TestCase("Good morning!", 9)]
        [TestCase("Good afternoon!", 14)]
        [TestCase("Good evening!", 20)]
        [TestCase("Good night!", 3)]
        public void GetGreeting_ShouldReturnCorrectMessage_WhenTimeIsCorrect(string message, int hour)
        {
            // Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2019, 1, 1, hour, 0, 0));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo($"{message}"));

        }
    }
}