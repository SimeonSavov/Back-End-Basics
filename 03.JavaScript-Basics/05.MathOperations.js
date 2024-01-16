function solve(numOne, numTwo, operation) {
    let result;

    switch (operation) {
        case '+':
            result = numOne + numTwo;
            break;
        case '-':
            result = numOne - numTwo;
            break;
        case '*':
            result = numOne * numTwo;
            break;
        case '/':
            result = numOne / numTwo;
            break;
        case '%':
            result = numOne % numTwo;
            break;
        case '**':
            result = numOne ** numTwo;
            break;
        default:
            break;
    }

    console.log(result);
}