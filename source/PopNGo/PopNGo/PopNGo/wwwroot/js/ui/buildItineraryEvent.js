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
     onDelete: Function
 }

 Tag: {
    name: String,
    textColor: String,
    backgroundColor: String,
 }

 * @function buildItineraryEvent
 * @param {Object} props 
 */

export const buildItineraryEvent = (props) => {
    // Clone the template
    const itineraryEventElement = document.getElementById("itinerary-event-template").content.cloneNode(true);

    const eventContainer = itineraryEventElement.querySelector('.single-timeline-area');
    eventContainer.setAttribute('data-event-id', props.apiEventID);
    eventContainer.setAttribute('data-itinerary-id', props.itineraryId);

    // Set the image
    const eventImage = itineraryEventElement.querySelector('#event-image');
    eventImage.alt = `${props.title} Image`;

    if (props.img === null || props.img === undefined) {
        eventImage.src = '/media/images/placeholder_event_card_image.png';
    } else {
        eventImage.src = props.img;
    }

    // Set the title
    itineraryEventElement.getElementById('event-title').textContent = props.title;

    // Set the date
    const time = props.date.toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    itineraryEventElement.getElementById('event-date').textContent = props.date.toLocaleString('default', { month: 'long' }) + " " + props.date.getDate() + ", " + props.date.getFullYear() + " at " + time;

    // Set the location
    itineraryEventElement.getElementById('event-location').textContent = props.address;

    // Set the tags:
    const tagsElement = itineraryEventElement.getElementById('event-tags-container');
    tagsElement.innerHTML = '';
    
    props.tags?.forEach(tag => {
        const tagEl = document.createElement('span');
        tagEl.classList.add('event-tag');
        tagEl.classList.add('rounded-pill');
        tagEl.textContent = formatTagName(tag.name);
        tagEl.style.color = tag.textColor;
        tagEl.style.backgroundColor = tag.backgroundColor;
        tagsElement.appendChild(tagEl);
    });

    const reminderSelect = itineraryEventElement.getElementById('reminder-time-select');
    const reminderTimeDiv = itineraryEventElement.getElementById('reminder-time-div');
    reminderSelect.value = props.reminderTime || 'custom';
    reminderSelect.addEventListener('change', () => {
        if (reminderSelect.value === 'custom') {
            reminderTimeDiv.style.display = 'block';
        } else {
            reminderTimeDiv.style.display = 'none';
        }
    });

    const customReminderTime = itineraryEventElement.getElementById('reminder-time');
    customReminderTime.value = props.reminderCustomTime || props.date.toString();
    const originalReminderTime = props.reminderCustomTime || props.date.toString();

    const saveReminderButton = itineraryEventElement.getElementById('save-reminder-button');
    saveReminderButton.setAttribute('data-event-id', props.apiEventID);
    saveReminderButton.setAttribute('data-itinerary-id', props.itineraryId);
    saveReminderButton.addEventListener('click', () => props.onSaveReminder(customReminderTime, originalReminderTime, reminderSelect.value, customReminderTime.value));

    // Set the delete button
    const deleteButton = itineraryEventElement.getElementById('delete-event-button');
    deleteButton.setAttribute('data-event-id', props.apiEventID);
    deleteButton.setAttribute('data-itinerary-id', props.itineraryId);
    deleteButton.addEventListener('click', () => props.onDelete());

    const eventRecommendations = itineraryEventElement.getElementById('event-recommendations');
    eventRecommendations.setAttribute('data-event-id', props.apiEventID);
    eventRecommendations.setAttribute('data-itinerary-id', props.itineraryId);
    eventRecommendations.setAttribute('data-latitude', props.latitude || '');
    eventRecommendations.setAttribute('data-longitude', props.longitude || '');

    const eventRecommendationsHotel = itineraryEventElement.getElementById('hotels-button-${eventData.latitude}-${eventData.longitude}');
    eventRecommendationsHotel.id = `hotels-button-${props.latitude}-${props.longitude}`;

    const eventRecommendationsFood = itineraryEventElement.getElementById('food-button-${eventData.latitude}-${eventData.longitude}');
    eventRecommendationsFood.id = `food-button-${props.latitude}-${props.longitude}`;

    const eventRecommendationsCoffee = itineraryEventElement.getElementById('coffee-button-${eventData.latitude}-${eventData.longitude}');
    eventRecommendationsCoffee.id = `coffee-button-${props.latitude}-${props.longitude}`;

    const eventSuggestions = itineraryEventElement.getElementById('suggestions-container-${eventData.latitude}-${eventData.longitude}');
    eventSuggestions.id = `suggestions-container-${props.latitude}-${props.longitude}`;

    return itineraryEventElement;
}


/**
 * Takes in event card props and returns a boolean indicating if the props are valid
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildItineraryEventProps(data) {
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
        onDelete: x => (typeof x === 'function' || x === undefined || x === null),
    }
    return validateObject(data, schema).length === 0;
}
