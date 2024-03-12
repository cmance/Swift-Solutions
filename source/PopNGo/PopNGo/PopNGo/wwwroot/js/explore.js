import { createTags, formatTags } from './eventsAPI.js';
import { formatStartTime } from './util/formatStartTime.js';
import { showLoginSignupModal } from './util/showUnauthorizedLoginModal.js';
import { addEventToHistory } from './api/history/addEventToHistory.js';
import { showToast } from './util/toast.js';
import { buildEventCard } from './ui/buildEventCard.js';
import { getEvents } from './api/events/getEvents.js';
import { getEventIsFavorited } from './api/favorites/getEventIsFavorited.js';

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

        // TODO: add validation
        let eventCardProps = {
            img: eventInfo.eventThumbnail,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventStartTime),
            city: eventInfo.full_Address.split(',')[1],
            state: eventInfo.full_Address.split(',')[2],
            tags: await formatTags(eventInfo.eventTags),
            favorited: await getEventIsFavorited(eventInfo.eventID),
        }

        buildEventCard(newEventCard, eventCardProps)
        eventsContainer.appendChild(newEventCard)
    }
}


// Function to display events
async function displayEvents5(events) {
    console.log(events)
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    const container = document.getElementById('eventsContainer');
    if (!container) {
        console.error('Container element #eventsContainer not found.');
        return;
    } else
        container.innerHTML = ''; // Clear the container

    if (!events || events.length === 0) {
        document.getElementById('no-events-section')?.classList.toggle('hidden', false); // Show the no events section
        return;
    }

    await createTags(events);

    processArray(events, async event => {
        // Create elements for each event and append them to the container
        const heart = new Image();
        heart.alt = 'Favorite/Unfavorite Event';
        heart.classList.add('heart-position');
        heart.style.cursor = 'pointer'; //might want to add this to css if possible, but i dont think its necessary

        let isFavorite;

        let eventInfo = {
            ApiEventID: event.eventID || "No ID available",
            EventDate: event.eventStartTime || "No date available",
            EventName: event.eventName || "No name available",
            EventDescription: event.eventDescription || "No description available",
            EventLocation: event.full_Address || "No location available",
        };

        const updateFavoriteStatus = () => {
            fetch(isFavorite ? "/api/FavoritesApi/RemoveFavorite" : "/api/FavoritesApi/AddFavorite", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(eventInfo)
            })
                .then(async response => {
                    if (response.status === 401) {
                        // Unauthorized, show the login/signup modal
                        showLoginSignupModal();
                        throw new Error('Unauthorized');
                    }
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    const text = await response.text();
                    return text ? JSON.parse(text) : {};
                })
                .then(() => {
                    // Update the favorite status and the image source
                    isFavorite = !isFavorite;
                    heart.src = isFavorite ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';
                
                    // Show a toast notification
                    showToast(isFavorite ? 'Event favorited!' : 'Event unfavorited!');
                })
        };

        fetch(`/api/FavoritesApi/IsFavorite?eventId=${event.eventID}`)
            .then(response => response.json())
            .then(favoriteStatus => {
                isFavorite = favoriteStatus;
                heart.src = isFavorite ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg'; //THIS IS WHERE THE IMAGE PATHS ARE HARDCODED
                heart.addEventListener('click', updateFavoriteStatus);
            });

        const eventEl = document.createElement('div');
        eventEl.classList.add('event');

        const name = document.createElement('h2');
        name.textContent = event.eventName || 'Event Name Not Available';

        const dateTime = document.createElement('p');
        dateTime.textContent = `Start: ${event.eventStartTime || 'Unknown Start Time'}, End: ${event.eventEndTime || 'Unknown End Time'}`;

        const location = document.createElement('p');
        location.textContent = event.full_Address || 'Location information not available';

        const description = document.createElement('p');
        description.textContent = event.eventDescription || 'No description available.';

        const thumbnail = new Image();
        thumbnail.src = event.eventThumbnail || 'https://yourdomain.com/path/to/default-thumbnail.png';
        thumbnail.alt = event.eventName || 'Event Thumbnail';

        const tags = document.createElement('div');
        tags.classList.add('tags');

        if (event.eventTags && event.eventTags.length > 0) {
            await formatTags(event.eventTags, tags);
        } else {
            const tagEl = document.createElement('span');
            tagEl.classList.add('tag');
            tagEl.textContent = "No tags available";
            tags.appendChild(tagEl);
        }

        eventEl.appendChild(name);
        eventEl.appendChild(dateTime);
        eventEl.appendChild(location);
        eventEl.appendChild(description);
        eventEl.appendChild(thumbnail);
        eventEl.appendChild(tags);
        eventEl.appendChild(heart);

        eventEl.onclick = () => {
            setModalContent(event.eventName, event.eventDescription, formatStartTime(event.eventStartTime), event.full_Address, event.eventTags);
            const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
            modal.show();
            addEventToHistory(eventInfo);
        }

        container.appendChild(eventEl);
    });
}


// Fetch event data and display it
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('no-events-section')?.classList.toggle('hidden', true); // Hide the no events section
    document.getElementById('searching-events-section')?.classList.toggle('hidden', true); // Hide the searching events section

    if (document.getElementById('events-container')) {
        getEvents("Events in Monmouth, Oregon").then(displayEvents)
    }

    document.getElementById('search-event-button').addEventListener('click', getEvents("").then(displayEvents));

    document.getElementById('search-event-input').addEventListener('keyup', function (event) {
        if (event.key === 'Enter') {
            getEvents("").then(displayEvents)
        }
    });
});
