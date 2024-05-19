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
import { getDistancesForEvents, getDistanceUnit, convertDistance } from './api/distance/getDistances.js';
import { getIsUserLoggedIn } from './api/user/getIsUserLoggedIn.js';
import { getForecastForLocation } from './api/weather/getForecast.js';
import { buildWeatherCard, validateBuildWeatherCardProps } from './ui/buildWeatherCard.js';
import { addMapLoadingSpinner, removeMapLoadingSpinner } from './util/mapLoadingSpinners.js';
import { bindItinerarySaving } from './util/bindItinerarySaving.js';

let map = null;
let mapMarkers = [];
let page = 0;
const pageSize = 10;
let num_searches = 0;
let user_is_logged_in = null;
let recaptcha_confirmed = false;
let userLocation = {};
let distanceUnit = "miles"
let currentApiEventID = null;

window.onCaptchaSuccess = function(token) {
    // Captcha is success, remove modal and allow to continue using page
    // Hide recaptcha modal
    document.getElementById('recaptcha-modal').style.display = 'none';
    recaptcha_confirmed = true;
}

document.addEventListener("DOMContentLoaded", async function () {
    // Check if user is logged in
    user_is_logged_in = await getIsUserLoggedIn();

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {
            distanceUnit = await getDistanceUnit();

            document.getElementById('distance-select').value = distanceUnit;
            document.getElementById('distance-select').toggleAttribute('disabled', true);

            // document.getElementById('distance-checkbox').checked = distanceUnit === 'miles';

            userLocation["lat"] = position.coords.latitude;
            userLocation["long"] = position.coords.longitude;

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

    await bindItinerarySaving();
}, { once: true });


async function loadSearchBarAndEvents(city, state, country) {
    await loadSearchBar().then(async () => {
        await setCountry(country);
        await setState(state);
        setCity(city);
    });

    let nextPageButton = document.getElementById('next-page-button');
    if (nextPageButton) {
        nextPageButton.addEventListener('click', nextPage);
    }

    let previousPageButton = document.getElementById('previous-page-button');
    if (previousPageButton) {
        previousPageButton.addEventListener('click', previousPage);
    }

    if (document.getElementById('events-container')) {
        await searchForEvents();
    }

    let searchEventButton = document.getElementById('search-event-button');
    if (searchEventButton) {
        searchEventButton.addEventListener('click', async function () {
            page = 0; // Reset page number when searching 
            await searchForEvents();
        });
    }

    let searchEventInput = document.getElementById('search-event-input');
    if (searchEventInput) {
        searchEventInput.addEventListener('keyup', async function (event) {
            if (event.key === 'Enter') {
                await searchForEvents();
            }
        });
    }


    let distanceSelect = document.getElementById('distance-select');
    if (distanceSelect) {
        distanceSelect.addEventListener('change', async function () {
            let oldUnit = distanceUnit;
            distanceUnit = distanceSelect.value.toLowerCase();
            const events = document.getElementById('events-container').children;
            for (let event of events) {
                let distance = event.querySelector('#distance-number').textContent;
                let convertedDistance = convertDistance(+distance, distanceUnit, oldUnit);

                event.querySelector('#distance-number').textContent = convertedDistance;
                event.querySelector('#distance-unit').textContent = distanceUnit === 'miles' ? 'mi' : 'km';
            }
        });
    }
}

/**
 * Takes in an event info object and adds it to the history via http and opens the event details modal
 * @async
 * @function onClickDetailsAsync
 * @param {string} eventInfo
 */
async function onClickDetailsAsync(eventInfo) {
    if (!eventInfo.apiEventID) {
        console.error("Failed to retrieve apiEventID from event info", eventInfo);
        return; // Early return if the apiEventID is undefined
    }

    const eventDetailsModalProps = {
        apiEventID: eventInfo.apiEventID,
        img: eventInfo.eventImage,
        title: eventInfo.eventName,
        description: (eventInfo.eventDescription ?? 'No description') + '...',
        date: new Date(eventInfo.eventDate),
        fullAddress: eventInfo.eventLocation,
        eventOriginalLink: eventInfo.eventOriginalLink,
        tags: eventInfo.tags,
        ticketLinks: eventInfo.ticketLinks,
        venueName: eventInfo.venueName,
        venuePhoneNumber: eventInfo.venuePhoneNumber,
        venueRating: eventInfo.venueRating,
        venueWebsite: eventInfo.venueWebsite
    };

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();

        addEventToHistory(eventInfo.apiEventID);
    } else {
        console.error("Validation failed for event details modal properties", eventDetailsModalProps);
    }
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
    await searchForEvents();
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
        await searchForEvents();
    }
}

function getPaginationIndex() {
    return (page * pageSize);
}

/**
 * Display events.
 * Events is an array of event objects returned from the API
 * @param {any} events
 */
async function displayEvents(events) {
    let eventsContainer = document.getElementById('events-container');
    eventsContainer.innerHTML = ''; // Clear the container
    let eventCardTemplate = document.getElementById('event-card-template');
    // let paginationDiv = document.getElementById('pagination');

    // Update the page number
    document.getElementById('page-number').innerHTML = page + 1;
    document.getElementById('previous-page-button').innerHTML = page;
    document.getElementById('next-page-button').innerHTML = page + 2;
    document.getElementById('previous-page-button').disabled = page === 0;


    events = events.map(event => { event.distance = null; event.distanceUnit = null; return event; });

    // Populate event data with distances
    if(userLocation.lat && userLocation.long) {
        const eventDistances = await getDistancesForEvents(userLocation.lat, userLocation.long, events, distanceUnit);
        if(eventDistances.distances.length > 0) {
            events = events.map((event, index) => {
                event.distance = eventDistances.distances[index];
                event.distanceUnit = eventDistances.unit;
                return event;
            });
        }
    } else {
        events = events.map(event => {
            event.distance = null;
            event.distanceUnit = null;
            return event;
        });
    }

    // TODO: BUG this errors when not logged in
    let bookmarkLists = [];
    try {
        bookmarkLists = await getBookmarkLists();
    } catch (error) {
        if (error instanceof UnauthorizedError) {
            disableAddToItineraryButton();
            console.error("Unauthorized to get bookmark lists");
        } else {
            console.error("Error getting bookmark lists", error);
        }
    }

    for (let eventInfo of events) {
        let newEventCard = eventCardTemplate.content.cloneNode(true);

        let eventCardProps = {
            apiEventID: eventInfo.apiEventID,
            img: eventInfo.eventImage,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventDate),
            city: eventInfo.eventLocation.split(',')[1],
            state: eventInfo.eventLocation.split(',')[2],
            tags: eventInfo.tags,
            bookmarkListNames: bookmarkLists.map(bookmarkList => bookmarkList.title),
            distance: eventInfo.distance,
            distanceUnit: eventInfo.distanceUnit,
            onPressBookmarkList: (bookmarkListName) => onPressSaveToBookmarkList(eventInfo.apiEventID, bookmarkListName),
            onPressEvent: () => onClickDetailsAsync(eventInfo),
        }
        if (validateBuildEventCardProps(eventCardProps)) {
            buildEventCard(newEventCard, eventCardProps);
            eventsContainer.appendChild(newEventCard);
        } else {
            console.error("Invalid event card props", eventCardProps);
        }
    }

    // paginationDiv.style.display = 'flex'; //Display after events are loaded
}

function disableAddToItineraryButton() {
    const addToItineraryButton = document.getElementById('dropdownMenuButton1');
    if (addToItineraryButton) {
        addToItineraryButton.disabled = true;
        addToItineraryButton.classList.add('disabled'); // Optionally add this class for CSS styling purposes
        addToItineraryButton.setAttribute('title', 'You must be logged in to add to itinerary');
    }
}

function displayWeatherForecast(weatherData) {
    let weatherForecastContainer = document.getElementById('weather-preview-container');
    weatherForecastContainer.innerHTML = ''; // Clear the container
    const weatherForecastTemplate = document.getElementById('weather-card-template');

    for (let forecast of weatherData.weatherForecasts) {
        let newForecastCard = weatherForecastTemplate.content.cloneNode(true);

        let forecastCardProps = {
            date: new Date(forecast.date),
            condition: forecast.condition,
            minTemp: +forecast.minTemp.toFixed(1),
            maxTemp: +forecast.maxTemp.toFixed(1),
            humidity: forecast.humidity,
            cloudCover: forecast.cloudCover,
            precipitationType: forecast.precipitationType,
            precipitationAmount: forecast.precipitationAmount.toFixed(2),
            precipitationChance: forecast.precipitationChance,
            temperatureUnit: weatherData.temperatureUnit,
            measurementUnit: weatherData.measurementUnit
        };

        if (validateBuildWeatherCardProps(forecastCardProps)) {
            buildWeatherCard(newForecastCard, forecastCardProps);
            weatherForecastContainer.appendChild(newForecastCard);
        } else {
            console.error("Invalid forecast card props", forecastCardProps);
        }
    }
}

/**
 * Run a search for events and display them
 * 
 * @async
 * @function searchForEvents
 * @returns {Promise<void>}
 */
async function searchForEvents() {
    num_searches++;
    if (num_searches % 10 === 0 && !recaptcha_confirmed && !user_is_logged_in) {
        // Show recaptcha modal
        document.getElementById('recaptcha-modal').style.display = 'block';
    }
    let paginationDiv = document.getElementById('pagination');
    paginationDiv.style.display = 'none'; //Hide pagination while searching
    createPlaceholderCards();
    toggleNoEventsSection(false);
    toggleSearchingEventsSection(true);
    toggleSearching();
    document.getElementById('distance-select').toggleAttribute('disabled', true);

    let date = document.getElementById('filter-dropdown').value;

    const events = await getEvents(getSearchQuery(), getPaginationIndex(), date);
    removePlaceholderCards(); // Remove the placeholder cards as the API has returned

    console.log(events);
    toggleSearchingEventsSection(false); // Hide the searching events section
    const country = document.getElementById('search-event-country').value;
    const state = document.getElementById('search-event-state').value;
    const city = document.getElementById('search-event-city').value;
    let mapCoords = await getLocationCoords(country, state, city);

    if (!events || events.length === 0) {
        paginationDiv.style.display = 'none';
        toggleNoEventsSection(true);
        removeMapLoadingSpinner();
    } else {
        displayEvents(events);
        initMap(events);

        if (map) {
            deleteMarkers(); // Clear markers before adding new ones
            map.setCenter(mapCoords ?? map.getCenter());
            events.forEach(eventInfo => {
                const lat = eventInfo.latitude ? eventInfo.latitude : 44.848; //Hardcoded Monmouth, Oregon coordinates for now
                const lng = eventInfo.longitude ? eventInfo.longitude : -123.229; //Hardcoded Monmouth, Oregon coordinates for now
                const position = { lat, lng };
                addMapMarker(position, map, eventInfo);
            });
        }

        paginationDiv.style.display = 'flex'; //Display after events are loaded
    }

    const weatherForecast = await getForecastForLocation(mapCoords.lat, mapCoords.lng);
    displayWeatherForecast(weatherForecast);
    
    toggleSearching();
    document.getElementById('distance-select').toggleAttribute('disabled', false);
}

function createPlaceholderCards() {
    let eventsContainer = document.getElementById('events-container')
    if (!eventsContainer) return; // If the element doesn't exist, exit the function
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

        map.addListener('drag', function () {
            document.getElementById("map-helper-text-container").style.display = 'none';
        });

        google.maps.event.addListener(map, 'idle', () => debounceUpdateLocationAndFetch(map, getPaginationIndex()));
    }

    events?.forEach(async eventInfo => {
        // Add a marker on the map for the event
        if (eventInfo) {
            const lat = eventInfo.latitude ? eventInfo.latitude : 44.848; //Hardcoded Monmouth, Oregon coordinates for now
            const lng = eventInfo.longitude ? eventInfo.longitude : -123.229; //Hardcoded Monmouth, Oregon coordinates for now
            const position = { lat, lng };

            addMapMarker(position, map, eventInfo);
            // removeMapLoadingSpinner();
            google.maps.event.addListener(map, 'idle', () => {
                debounceUpdateLocationAndFetch(map);
            });
        }
        removeMapLoadingSpinner();
        revealHelperText();
    });

}

function addMapMarker(position, map, eventInfo) {
    let mapMarker = new google.maps.Marker({
        position: position,
        map: map,
        title: eventInfo.eventName
    });
    mapMarker.addListener('click', async function () {
        onClickDetailsAsync(eventInfo);
    });
    mapMarkers.push(mapMarker);
}

// Function to set map on all markers, if passed null, it will remove the markers
function setMapOnAll(map) {
    for (let i = 0; i < mapMarkers.length; i++) {
        mapMarkers[i].setMap(map);
    }
}

// Function to clear markers, but does not remove them from the mapMarkers array
function clearMarkers() {
    setMapOnAll(null);
}

// Function to delete markers
function deleteMarkers() {
    clearMarkers();
    mapMarkers = [];
}

function revealHelperText() {
    document.querySelector('#map-helper-text-container .helper-text p').style.display = 'block';
}

window.onload = async function () {

    createPlaceholderCards(); // Create placeholder cards while waiting for the API to return
    addMapLoadingSpinner();

    if (document.getElementById('demo-map-id')) {
        loadMapScript();
    }
}
