function solve(namesArr) {
    'use strict';

    namesArr.sort((a, b) => a.localeCompare(b)); // with sort() - the array is modify by ASC.

    for (let i = 1; i <= namesArr.length; i++) {
        console.log(`${i}.${namesArr[i - 1]}`);
    }
}