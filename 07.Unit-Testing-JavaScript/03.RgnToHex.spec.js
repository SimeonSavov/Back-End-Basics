import { rgbToHexColor } from './03.RgbToHex.js'
import { expect } from 'chai'

describe('rgbToHexColor', () => {
    it('should return undefined if red value is invalid', () => {
        // Arrange
        // Act
        const nonNumericRedValueResult = rgbToHexColor('200', 0 ,0)
        const negativeRedValueResult = rgbToHexColor(-13, 0, 0)
        const overLimitRedValueResult = rgbToHexColor(300, 0, 0)
        // Assert
        expect(nonNumericRedValueResult).to.be.undefined;
        expect(negativeRedValueResult).to.be.undefined;
        expect(overLimitRedValueResult).to.be.undefined;
    })

    it('should return undefined if green value is invalid', () => {
        // Arrange
        // Act
        const nonNumericGreenValueResult = rgbToHexColor(0, '200' ,0)
        const negativeGreenValueResult = rgbToHexColor(0, -13, 0)
        const overLimitGreenValueResult = rgbToHexColor(0, 300, 0)
        // Assert
        expect(nonNumericGreenValueResult).to.be.undefined;
        expect(negativeGreenValueResult).to.be.undefined;
        expect(overLimitGreenValueResult).to.be.undefined;
    })

    it('should return undefined if blue value is invalid', () => {
        // Arrange
        // Act
        const nonNumericBlueValueResult = rgbToHexColor(0, 0, '200')
        const negativeBlueValueResult = rgbToHexColor(0, 0, -13)
        const overLimitBlueValueResult = rgbToHexColor(0, 0, 300)
        // Assert
        expect(nonNumericBlueValueResult).to.be.undefined;
        expect(negativeBlueValueResult).to.be.undefined;
        expect(overLimitBlueValueResult).to.be.undefined;
    })

    it('should return a correct hex if a correct rgb is given', () => {
        // Arrange
        const red = 22;
        const green = 34;
        const blue = 26;
        // Act
        const result = rgbToHexColor(red, green, blue)
        // Assert
        expect(result).to.equals('#16221A');
    })

    it('should return a correct hex if a min values are given', () => {
        // Arrange
        const red = 0;
        const green = 0;
        const blue = 0;
        // Act
        const result = rgbToHexColor(red, green, blue)
        // Assert
        expect(result).to.equals('#000000');
    })

    it('should return a correct hex if a max values are given', () => {
        // Arrange
        const red = 255;
        const green = 255;
        const blue = 255;
        // Act
        const result = rgbToHexColor(red, green, blue)
        // Assert
        expect(result).to.equals('#FFFFFF');
    })
})