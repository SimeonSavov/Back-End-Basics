import { createCalculator } from './04.AddAndSubstract.js'
import { expect } from 'chai'

describe('createCalculator', () => {
    it('should return 0 if no operation are executed on the calculator', () => {
        // Arrange
        const calculator = createCalculator();
        // Act
        const result = calculator.get();
        // Assert
        expect(result).equals(0);
    })

    it('should return a negative number if only substract operations are executed with positive numbers on the calc', () => {
        // Arrange
        const calculator = createCalculator();
        // Act
        calculator.subtract(3)
        calculator.subtract(3)
        calculator.subtract(20)
        const result = calculator.get();
        // Assert
        expect(result).equals(-26);
    })

    it('should return a positive number if only add operations are executed with positive numbers on the calc', () => {
        // Arrange
        const calculator = createCalculator();
        // Act
        calculator.add(3)
        calculator.add(3)
        calculator.add(20)
        const result = calculator.get();
        // Assert
        expect(result).equals(26);
    })

    it('should handle numeric values as strings', () => {
        // Arrange
        const calculator = createCalculator();
        // Act
        calculator.add('3')
        calculator.add('3')
        calculator.add('20')
        const result = calculator.get();
        // Assert
        expect(result).equals(26);
    })

    it('should handle a mix of operations', () => {
        // Arrange
        const calculator = createCalculator();
        // Act
        calculator.add(3)
        calculator.add(3)
        calculator.add(20)
        calculator.subtract('20')
        calculator.subtract('3')
        const result = calculator.get();
        // Assert
        expect(result).equals(3);
    })
})