import { validateObject } from "../validation.js";
// Props object example:
// {
//     img: String link,
//     title: String,
//     date: DateTime,
//     city: String,
//     state: String,
//     tags: Array[String],
// }


/**
 * Takes in the event card element and the event info props and updates the event card element
 * Props:
{
     img: String link,
     title: String,
     date: DateTime,
     city: String,
     state: String,
     tags: Array[Tag],
     distance: Number,
     distanceUnit: String,
     bookmarkListNames: Array[String]
     onPressBookmarkList: (bookmarkListName: String) => void (optional),
     onPressEvent: Function
     onPressDelete: Function (optional) If not provided, the delete button will be removed
 }

 Tag: {
    tagName: String,
    tagTextColor: String,
    tagBackgroundColor: String,
 }

 * @function buildEventCard
 * @param {HTMLElement} eventCardElement 
 * @param {Object} props 
 */

export const buildEventCard = (eventCardElement, props) => {
    // Set the event card click event
    eventCardElement.querySelector('#event-card-container').addEventListener('click', () => {
        props.onPressEvent();
    });

    // Set the image
    if (props.img === null || props.img === undefined) {
        eventCardElement.querySelector('#event-card-image').src = '/media/images/placeholder_event_card_image.png';
    } else {
        eventCardElement.querySelector('#event-card-image').src = props.img;
    }

    // Set the title
    eventCardElement.querySelector('#event-card-title').textContent = props.title;

    // Set the date
    eventCardElement.querySelector('#day-number').textContent = props.date.getDate();
    eventCardElement.querySelector('#month').textContent = props.date.toLocaleString('default', { month: 'short' });

    // Set the distance
    if (props.distance !== null) {
        eventCardElement.querySelector('#distance-number').textContent = props.distance;
        eventCardElement.querySelector('#distance-unit').textContent = props.distanceUnit;
    } else {
        eventCardElement.querySelector('#distance').remove();
    }

    // Set the location
    eventCardElement.querySelector('#event-card-location').textContent = props.state
    ? `${props.city}, ${props.state}` // If state is provided, display the city and state
    : props.city; // If state is not provided, only display the city

    // Fill the bookmark button dropdown with bookmark items
    const bookmarkDropdownMenu = eventCardElement.querySelector('.dropdown-menu');
    bookmarkDropdownMenu.innerHTML = ''

    if (!props.bookmarkListNames || !props.onPressBookmarkList || props.bookmarkListNames.length === 0) {
        // If no bookmark list names are provided, remove the dropdown menu
        eventCardElement.querySelector('#event-card-bookmark-button').remove();
    } else {
        props.bookmarkListNames.forEach(bookmarkListName => {
            const bookmarkItem = document.createElement('a');
            bookmarkItem.classList.add('dropdown-item');
            bookmarkItem.textContent = bookmarkListName;
            bookmarkItem.addEventListener('click', (event) => {
                // Prevent the event from bubbling up (stop the event from triggering the card click event)
                if (event && event.stopPropagation) event.stopPropagation();
                props.onPressBookmarkList(bookmarkListName);
            });
            bookmarkDropdownMenu.appendChild(bookmarkItem);
        });

        // Prevent bubbling up of the click event on the bookmark button
        eventCardElement.querySelector('#event-card-bookmark-button').addEventListener('click', (event) => {
            if (event && event.stopPropagation) event.stopPropagation();
        });
    }

    // Set the delete button click event
    if (props.onPressDelete) {
        eventCardElement.querySelector('.event-card-delete-icon-container').addEventListener('click', (event) => {
            event.stopPropagation();
            props.onPressDelete();
        });
    } else {
        eventCardElement.querySelector('.event-card-delete-icon-container').remove();
    }

    // Set the tags:
    const tagsElement = eventCardElement.querySelector('#event-card-tags-container');
    tagsElement.innerHTML = '';
    props.tags.forEach(tag => {
        const tagEl = document.createElement('span');
        tagEl.classList.add('event-tag');
        tagEl.classList.add('rounded-pill');
        tagEl.textContent = tag.tagName;
        tagEl.style.color = tag.tagTextColor;
        tagEl.style.backgroundColor = tag.tagBackgroundColor;
        tagsElement.appendChild(tagEl);
    });
}


/**
 * Takes in event card props and returns a boolean indicating if the props are valid
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildEventCardProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        img: x => (typeof x === 'string' || x === undefined || x === null),
        title: x => typeof x === 'string',
        date: x => x instanceof Date,
        city: x => typeof x === 'string',
        state: x => typeof x === 'string',
        tags: x => Array.isArray(x),
        distance: x => typeof x === 'number' || x === null,
        distanceUnit: x => typeof x === 'string' || x === null,
        bookmarkListNames: x => Array.isArray(x) || x === undefined || x === null,
        onPressBookmarkList: x => (typeof x === 'function' || x === undefined || x === null),
        onPressEvent: x => (typeof x === 'function' || x === undefined || x === null),
        onPressDelete: x => (typeof x === 'function' || x === undefined || x === null),
    }
    return validateObject(data, schema).length === 0;
}
