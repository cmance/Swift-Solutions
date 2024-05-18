import { formatTagName } from "../util/tags.js";
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
    name: String,
    textColor: String,
    backgroundColor: String,
 }

 * @function buildItinerary
 * @param {Object} props 
 */

export const buildItinerary = (props) => {
    // Clone the template
    const itineraryElement = document.getElementById("itinerary-accordion-template").content.cloneNode(true);

    // Set the index
    itineraryElement.getElementById('heading${index}').id = `heading${props.index}`;
    
    const collapsableSection = itineraryElement.getElementById('collapse${index}');
    collapsableSection.id = `collapse${props.index}`;
    collapsableSection.setAttribute("aria-labelledby", `heading${props.index}`);
    collapsableSection.setAttribute("data-bs-parent", "#itinerary-accordion");

    const accordionHeaderBG = itineraryElement.getElementById('accordion-header-bg');
    accordionHeaderBG.setAttribute("data-bs-target", `#collapse${props.index}`);
    accordionHeaderBG.setAttribute("aria-controls", `#collapse${props.index}`);

    const deleteButton = itineraryElement.getElementById('itinerary-delete-button');
    deleteButton.setAttribute("data-itinerary-id", props.index);
    deleteButton.addEventListener('click', () => props.onDelete(deleteButton));

    // Set the title
    itineraryElement.getElementById('itinerary-title').textContent = props.title;

    return itineraryElement;
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
        city: x => typeof x === 'string' || x === undefined || x === null,
        state: x => typeof x === 'string' || x === undefined || x === null,
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
