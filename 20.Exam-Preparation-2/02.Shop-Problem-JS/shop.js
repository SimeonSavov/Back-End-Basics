function solve(input) {
    'use strict';

    const numberOfProducts = parseInt(input[0], 10);
    const allProducts = input.slice(1, numberOfProducts + 1);

    const allCommands = input.slice(numberOfProducts + 1);

    for (let i = 0; i < allCommands.length; i++) {
        const rawParams = allCommands[i].split(' ');
        const commandName = rawParams[0];

        if (commandName === 'Sell') {
            const soldProduct = allProducts.shift();
            console.log(`${soldProduct} product sold!`);
        }
        else if (commandName === 'Add') {
            const productToAdd = allCommands[i].slice(4);

            if (!productToAdd) {
                continue;
            }

            allProducts.push(productToAdd);
        }
        else if (commandName === 'Swap') {
            const startIndex = parseInt(rawParams[1], 10);
            const endIndex = parseInt(rawParams[2], 10);

            if (isNaN(startIndex) || startIndex < 0 || startIndex >= allProducts.length) {
                continue;
            }

            if (isNaN(endIndex) || endIndex < 0 || endIndex >= allProducts.length) {
                continue;
            }

            const productOnStartIndex = allProducts[startIndex];

            allProducts[startIndex] = allProducts[endIndex];

            allProducts[endIndex] = productOnStartIndex;

            console.log('Swapped!');
        }
        else if (commandName === 'End') {
            break;
        }
    }

    if (allProducts.length) {
        console.log(`Products left: ${allProducts.join(', ')}`);
    }
    else {
        console.log('The shop is empty');
    }
}