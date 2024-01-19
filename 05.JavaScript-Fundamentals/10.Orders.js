function solve(product, quantity) {
    'use strict';
    let price = 0;

    switch (product) {
        case 'coffee':
            price = quantity * 1.50; 
            break;
        case 'coke':
            price = quantity * 1.40;
            break;
        case 'water':
            price = quantity * 1.00;
            break;
        case 'snacks':
            price = quantity * 2.00;
            break;
        default:
            console.log('Error!')
            break;
    }

    console.log(price.toFixed(2));
}