import { validateObject } from "../validation.js";

/**
 * Takes in the the bookmark list card element and props and builds the card
 * 
BookmarkListCardProps:
{
    bookmarkListName: String,
    eventQuantity: Number,
    image: String | null,
    onClick: Function,
    onClickDelete: Function,
 }
 * @function buildEventCard
 * @param {HTMLElement} bookmarkListCardElement 
 * @param {BookmarkListCardProps} props 
 */
export const buildBookmarkListCard = (bookmarkListCardElement, props) => {
    if (!validateBuildBookmarkListCardProps(props)) {
        throw new Error('Invalid props');
    }

    const { bookmarkListName, eventQuantity, image, onClick, onClickDelete } = props;

    // Set the bookmark list name
    bookmarkListCardElement.querySelector('.bookmarkListCardTitleText').textContent = bookmarkListName;

    // Set the event quantity
    bookmarkListCardElement.querySelector('.bookmarkListCardQuantityText').textContent = eventQuantity;

    // Set the bookmark list background image
    if (typeof props.image === 'string') {
        bookmarkListCardElement.querySelector('.bookmarkListCard').style.backgroundImage = `url(${image})`;
    }

    // Add the event listener
    bookmarkListCardElement.querySelector('.bookmarkListCard').addEventListener('click', onClick);

    // Add the delete button event listener
    bookmarkListCardElement.querySelector('.bookmarkListCardDeleteButton').addEventListener('click', onClickDelete);
}


/**
 * Takes in the bookmark list card props and validates them
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildBookmarkListCardProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        bookmarkListName: x => typeof x === 'string',
        eventQuantity: x => typeof x === 'number',
        image: x => (typeof x === 'string' || x === null || x === undefined),
        onClick: x => (typeof x === 'function' || x === undefined || x === null),
        onClickDelete: x => (typeof x === 'function' || x === undefined || x === null),
    }

    return validateObject(data, schema).length === 0;
}
