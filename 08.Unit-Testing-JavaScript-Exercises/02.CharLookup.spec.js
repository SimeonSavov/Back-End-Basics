import { lookupChar } from './02.CharLookup.js'
import { expect } from 'chai'

describe('lookupChar', () => {
    it('should return undefined when first parameter is NOT string, and second parameter is integer', () => {
        // Arrange
        const incorrectFirstParam = 1;
        const correctSecondParam = 15;
        // Act
        const undefinedResult = lookupChar(incorrectFirstParam, correctSecondParam);
        // Assert
        expect(undefinedResult).to.be.undefined;
    })

    it('should return undefined when first parameter is string, and second parameter is NOT integer', () => {
        // Arrange
        const correctFirstParam = 'string here';
        const incorrectSecondParam = '10';
        // Act
        const undefinedResult = lookupChar(correctFirstParam, incorrectSecondParam);
        // Assert
        expect(undefinedResult).to.be.undefined;
    })

    it('should return undefined when first parameter is string, and second parameter is incorrect float number', () => {
        // Arrange
        const correctFirstParam = 'string here';
        const incorrectFloatSecondParam = 1.10;
        // Act
        const undefinedResult = lookupChar(correctFirstParam, incorrectFloatSecondParam);
        // Assert
        expect(undefinedResult).to.be.undefined;
    })

    it('should return incorect index when first parameter is correct type, and second parameter is over the string length', () => {
        // Arrange
        const correctFirstParam = 'string here'
        const overTheStringSecondParam = 12
        // Act
        const incorectIndexResult = lookupChar(correctFirstParam, overTheStringSecondParam)
        // Assert
        expect(incorectIndexResult).to.be.equal('Incorrect index');
    })

    it('should return incorect index when first parameter is correct type, and second parameter is less than string length', () => {
        // Arrange
        const correctFirstParam = 'string here'
        const lowerLengthSecondParam = -12
        // Act
        const incorectIndexResult = lookupChar(correctFirstParam, lowerLengthSecondParam)
        // Assert
        expect(incorectIndexResult).to.be.equal('Incorrect index');
    })

    it('should return correct char when all parameter are correct' , () => {
        // Arrange
        const correctFirstParam = 'string here';
        const correctSecondParam = 1;
        // Act
        const correctResult = lookupChar(correctFirstParam, correctSecondParam)
        // Assert
        expect(correctResult).to.be.equal('t');
    })
})