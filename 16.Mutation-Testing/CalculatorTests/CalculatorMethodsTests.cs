using Calculator;

namespace CalculatorTests
{
    public class CalculatorMethodsTests
    {
        public CalculatorEngine calculatorEngine = new CalculatorEngine();

        [Test]
        [TestCase(5, 5, 10)]
        public void Sum_WhenWeProvideValidParams_ShouldReturnValidResult(int a, int b, int expect)
        {
            // Act
            var result = calculatorEngine.Sum(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expect));
        }

        [Test]
        [TestCase(5, 5, 0)]
        public void Substract_WhenWeProvideValidParams_ShouldReturnValidResult(int a, int b, int expect)
        {
            // Act
            var result = calculatorEngine.Substract(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expect));
        }

        [Test]
        [TestCase(5, 2, 10)]
        public void Multiply_WhenWeProvideValidParams_ShouldReturnValidResult(int a, int b, int expect)
        {
            // Act
            var result = calculatorEngine.Multiply(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expect));
        }

        [Test]
        [TestCase(4, 2, 2)]
        public void Divide_WhenWeProvideValidParams_ShouldReturnValidResult(int a, int b, int expect)
        {
            // Act
            var result = calculatorEngine.Divide(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expect));
        }
    }
}