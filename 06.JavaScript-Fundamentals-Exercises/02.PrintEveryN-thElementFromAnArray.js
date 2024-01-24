function solve(arrayOfStrings, step) {
    'use strict';
    
    const result = [];

    for (let i = 0; i <= arrayOfStrings.length; i+=step){
        result.push(arrayOfStrings[i]);
    }

    return result;
}