import { isOddOrEven } from './01.EvenOrOdd.js'
import { expect } from 'chai'

describe('isOddOrEven', () => {
    it('should return undefined when non string values is given', () => {
        // Arrange
        const inputValueNum = 15;
        const inputValueUndefined = undefined;
        const inputValueNull = null;
        const inputValueFloatNum = 15.15;
        // Act
        const resultNum = isOddOrEven(inputValueNum);
        const resultUndefined = isOddOrEven(inputValueUndefined);
        const resultNull = isOddOrEven(inputValueNull);
        const resultFloatNum = isOddOrEven(inputValueFloatNum);
        // Assert
        expect(resultNum).to.be.undefined;
        expect(resultUndefined).to.be.undefined;
        expect(resultNull).to.be.undefined;
        expect(resultFloatNum).to.be.undefined;
    })

    it('should return even result when string length input is even', () => {
        // Arrange
        const evenStringLength = '1234'
        // Act
        const evenStringResult = isOddOrEven(evenStringLength)
        // Assert
        expect(evenStringResult).to.be.equal('even')
    })

    it('should return odd result when string length input is odd', () => {
        // Arrange
        const oddStringLength = '123'
        // Act
        const oddStringResult = isOddOrEven(oddStringLength)
        // Assert
        expect(oddStringResult).to.be.equal('odd')
    })

    it('should return even when the input string is empty === 0', () => {
        // Arrange
        const zeroStringLength = ''
        // Act
        const zeroStringResult = isOddOrEven(zeroStringLength)
        // Assert
        expect(zeroStringResult).to.be.equal('even')
    })
})