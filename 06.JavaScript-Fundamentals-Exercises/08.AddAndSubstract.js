function solve(firstNum, secondNum, thirdNum) {
    'use strict';

    // function sum(firstNum, secondNum) {
    //     return firstNum + secondNum;
    // }

    const sum = (first, second) => first + second;
    const substract = (first, second) => first - second;

    const result = substract(sum(firstNum, secondNum), thirdNum);

    console.log(result);
}