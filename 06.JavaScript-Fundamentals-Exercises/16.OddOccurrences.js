function solve(input) {
    'use strict';

    let words = input.toLowerCase().split(' ');
    let wordOccurrences = {};

    for (const word of words) {
        if (wordOccurrences[word]) {
            wordOccurrences[word]++;
        }
        else {
            wordOccurrences[word] = 1;
        }
    }

    let result = [];
    for (const word in wordOccurrences) {
        if (wordOccurrences[word] % 2 !== 0) {
            result.push(word);
        }
    }

    return result.join(' ');
}