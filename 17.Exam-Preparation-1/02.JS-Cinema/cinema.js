function solve(input) {
    'use strict';

    const numberOfMovies = parseInt(input[0], 10); //Count of movies

    const allMovies = input.slice(1, numberOfMovies + 1); // Get All Movies from the Array
    const allCommands = input.slice(numberOfMovies + 1)
    
    for (let i = 0; i < allCommands.length; i++) {
        const rawParams = allCommands[i].split(' ');
        const commandName = rawParams[0];

        if (commandName === 'Sell') {
            const soldMovie = allMovies.shift();
            console.log(`${soldMovie} ticket sold!`);
        }
        else if (commandName === 'Add') {
            const movieTitleToAdd = allCommands[i].slice(4);

            if (!movieTitleToAdd) {
                continue;
            }
            
            allMovies.push(movieTitleToAdd);
        }
        else if (commandName === 'Swap') {
            const firstIndex = parseInt(rawParams[1], 10);
            const secondIndex = parseInt(rawParams[2], 10);


            if (isNaN(firstIndex) || firstIndex < 0 || firstIndex >= allMovies.length) {
                continue;
            }

            if (isNaN(secondIndex) || secondIndex < 0 || secondIndex >= allMovies.length) {
                continue;
            }

            const movieOnFirstIndex = allMovies[firstIndex];

            allMovies[firstIndex] = allMovies[secondIndex];

            allMovies[secondIndex] = movieOnFirstIndex;

            console.log('Swapped!');
        }
        else if (commandName === 'End') {
            break;
        }
    }

    if (allMovies.length) {
        console.log(`Tickets left: ${allMovies.join(', ')}`);
    }
    else {
        console.log('The box office is empty');
    }
}