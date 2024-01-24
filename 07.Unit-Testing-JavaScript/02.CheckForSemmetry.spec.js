import { isSymmetric } from './02.CheckForSemmetry.js'
import { expect } from 'chai'

describe('isSymmetric', () => {
    it('if given a empty array should return true', () => {
        // Arrange
        const inputArr = [];
        // Act
        const result = isSymmetric(inputArr);
        // Assert
        expect(result).to.be.true;
    })

    it('should return false if a non-array value is given', () => {
        // Arrange
        // Act
        const nanResult = isSymmetric(NaN);
        const undefiendResult = isSymmetric(undefined);
        const nullResult = isSymmetric(null);
        const stringResult = isSymmetric('hey');
        const objectResult = isSymmetric({});
        const numericResult = isSymmetric(123);
        // Assert
        expect(nanResult).to.be.false;
        expect(undefiendResult).to.be.false;
        expect(nullResult).to.be.false;
        expect(stringResult).to.be.false;
        expect(objectResult).to.be.false;
        expect(numericResult).to.be.false;
    })

    it('should return false if a non-semmetric array is given', () => {
        // Arrange
        const nonSemmetricArr = [1, 2, 3, 4]
        // Act
        const result = isSymmetric(nonSemmetricArr);
        // Assert
        expect(result).to.be.false;
    })

    it('should return true if a semmetric array is given', () => {
        // Arrange
        const semmetricArr = [3, 2, 1, 2, 3]
        // Act
        const result = isSymmetric(semmetricArr);
        // Assert
        expect(result).to.be.true;
    })

    it('should return false if a string and numeric semmetric values are given', () => {
        // Arrange
        const semmetricArr = [3, 2, 1, '2', '3']
        // Act
        const result = isSymmetric(semmetricArr);
        // Assert
        expect(result).to.be.false;
    })
})