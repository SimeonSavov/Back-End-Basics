function dvd_collection(dvds) {
    'use strict';

    const numberOfDvds = parseInt(dvds[0], 10);
    const allDVDs = dvds.slice(1, numberOfDvds + 1);

    const allCommands = dvds.slice(numberOfDvds + 1);

    for (let i = 0; i < allCommands.length; i++) {
        const rawParams = allCommands[i].split(' ');
        const commandName = rawParams[0];

        if (commandName === 'Watch') {
            const watchedDVD = allDVDs.shift();
            console.log(`${watchedDVD} DVD watched!`);
        }
        else if (commandName === 'Buy') {
            const dvdToBuy = rawParams.slice(1).join(' ');

            if (!dvdToBuy) {
                continue;
            }

            allDVDs.push(dvdToBuy);
        }
        else if (commandName === 'Swap') {
            const startIndex = parseInt(rawParams[1], 10);
            const endIndex = parseInt(rawParams[2], 10);

            if (isNaN(startIndex) || startIndex < 0 || startIndex >= allDVDs.length ||
                isNaN(endIndex) || endIndex < 0 || endIndex >= allDVDs.length) {
                continue;
            }

            const dvdOnStartIndex = allDVDs[startIndex];
            allDVDs[startIndex] = allDVDs[endIndex];
            allDVDs[endIndex] = dvdOnStartIndex;

            console.log('Swapped!');
        }
        else if (commandName === 'Done') {
            break;
        }
    }

    if (allDVDs.length) {
        console.log(`DVDs left: ${allDVDs.join(', ')}`);
    }
    else {
        console.log('The collection is empty');
    }
}