import { createTags, formatTags } from './util/tags.js';
import { formatStartTime } from './util/formatStartTime.js';
import { showLoginSignupModal } from './util/showUnauthorizedLoginModal.js';
import { addEventToHistory } from './api/history/addEventToHistory.js';
import { showToast } from './util/toast.js';
import { buildEventCard } from './ui/buildEventCard.js';
import { getEvents } from './api/events/getEvents.js';
import { getEventIsFavorited } from './api/favorites/getEventIsFavorited.js';
import { removeEventFromFavorites } from './api/favorites/removeEventFromFavorites.js';
import { addEventToFavorites } from './api/favorites/addEventToFavorites.js';

async function setModalContent(eventName, eventDescription, eventStartTime, eventAddress, eventTags) {
    const modal = document.getElementById('event-details-modal');
    document.getElementById('modal-title').innerHTML = eventName;
    document.getElementById('modal-description').innerHTML = eventDescription;
    document.getElementById('modal-address').innerHTML = eventAddress;
    document.getElementById('modal-date').innerHTML = eventStartTime;
    const tagsContainer = document.getElementById('modal-tags-container');
    tagsContainer.innerHTML = '';

    console.log(eventTags)

    if (eventTags && eventTags.length > 0) {
        await formatTags(eventTags, tagsContainer);
    } else {
        const tagEl = document.createElement('span');
        tagEl.classList.add('tag');
        tagEl.textContent = "No tags available";
        tagsContainer.appendChild(tagEl);
    }
}

/**
 * Display events.
 * Events is an array of event objects returned from the API
 * @param {any} events
 */
async function displayEvents(events) {
    let eventsContainer = document.getElementById('events-container')
    eventsContainer.innerHTML = ''; // Clear the container
    let eventCardTemplate = document.getElementById('event-card-template')

    if (!events || events.length === 0) {
        document.getElementById('no-events-section')?.classList.toggle('hidden', false); // Show the no events section
        return;
    } else {
        document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    }

    const eventTags = events.map(event => event.eventTags).flat().filter(tag => tag)
    await createTags(eventTags);

    for (let eventInfo of events) {
        let newEventCard = eventCardTemplate.content.cloneNode(true);

        let eventApiBody = {
            ApiEventID: eventInfo.eventID || "No ID available",
            EventDate: eventInfo.eventStartTime || "No date available",
            EventName: eventInfo.eventName || "No name available",
            EventDescription: eventInfo.eventDescription || "No description available",
            EventLocation: eventInfo.full_Address || "No location available",
        };

        // TODO: add validation
        let eventCardProps = {
            img: eventInfo.eventThumbnail,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventStartTime),
            city: eventInfo.full_Address.split(',')[1],
            state: eventInfo.full_Address.split(',')[2],
            tags: await formatTags(eventInfo.eventTags),
            favorited: await getEventIsFavorited(eventInfo.eventID),
            onPressFavorite: () => onPressFavorite(eventApiBody, eventCardProps.favorited)
        }

        buildEventCard(newEventCard, eventCardProps);
        eventsContainer.appendChild(newEventCard);
    }
}

/**
 * Takes in an apiEventId, and a favorite status, and updates the favorite status of the event via http
 * 
 * eventApiBody: {
        ApiEventID: eventInfo.eventID || "No ID available",
        EventDate: eventInfo.eventStartTime || "No date available",
        EventName: eventInfo.eventName || "No name available",
        EventDescription: eventInfo.eventDescription || "No description available",
        EventLocation: eventInfo.full_Address || "No location available",
    };
 * 
 * @async
 * @function onPressFavorite
 * @param {object} eventApiBody
 * @param {any} favorited
 * @returns {Promise<void>}
 */
async function onPressFavorite(eventInfo, favorited) {
    if (favorited) {
        removeEventFromFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event unfavorited!');
    } else {
        addEventToFavorites(eventInfo).catch((error) => {
            // TODO: check that it is an unauthorized error
            // Unauthorized, show the login/signup modal
            showLoginSignupModal();
        })
        showToast('Event favorited!');
    }
}

// Fetch event data and display it
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (document.getElementById('events-container')) {
        getEvents("Events in Monmouth, Oregon").then(displayEvents)
    }

    document.getElementById('search-event-button').addEventListener('click', () => {
        getEvents(document.getElementById('search-event-input').value).then(displayEvents)
    });

    document.getElementById('search-event-input').addEventListener('keyup', function (event) {
        if (event.key === 'Enter') {
            getEvents(document.getElementById('search-event-input').value).then(displayEvents)
        }
    });
});
