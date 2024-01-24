function solve(input) {
    'use strict';

    let parking = new Set();

    for (const data of input) {
        let[direction, carNumber] = data.split(', ');

        if (direction === 'IN') {
            parking.add(carNumber);
        }
        else if (direction === 'OUT') {
            parking.delete(carNumber);
        }
    }
    

    let sortedCars = Array.from(parking).sort();

    if (sortedCars.length === 0) {
        console.log('Parking Lot is Empty')
    }
    else {
        for (const car of sortedCars) {
            console.log(car);
        }
    }
}