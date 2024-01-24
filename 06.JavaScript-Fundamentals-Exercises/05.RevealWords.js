function solve(text, replaceWord) {
    'use strict';

    const seperatedText = text.split(', ');

    const repleacedWords = replaceWord.split(' ');

    let result = '';

    for (const word of repleacedWords) {
        if (word[0] === '*') {
            const correspondingWord = seperatedText.find(x => x.length === word.length)
            result += correspondingWord + ' ';
        } 
        else {
            result += word + ' ';
        }
    }

    console.log(result);
}