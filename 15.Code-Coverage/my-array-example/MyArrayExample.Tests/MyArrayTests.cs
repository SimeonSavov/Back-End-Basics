namespace MyArrayExample.Tests
{
    public class MyArrayTests
    {
        [Test]
        public void Replace_ShouldReplace_IfPositionIsValid()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act
            var result = myArray.Replace(0, 100);

            // Assert
            Assert.True(result);
            Assert.That(myArray.Array[0], Is.EqualTo(100));
        }

        [Test]
        public void Replace_ShouldNotReplace_IfPositionIsLessThanZero()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => myArray.Replace(-20, 2));
        }

        [Test]
        public void Replace_ShouldNotReplace_IfPositionIsAboveTheSizeOfTheArray()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => myArray.Replace(100, 2));
        }

        [Test]
        public void FindMax_ShouldReturnMaxValueFromTheArray()
        {
            // Arrange
            var myArray = new MyArray(20);

            // Act
            var result = myArray.FindMax();

            // Assert
            Assert.That(result, Is.EqualTo(19));
        }

        [Test]
        public void FindMax_ShouldThrowException_IfArrayIsEmpty()
        {
            // Arrange
            var myArray = new MyArray(0);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => myArray.FindMax());
        }
    }
}