function solve(array, numOfRotations) {
    'use strict';
    
    for (let i = 0; i < numOfRotations; i++){
        const firstElement = array.shift(); // Modify the whole array
        array.push(firstElement);
    }

    console.log(array.join(' '));
}