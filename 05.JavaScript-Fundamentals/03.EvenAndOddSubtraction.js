function solve(input) {
    'use strict';

    let evenNums = 0;
    let oddNums = 0;

    for (let i = 0; i < input.length; i++) {
        let currentNumber = Number(input[i]);

        if (currentNumber % 2 === 0) {
            evenNums += currentNumber;
        }
        else {
            oddNums += currentNumber;
        }
    }

    console.log(evenNums - oddNums);
}