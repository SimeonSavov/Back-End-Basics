function solve(object) {
    'use strict';

    let uniqueNames = {};
    object.forEach(element => {
        let keyValuePair = element.split(' ');
        let key = keyValuePair[0]; // Name
        let value = keyValuePair[1]; // Phone Number
        
        uniqueNames[key] = value;
    });

    for (let key in uniqueNames) {
        console.log(`${key} -> ${uniqueNames[key]}`);
    }
}