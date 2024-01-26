import { analyzeArray } from './04.ArrayAnalyzer.js'
import { expect } from 'chai'

describe('analyzeArray', () => {
    it('should return undefined when string input is given', () => {
        // Arrange
        const nonArrayInput = 'something';
        // Act
        const undefinedResult = analyzeArray(nonArrayInput);
        // Assert
        expect(undefinedResult).to.be.undefined;
    })

    it('should return undefined when empty array is given', () => {
        // Arrange
        const emptyArrayInput = [];
        // Act
        const undefinedResult = analyzeArray(emptyArrayInput);
        // Assert
        expect(undefinedResult).to.be.undefined;
    })

    it('should return correct result when is given array with different numbers', () => {
        // Arrange
        const correctArrayInput = [3, 5, -2, 4, 1];
        // Act
        const correctResult = analyzeArray(correctArrayInput);
        // Assert
        expect(correctResult).to.deep.equal({min: -2, max: 5, length: 5});
    })

    it('should return correct result when is given array with only one number', () => {
        // Arrange
        const arrayInput = [3];
        // Act
        const correctResult = analyzeArray(arrayInput);
        // Assert
        expect(correctResult).to.deep.equal({min: 3, max: 3, length: 1});
    })

    it('should return correct result when is given array with multiple same elements ', () => {
        // Arrange
        const arraySameInput = [8, 8, 8, 8];
        // Act
        const correctResult = analyzeArray(arraySameInput);
        // Assert
        expect(correctResult).to.deep.equal({min: 8, max: 8, length: 4});
    })
})