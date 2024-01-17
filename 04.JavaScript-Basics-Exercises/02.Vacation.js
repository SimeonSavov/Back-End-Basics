function solve(groupOfPeople, typeOfGroup, typeOfDay) {
    'use strict';

    let totalAmount = 0;

    if (typeOfGroup === 'Students') {
        if (typeOfDay === 'Friday') {
            totalAmount = groupOfPeople * 8.45;
        }
        else if (typeOfDay === 'Saturday') {
            totalAmount = groupOfPeople * 9.80;
        }
        else if (typeOfDay === 'Sunday') {
            totalAmount = groupOfPeople * 10.46;
        }

        if (groupOfPeople >= 30) {
            totalAmount *= 0.85;
        }
    }
    else if (typeOfGroup === 'Business') {
        if (typeOfDay === 'Friday') {
            totalAmount = groupOfPeople * 10.90;
        }
        else if (typeOfDay === 'Saturday') {
            totalAmount = groupOfPeople * 15.60;
        }
        else if (typeOfDay === 'Sunday') {
            totalAmount = groupOfPeople * 16;
        }

        if (groupOfPeople >= 100) {
            const pricePerNight = totalAmount / groupOfPeople;
            totalAmount = pricePerNight * (groupOfPeople - 10); 
        }
    }
    else if (typeOfGroup === 'Regular') {
        if (typeOfDay === 'Friday') {
            totalAmount = groupOfPeople * 15;
        }
        else if (typeOfDay === 'Saturday') {
            totalAmount = groupOfPeople * 20;
        }
        else if (typeOfDay === 'Sunday') {
            totalAmount = groupOfPeople * 22.50;
        }

        if (groupOfPeople >= 10 && groupOfPeople <= 20) {
            totalAmount *= 0.95;
        }
    }

    console.log(`Total price: ${totalAmount.toFixed(2)}`);
}