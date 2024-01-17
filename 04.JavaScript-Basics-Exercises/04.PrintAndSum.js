function solve(startNum, endNum) {
    'use strict';
    
    let message = '';
    let sum = 0;

    for (let i = startNum; i <= endNum; i++){
        sum += i;
        message += `${i} `;
    }

    console.log(message.trimEnd());
    console.log(`Sum: ${sum}`);
}
