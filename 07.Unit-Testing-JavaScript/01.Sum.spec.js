import { sum } from './01.Sum.js'
import { expect } from 'chai'

describe('sum', () => {
    it('should return zero if an empty array is given', () => {
        //Arrange
        const inputArray = [];

        //Act
        const result = sum(inputArray);

        //Assert
        expect(result).to.equals(0);
    })

    it('should return the single element as a sum if a single element array is given', () => {
        //Arrange
        const inputArray = [25];

        //Act
        const result = sum(inputArray);

        //Assert
        expect(result).to.equals(25);
    })

    it('should return the total sum of an array if a multi value array is given', () => {
        //Arrange
        const inputArray = [25, 25, 25];

        //Act
        const result = sum(inputArray);

        //Assert
        expect(result).to.equals(75);
    })

    it('should return a total sum of an array if the input is string as a numbers', () => {
        //Arrange
        const inputArray = ['25', '25', '25'];

        //Act
        const result = sum(inputArray);

        //Assert
        expect(result).to.equals(75);
    })
});