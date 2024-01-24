function solve(heroesData) {
    'use strict';

    let heroes = [];

    for (let data of heroesData) {
        let tokens = data.split(' / ');
        let heroName = tokens[0];
        let heroLevel = parseInt(tokens[1], 10);
        let heroItems = [];

        if (tokens.length > 2) {
            heroItems = tokens[2]. split(', ');
        }
        
        let hero = {
            name: heroName,
            level: heroLevel,
            items: heroItems
        };

        heroes.push(hero);
    }

    heroes.sort((a, b) => a.level - b.level);

    for (const hero of heroes) {
        console.log(`Hero: ${hero.name}`);
        console.log(`level => ${hero.level}`);
        console.log(`items => ${hero.items.join(', ')}`);
    }
}