function solve(arrayOfEmployees) {
    'use strict';

    const employeeData = [];

    for (const employeeRaw of arrayOfEmployees) {
        employeeData.push({
            name: employeeRaw,
            personalNum: employeeRaw.length
        })
    }

    employeeData.forEach((employee) => console.log(`Name: ${employee.name} -- Personal Number: ${employee.personalNum}`));
}