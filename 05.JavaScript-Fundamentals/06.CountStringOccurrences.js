function solve(text, searchWord) {
    'use strict';

    let words = text.split(' ');
    let counter = 0;

    for (let word of words) {
        if (word === searchWord) {
            counter++;
        }
    }

    console.log(counter);
}