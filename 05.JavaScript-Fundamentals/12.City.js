function solve(object) {
    'use strict';

    for (let key in object) {
        console.log(`${key} -> ${object[key]}`);
    }
}