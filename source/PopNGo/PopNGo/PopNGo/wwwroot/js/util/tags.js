import { capitalizeFirstLetter } from "./capitalizeFirstLetter.js";

/**
 * Takes in a string tag and returns the tag name formatted for display
 * 
 * Example:
 * sports_and_stuff -> Sports and stuff
 * @param {String} tag - The string tag name
 * @returns {String} The formatted tag name
 */
export function formatTagName(tag) {
    return capitalizeFirstLetter(tag).replace(/-|_/g, ' ');
}
