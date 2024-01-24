function solve(commands) {
    'use strict';

    let movies = [];

    for (const command of commands) {
        let tokens = command.split(' ');

        if (tokens[0] === 'addMovie') {
            let movieName = tokens.slice(1).join(' ');
            movies.push({
                name: movieName
            });
        }
        else {
            let movieName = tokens[0];
            let movie = movies.find(movie => movie.name === movieName);
    
            if (movie) {
                if (tokens [1] === 'directedBy') {
                    movie.director = tokens.slice(2).join(' ');
                }
                else if (tokens[1] === 'onDate') {
                    movie.date = tokens.slice(2).join(' ');
                }
            }
        }
    }

    // Filter out movies without complete information
    let validMovies = movies.filter(movie => movie.name && movie.director && movie.date);

    // Print the result in JSON format
    validMovies.forEach(movie => {
        console.log(JSON.stringify(movie));
    });
}