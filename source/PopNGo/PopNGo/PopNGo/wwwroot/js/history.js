import { buildEventCard, validateBuildEventCardProps } from "./ui/buildEventCard.js";
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { getBookmarkLists } from "./api/bookmarkLists/getBookmarkLists.js";
import { onPressSaveToBookmarkList } from "./util/onPressSaveToBookmarkList.js";
import { applyFiltersAndSortEvents } from './util/filter.js';
import { getAllUserEventsFromItinerary} from './api/itinerary/itineraryApi.js'; // Adjust the import path as necessary
import { bindItinerarySaving } from "./util/bindItinerarySaving.js";
import { formatTagName } from "./util/tags.js";


let currentApiEventID = null;

// Fetch event data and display it on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log("History page loaded.")
    fetchEvents()
        .then(data => {
            if (!data || data.length === 0) {
                console.log('No data returned');
                document.getElementById("no-history-message").style.display = "block";
                document.getElementById("no-history-img-container").style.display = "block";
                // Handle no data case here
            }

            var filterTagDropdown = document.getElementById('filter-tag-dropdown')
            filterTagDropdown.value = ''; // Reset the tag filter
            filterTagDropdown.innerHTML = '<option value="" disabled selected>Filter by Tag</option>';

            // Populate filter dropdown with tags names from the events
            let tags = [];
            data.forEach(event => {
                tags = tags.concat(event.tags);
            });
            // Replace tag objects with tag names
            tags = tags.map(tag => formatTagName(tag.name));
            tags = [...new Set(tags)]; // Remove duplicates
            // Populate the filter dropdown with the tags
            document.getElementById('filter-tag-dropdown').style.display = 'flex';
            const option = document.createElement('option');
            option.value = '';
            option.innerText = "Any";
            filterTagDropdown.appendChild(option);

            tags.forEach(tag => {
                const option = document.createElement('option');
                option.value = tag;
                option.innerText = tag;
                filterTagDropdown.appendChild(option);
            });

            displayEvents(data);
            document.getElementById("history-container").style.display = "block";

            bindItinerarySaving();
        })
        .catch(error => {
            if (error.message === 'Unauthorized') {
                // Handle 401 error here
                console.error('Unauthorized: ', error);
            } else {
                console.error('Error fetching event data:', error);
            }
        });

    document.getElementById('filter-dropdown-container').style.display = 'flex';
    document.getElementById('filter-button').addEventListener('click', function () {
        fetchEvents()
            .then(data => {
                if (!data || data.length === 0) {
                    console.log('No data returned');
                    document.getElementById("no-history-message").style.display = "block";
                    document.getElementById("no-history-img-container").style.display = "block";
                    // Handle no data case here
                } else {
                    try {
                        let sortedEvents = applyFiltersAndSortEvents(data);
                        if (sortedEvents === false) {
                            document.getElementById('invalid-feedback').style.display = 'block';
                        }
                        else {
                            document.getElementById('invalid-feedback').style.display = 'none';
                        }
                        displayEvents(sortedEvents);
                    } catch (error) {
                        if (error.message === 'Invalid date range. Start date cannot be after end date.') {
                            // Reveal the div
                            document.getElementById('invalid-feedback').style.display = 'block';
                        } else {
                            // Handle other errors
                            console.error(error);
                        }
                    }
                }
            })
            .catch(error => {
                if (error.message === 'Unauthorized') {
                    // Handle 401 error here
                    console.log("error caught");
                } else {
                    console.error('Error fetching event data:', error);
                }
            });
    });
    
});

// Append event cards to the container element
async function displayEvents(events) {

    const container = document.getElementById('event-history-card-container');
    if (!container) {
        console.error('Container element #event-history-card-container not found.');
        return;
    }
    document.getElementById("no-events-found-filter-message").style.display = "none";

    // Clear the container
    container.innerHTML = '';

    // Check if there are any events
    if (!events || events.length === 0) {
        document.getElementById("no-events-found-filter-message").style.display = "block";
        return;
    }

    // Append event cards to the container
    for (const eventInfo of events) {
        // Get the template
        const template = document.getElementById('event-card-template');

        const bookmarkLists = await getBookmarkLists();

        let eventProps = {
            apiEventID: eventInfo.apiEventID,
            img: eventInfo.eventImage,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventDate),
            city: eventInfo.eventLocation.split(',')[1],
            state: eventInfo.eventLocation.split(',')[2],
            eventOriginalLink: eventInfo.eventOriginalLink,
            tags: eventInfo.tags,
            bookmarkListNames: bookmarkLists.map(bookmarkList => bookmarkList.title),
            ticketLinks: eventInfo.ticketLinks,
            venueName: eventInfo.venueName,
            venuePhoneNumber: eventInfo.venuePhoneNumber,
            venueRating: eventInfo.venueRating,
            venueWebsite: eventInfo.venueWebsite,
            distanceUnit: null,
            distance: null,
            onPressBookmarkList: (bookmarkListName) => onPressSaveToBookmarkList(eventInfo.apiEventID, bookmarkListName),
            onPressEvent: () => onClickDetailsAsync(eventInfo),
        };
        // console.log(eventProps)
        // Clone the template
        const eventCard = template.content.cloneNode(true);

        if (validateBuildEventCardProps(eventProps)) {
            buildEventCard(eventCard, eventProps);
            container.appendChild(eventCard);
        }
    }
}

// Fetch event data and display it
async function fetchEvents() {
    const response = await fetch("/api/EventHistoryApi/EventHistory");
    if (!response.ok) {
        if (response.status === 401) {
            document.getElementById("login-prompt").style.display = "block";
            throw new Error('Unauthorized');
        } else if (response.status === 404) {
            document.getElementById("no-history-message").style.display = "block";
            document.getElementById("no-history-img-container").style.display = "block";
            throw new Error('No history found');
        }
        throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log(data);
    return data;
}

/**
 * Opens the event details modal
 * @param {any} eventInfo
 */
async function onClickDetailsAsync(eventInfo) {
    const eventDetailsModalProps = {
        apiEventID: eventInfo.apiEventID,
        img: eventInfo.eventImage,
        title: eventInfo.eventName,
        description: (eventInfo.eventDescription ?? 'No description') + '...',
        date: new Date(eventInfo.eventDate),
        fullAddress: eventInfo.eventLocation,
        eventOriginalLink: eventInfo.eventOriginalLink,
        ticketLinks: eventInfo.ticketLinks,
        venueName: eventInfo.venueName,
        venuePhoneNumber: eventInfo.venuePhoneNumber,
        venueRating: eventInfo.venueRating,
        venueWebsite: eventInfo.venueWebsite,
        tags: eventInfo.tags,
    }

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();
    };
}