import { mathEnforcer } from './03.MathEnforcer.js'
import { expect } from 'chai'

describe('mathEnforcer', () => {
    describe('addFive', () => {
        it('should return undefined when input string is given', () => {
            // Arrange
            const stringInput = 'someString';
            // Act
            const undefinedResult = mathEnforcer.addFive(stringInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when input undefined is given', () => {
            // Arrange
            const undefinedInput = undefined;
            // Act
            const undefinedResult = mathEnforcer.addFive(undefinedInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when input is number as a string', () => {
            // Arrange
            const numberAsAInput = '11';
            // Act
            const undefinedResult = mathEnforcer.addFive(numberAsAInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return correct result when input is floating number and assert with closeTo ', () => {
            // Arrange
            const floatingInput = 1.01;
            // Act
            const correctResult = mathEnforcer.addFive(floatingInput);
            // Assert
            expect(correctResult).to.be.closeTo(6.01, 0.01);
        })

        it('should return correct result when input is floating number and assert with equal ', () => {
            // Arrange
            const floatingInput = 1.01;
            // Act
            const correctResult = mathEnforcer.addFive(floatingInput);
            // Assert
            expect(correctResult).to.be.equal(6.01);
        })

        it('should return correct result when input is floating number with a lot of digits and assert with closeTo ', () => {
            // Arrange
            const floatingInput = 1.0000001;
            // Act
            const correctResult = mathEnforcer.addFive(floatingInput);
            // Assert
            expect(correctResult).to.be.closeTo(6.01, 0.01);
        })

        it('should return correct result when input is integer number', () => {
            // Arrange
            const integerNumber = 5;
            // Act
            const correctResult = mathEnforcer.addFive(integerNumber);
            // Assert
            expect(correctResult).to.be.equal(10);
        })

        it('should return correct result when input is negative integer number', () => {
            // Arrange
            const negativeNumber = -15;
            // Act
            const correctResult = mathEnforcer.addFive(negativeNumber);
            // Assert
            expect(correctResult).to.be.equal(-10);
        })

        it('should return correct result when input is negative integer number', () => {
            // Arrange
            const negativeNumber = -5;
            // Act
            const correctResult = mathEnforcer.addFive(negativeNumber);
            // Assert
            expect(correctResult).to.be.equal(0);
        })

        it('should return correct result when input is a zero', () => {
            // Arrange
            const zeroInput = 0;
            // Act
            const correctResult = mathEnforcer.addFive(zeroInput);
            // Assert
            expect(correctResult).to.be.equal(5);
        })
    })

    describe('subtractTen', () => {
        it('should return correct result when input is zero', () => {
            // Arrange
            const zeroInput = 0;
            // Act
            const correctResult = mathEnforcer.subtractTen(zeroInput);
            // Assert
            expect(correctResult).to.be.equal(-10);
        })

        it('should return undefined when input string is given', () => {
            // Arrange
            const stringInput = 'someString';
            // Act
            const undefinedResult = mathEnforcer.subtractTen(stringInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when input undefined is given', () => {
            // Arrange
            const undefinedInput = undefined;
            // Act
            const undefinedResult = mathEnforcer.subtractTen(undefinedInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when input is number as a string', () => {
            // Arrange
            const numberAsAInput = '11';
            // Act
            const undefinedResult = mathEnforcer.subtractTen(numberAsAInput);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return correct result when input is floating number and assert with closeTo ', () => {
            // Arrange
            const floatingInput = 1.01;
            // Act
            const correctResult = mathEnforcer.subtractTen(floatingInput);
            // Assert
            expect(correctResult).to.be.closeTo(-8.99, 0.01);
        })

        it('should return correct result when input is floating number and assert with equal ', () => {
            // Arrange
            const floatingInput = 1.01;
            // Act
            const correctResult = mathEnforcer.subtractTen(floatingInput);
            // Assert
            expect(correctResult).to.be.equal(-8.99);
        })

        it('should return correct result when input is floating number with a lot of digits and assert with closeTo ', () => {
            // Arrange
            const floatingInput = 1.0000001;
            // Act
            const correctResult = mathEnforcer.subtractTen(floatingInput);
            // Assert
            expect(correctResult).to.be.closeTo(-8.99, 0.01);
        })

        it('should return correct result when input is integer number', () => {
            // Arrange
            const integerNumber = 5;
            // Act
            const correctResult = mathEnforcer.subtractTen(integerNumber);
            // Assert
            expect(correctResult).to.be.equal(-5);
        })

        it('should return correct result when input is negative integer number', () => {
            // Arrange
            const negativeNumber = -15;
            // Act
            const correctResult = mathEnforcer.subtractTen(negativeNumber);
            // Assert
            expect(correctResult).to.be.equal(-25);
        })

        it('should return correct result when input is positive integer number', () => {
            // Arrange
            const positiveNumber = 10;
            // Act
            const correctResult = mathEnforcer.subtractTen(positiveNumber);
            // Assert
            expect(correctResult).to.be.equal(0);
        })
    })

    describe('sum', () => {
        it('should return correct result when both parameters are zero', () => {
            // Arrange
            const zeroInput1 = 0;
            const zeroInput2 = 0;
            // Act
            const correctResult = mathEnforcer.sum(zeroInput1, zeroInput2);
            // Assert
            expect(correctResult).to.be.equal(0);
        })

        it('should return undefined when first parameter is incorrect and second parameter is correct', () => {
            // Arrange
            const firstIncorrectParam = 'some';
            const secondCorrectParam = 5;
            // Act
            const undefinedResult = mathEnforcer.sum(firstIncorrectParam, secondCorrectParam);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when first parameter is incorrect and second parameter is incorrect', () => {
            // Arrange
            const firstIncorrectParam = 'some';
            const secondIncorrectParam = 'some';
            // Act
            const undefinedResult = mathEnforcer.sum(firstIncorrectParam, secondIncorrectParam);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when first parameter is correct and second parameter is incorrect', () => {
            // Arrange
            const firstCorrectParam = 5;
            const secondIncorrectParam = 'some';
            // Act
            const undefinedResult = mathEnforcer.sum(firstCorrectParam, secondIncorrectParam);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return undefined when first parameter is number as string, second is correct', () => {
            // Arrange
            const firstStringNumberParam = '5';
            const secondCorrectParam = 5;
            // Act
            const undefinedResult = mathEnforcer.sum(firstStringNumberParam, secondCorrectParam);
            // Assert
            expect(undefinedResult).to.be.undefined;
        })

        it('should return correct result when first parameter is number, second is number', () => {
            // Arrange
            const firstCorrectParam = 5;
            const secondCorrectParam = 5;
            // Act
            const correctResult = mathEnforcer.sum(firstCorrectParam, secondCorrectParam);
            // Assert
            expect(correctResult).to.be.equal(10);
        })

        it('should return correct result when first parameter is negative number, second is negative number', () => {
            // Arrange
            const firstCorrectParam = -5;
            const secondCorrectParam = -5;
            // Act
            const correctResult = mathEnforcer.sum(firstCorrectParam, secondCorrectParam);
            // Assert
            expect(correctResult).to.be.equal(-10);
        })

        it('should return correct result when first parameter is floating number, second is number and assert with equal', () => {
            // Arrange
            const firstFloatingParam = 5.01;
            const secondCorrectParam = 5;
            // Act
            const correctResult = mathEnforcer.sum(firstFloatingParam, secondCorrectParam);
            // Assert
            expect(correctResult).to.be.equal(10.01);
        })

        it('should return correct result when first parameter is floating number, second is number and assert with closeTo', () => {
            // Arrange
            const firstFloatingParam = 5.0000001;
            const secondCorrectParam = 5;
            // Act
            const correctResult = mathEnforcer.sum(firstFloatingParam, secondCorrectParam);
            // Assert
            expect(correctResult).to.be.closeTo(10.01, 0.01);
        })

        it('should return correct result when first parameter is negative number, second is positive number', () => {
            // Arrange
            const firstFloatingParam = -5;
            const secondCorrectParam = 5;
            // Act
            const correctResult = mathEnforcer.sum(firstFloatingParam, secondCorrectParam);
            // Assert
            expect(correctResult).to.be.equal(0);
        })

        it('should return correct result when first parameter is zero, second is floating number', () => {
            // Arrange
            const firstPositiveParam = 0;
            const secondFloatingParam = 0.1;
            // Act
            const correctResult = mathEnforcer.sum(firstPositiveParam, secondFloatingParam);
            // Assert
            expect(correctResult).to.be.closeTo(0.1, 0.01);
        })
    })
})