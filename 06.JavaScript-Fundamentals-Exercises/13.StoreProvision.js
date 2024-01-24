function solve(currentStock, orderedStock) {
    'use strict';

    const storeStock = {};

    for (let i = 0; i < currentStock.length; i+=2) {
        const stockName = currentStock[i];
        const stockAmount = parseInt(currentStock[i + 1], 10);

        storeStock[stockName] = stockAmount;
    }

    for (let i = 0; i < orderedStock.length; i+=2) {
        const stockName = orderedStock[i];
        const stockAmount = parseInt(orderedStock[i + 1], 10);

        if (storeStock[stockName] !== undefined) {
            storeStock[stockName] += stockAmount;
        }
        else {
            storeStock[stockName] = stockAmount;
        }
    }

    Object.keys(storeStock).forEach((currentItemName) => console.log(`${currentItemName} -> ${storeStock[currentItemName]}`))
}