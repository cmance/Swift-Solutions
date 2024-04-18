import { createTags, formatTags } from './util/tags.js';
import { addEventToHistory } from './api/history/addEventToHistory.js';
import { buildEventCard, validateBuildEventCardProps } from './ui/buildEventCard.js';
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { getEvents } from './api/events/getEvents.js';
import { loadMapScript } from './util/loadMapScript.js';
import { getLocationCoords } from './util/getSearchLocationCoords.js';
import {
    loadSearchBar, getSearchQuery, toggleNoEventsSection, toggleSearchingEventsSection,
    setCity, setCountry, setState, toggleSearching
} from './util/searchBarEvents.js';
import { debounceUpdateLocationAndFetch } from './util/mapFetching.js';
import { getNearestCityAndStateAndCountry } from './util/getNearestCityAndStateAndCountry.js';
import { getBookmarkLists } from './api/bookmarkLists/getBookmarkLists.js';
import { onPressSaveToBookmarkList } from './util/onPressSaveToBookmarkList.js';
import { UnauthorizedError } from './util/errors.js';

let map = null;
let page = 0;
const pageSize = 10;

// Fetch event data and display it
// Call getLocation when the script is loaded
document.addEventListener("DOMContentLoaded", async function () {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {
            const { city, state, country } = await getNearestCityAndStateAndCountry(position.coords.latitude, position.coords.longitude);
            await loadSearchBarAndEvents(city, state, country);
        }, async function (error) {
            if (error.code === error.PERMISSION_DENIED) {
                const defaultCountry = "United States";
                const defaultCity = "Las Vegas";
                const defaultState = "Nevada";

                await loadSearchBarAndEvents(defaultCity, defaultState, defaultCountry);
            }
        });
    }
}, { once: true });


async function loadSearchBarAndEvents(city, state, country) {
    await loadSearchBar().then(async () => {
        await setCountry(country);
        await setState(state);
        setCity(city);
    });

    document.getElementById('next-page-button').addEventListener('click', nextPage);
    document.getElementById('previous-page-button').addEventListener('click', previousPage);

    if (document.getElementById('events-container')) {
        await searchForEvents();
    }

    document.getElementById('search-event-button').addEventListener('click', async function () { await searchForEvents(); });

    document.getElementById('search-event-input').addEventListener('keyup', async function (event) {
        if (event.key === 'Enter') {
            await searchForEvents();
        }
    });
}

/**
 * Takes in an event info object and adds it to the history via http and opens the event details modal
 * @async
 * @function onClickDetailsAsync
 * @param {string} eventInfo
 */
async function onClickDetailsAsync(eventInfo) {
    console.log("Event Info: ", eventInfo);
    const eventDetailsModalProps = {
        img: eventInfo.eventImage,
        title: eventInfo.eventName,
        description: (eventInfo.eventDescription ?? 'No description') + '...',
        date: new Date(eventInfo.eventDate),
        fullAddress: eventInfo.eventLocation,
        eventOriginalLink: eventInfo.eventOriginalLink,
        tags: await formatTags(eventInfo.eventTags),
        ticketLinks : eventInfo.ticketLinks,
        venueName: eventInfo.venueName,
        venuePhoneNumber: eventInfo.venuePhoneNumber,
        venueRating: eventInfo.venueRating,
        venueWebsite: eventInfo.venueWebsite
    }
    

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();

        addEventToHistory(eventInfo.apiEventID);
    };
}

/**
 * Get the next page of events and display them
 * @async
 * @function nextPage
 * @returns {Promise<void>}
 */
async function nextPage() {
    window.scrollTo(0, 0);
    page++;
    searchForEvents();

    document.getElementById('page-number').innerHTML = page + 1

    document.getElementById('previous-page-button').innerHTML = page;
    document.getElementById('next-page-button').innerHTML = page + 2;
    document.getElementById('previous-page-button').disabled = false;
}

/**
 * Get the previous page of events and display them
 * @async
 * @function previousPage
 * @returns {Promise<void>}
 */
async function previousPage() {
    if (page > 0) {
        window.scrollTo(0, 0);
        page--;
        searchForEvents();
        // set page number
        document.getElementById('page-number').innerHTML = page + 1;
        document.getElementById('previous-page-button').innerHTML = page;
        document.getElementById('next-page-button').innerHTML = page + 2;
        if (page === 0) {
            document.getElementById('previous-page-button').disabled = true;
        }
    }
}

export function getPaginationIndex() {
    return (page * pageSize);
}

/**
 * Display events.
 * Events is an array of event objects returned from the API
 * @param {any} events
 */
export async function displayEvents(events) {
    let eventsContainer = document.getElementById('events-container');
    eventsContainer.innerHTML = ''; // Clear the container
    let eventCardTemplate = document.getElementById('event-card-template');
    // let paginationDiv = document.getElementById('pagination');

    const eventTags = events.map(event => event.eventTags).flat().filter(tag => tag)
    await createTags(eventTags);

    // TODO: BUG this errors when not logged in
    let bookmarkLists = [];
    try {
        bookmarkLists = await getBookmarkLists();
    } catch (error) {
        if (error instanceof UnauthorizedError) {
            console.error("Unauthorized to get bookmark lists");
        } else {
            console.error("Error getting bookmark lists", error);
        }
    }

    for (let eventInfo of events) {
        let newEventCard = eventCardTemplate.content.cloneNode(true);

        let eventCardProps = {
            img: eventInfo.eventImage,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventDate),
            city: eventInfo.eventLocation.split(',')[1],
            state: eventInfo.eventLocation.split(',')[2],
            tags: await formatTags(eventInfo.eventTags),
            bookmarkListNames: bookmarkLists.map(bookmarkList => bookmarkList.title),
            onPressBookmarkList: (bookmarkListName) => onPressSaveToBookmarkList(eventInfo.apiEventID, bookmarkListName),
            onPressEvent: () => onClickDetailsAsync(eventInfo),
        }
        if (validateBuildEventCardProps(eventCardProps)) {
            buildEventCard(newEventCard, eventCardProps);
            eventsContainer.appendChild(newEventCard);
        }
    }

    // paginationDiv.style.display = 'flex'; //Display after events are loaded
}

/**
 * Run a search for events and display them
 * 
 * @async
 * @function searchForEvents
 * @returns {Promise<void>}
 */
async function searchForEvents() {
    let paginationDiv = document.getElementById('pagination');
    paginationDiv.style.display = 'none'; //Hide pagination while searching
    createPlaceholderCards();
    addMapLoadingSpinner();
    // console.log("search")
    toggleNoEventsSection(false);
    toggleSearchingEventsSection(true);
    toggleSearching();

    const events = await getEvents(getSearchQuery(), getPaginationIndex());
    removePlaceholderCards(); // Remove the placeholder cards as the API has returned

    console.log(events);
    toggleSearchingEventsSection(false); // Hide the searching events section
    if (!events || events.length === 0) {
        paginationDiv.style.display = 'none';
        toggleNoEventsSection(true);
        removeMapLoadingSpinner();
    } else {
        displayEvents(events);
        initMap(events);

        const country = document.getElementById('search-event-country').value;
        const state = document.getElementById('search-event-state').value;
        const city = document.getElementById('search-event-city').value;
        let mapCoords = await getLocationCoords(country, state, city);
        console.debug("Coords: ", mapCoords);
        if (map)
            map.setCenter(mapCoords ?? map.getCenter());

        paginationDiv.style.display = 'flex'; //Display after events are loaded
    }
    toggleSearching();
}

function createPlaceholderCards() {
    let eventsContainer = document.getElementById('events-container')
    eventsContainer.innerHTML = ''; // Clear the container
    let placeholderCardTemplate = document.getElementById('blank-placeholder-event-card-template')
    for (let i = 0; i < 10; i++) { // Replace 10 with the number of placeholder cards you want to create
        let newPlaceholderCard = placeholderCardTemplate.content.cloneNode(true);
        let card = newPlaceholderCard.querySelector('#event-card-container');
        card.classList.add('placeholder-style');
        let randomDuration = Math.random() + 1; // Generate a random number between 1 and 2
        card.style.animationDuration = `${randomDuration}s`;
        eventsContainer.appendChild(newPlaceholderCard);
    }
}

function removePlaceholderCards() {
    let eventsContainer = document.getElementById('events-container')
    while (eventsContainer.firstChild) {
        eventsContainer.removeChild(eventsContainer.firstChild);
    }
}

// Function to create the map and display events
window.initMap = async function (events) {
    var monmouth = { lat: 44.848, lng: -123.229 }; //Hardcoded Monmouth, Oregon coordinates for now

    // Check if the map already exists
    if (!map) {
        // If it doesn't, create a new one
        map = new google.maps.Map(document.getElementById('demo-map-id'), {
            center: monmouth,
            zoom: 10,
            minZoom: 10,
            maxZoom: 15
        });

        google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map, getPaginationIndex()));
    }

    events?.forEach(async eventInfo => {
        // Add a marker on the map for the event
        if (eventInfo) {
            const lat = eventInfo.latitude ? eventInfo.latitude : 44.848; //Hardcoded Monmouth, Oregon coordinates for now
            const lng = eventInfo.longitude ? eventInfo.longitude : -123.229; //Hardcoded Monmouth, Oregon coordinates for now
            const position = { lat, lng };
            const marker = new google.maps.Marker({
                position,
                map,
                title: eventInfo.eventName
            });

            removeMapLoadingSpinner();


            marker.addListener('click', async function () {
                onClickDetailsAsync(eventInfo);
            });

            google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map));
        }
    });

}

function addMapLoadingSpinner() {
    document.getElementById('loading-overlay').style.display = 'flex';
}

function removeMapLoadingSpinner() {
    document.getElementById('loading-overlay').style.display = 'none';
}

window.onload = async function () {

    createPlaceholderCards(); // Create placeholder cards while waiting for the API to return
    addMapLoadingSpinner();

    if (document.getElementById('demo-map-id')) {
        loadMapScript();
    }
}

// Save Location, Remove Location, Use Location
let savedLocations = JSON.parse(localStorage.getItem('savedLocations')) || [];
function displaySavedLocations() {
    const savedLocationsContainer = document.getElementById('saved-locations-container');
    savedLocationsContainer.innerHTML = '';

    savedLocations.forEach((location, index) => {
        const locationElement = document.createElement('div');
        locationElement.textContent = `${location.city}, ${location.state}, ${location.country}`;

        const removeButton = document.createElement('button');
        removeButton.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                      <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                      <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                                  </svg>`;
        removeButton.classList.add('btn', 'btn-outline-warning', 'remove-button');
        removeButton.addEventListener('click', function () {
            removeLocation(index);
            displaySavedLocations();
        });

        const useLocationButton = document.createElement('button');
        useLocationButton.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-right-circle" viewBox="0 0 16 16">
                                           <path fill-rule="evenodd" d="M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8m15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0M4.5 7.5a.5.5 0 0 0 0 1h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5z"/>
                                       </svg>`;
        useLocationButton.classList.add('btn', 'btn-outline-primary', 'use-button');
        useLocationButton.addEventListener('click', async function () {
            document.getElementById('search-event-country').value = location.country;
            document.getElementById('search-event-state').value = location.state;
            document.getElementById('search-event-city').value = location.city;

            await loadSearchBar();
            await setCountry("United States");
            await setState(location.state);
            setCity(location.city);

            searchForEvents();
        });

        locationElement.appendChild(removeButton);
        locationElement.appendChild(useLocationButton);
        savedLocationsContainer.appendChild(locationElement);
    });
}

displaySavedLocations();
document.getElementById('save-location-button').addEventListener('click', function () {
    saveLocation();
});
function saveLocation() {
    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;

    savedLocations.push({ city, state, country });
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}
function removeLocation(index) {
    savedLocations.splice(index, 1);
    localStorage.setItem('savedLocations', JSON.stringify(savedLocations));
    displaySavedLocations();
}
