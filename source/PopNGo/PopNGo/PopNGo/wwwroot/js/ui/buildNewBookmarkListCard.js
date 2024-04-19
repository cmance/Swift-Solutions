import { validateObject } from "../validation.js";

/**
 * Takes in the the new bookmark list card element and props and builds the card
 * Props:
 * {
 *   onClickCreateBookmarkList: (listName: String) => function,
 * }
 * @function buildEventCard
 * @param {HTMLElement} bookmarkListCardElement 
 * @param {Object?} props 
 */
export const buildNewBookmarkListCard = (bookmarkListCardElement, props) => {
    if (!validateNewBuildBookmarkListCardProps(props)) {
        throw new Error('Invalid props');
    }
    
    
    // Wire up the create bookmark list button
    const createBookmarkListButton = bookmarkListCardElement.querySelector('.saveNewBookmarkListButton');
    const bookmarkNameInput = bookmarkListCardElement.querySelector('#new-bookmark-list-card-title-input');
    
    createBookmarkListButton.addEventListener('click', () => {
        const bookmarkListName = bookmarkNameInput.value;
        props.onClickCreateBookmarkList(bookmarkListName);
    });

    // Set the create bookmark list button to disabled if the input is empty
    createBookmarkListButton.disabled = bookmarkNameInput.value === '';
    bookmarkNameInput.addEventListener('input', () => {
        createBookmarkListButton.disabled = bookmarkNameInput.value === '';
    });
}


/**
 * Takes in the new bookmark list card props and validates them
 * @param {any} data
 * @returns {boolean}
 */
export function validateNewBuildBookmarkListCardProps(data) {
    if (data === undefined || data === null) {
        return true;
    }

    const schema = {
        onClickCreateBookmarkList: x => (typeof x === 'function' || x === undefined || x === null),
    }

    return validateObject(data, schema).length === 0;
}
