function solve(year) {
    'use strict';

    const isLeapYear = (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
    const message = isLeapYear ? 'yes' : 'no';

    console.log(message);
}