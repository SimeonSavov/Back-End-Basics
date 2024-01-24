function solve(array) {
    'use strict';

    array.sort((a, b) => a - b); // Sorting numbers from min to max

    const result = [];

    while (array.length > 0) {
        const firstElement = array.shift();
        const lastElement = array.pop();

        result.push(firstElement);

        if (lastElement !== undefined) {
            result.push(lastElement);
        }
    }

    return result;
}