import { buildEventCard, validateBuildEventCardProps } from "./ui/buildEventCard.js";
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { formatTags } from "./util/tags.js";
import { getBookmarkLists } from "./api/bookmarkLists/getBookmarkLists.js";
import { onPressSaveToBookmarkList } from "./util/onPressSaveToBookmarkList.js";

// Fetch event data and display it on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log("History page loaded.")
    fetchEvents()
        .then(data => {
            displayEvents(data);
            document.getElementById("history-container").style.display = "block";
        })
        .catch(error => {
            if (error.message === 'Unauthorized') {
                // Handle 401 error here
                console.error('Unauthorized: ', error);
            } else {
                console.error('Error fetching event data:', error);
            }
        });

    // Append event cards to the container element
    async function displayEvents(events) {
        const container = document.getElementById('event-history-card-container');
        if (!container) {
            console.error('Container element #event-history-card-container not found.');
            return;
        }

        if(!events || events.length === 0) {
            console.log("No events found.");
        }

        // Clear the container
        container.innerHTML = '';

        // Append event cards to the container
        for (const eventInfo of events) {
            // Get the template
            const template = document.getElementById('event-card-template');

            const bookmarkLists = await getBookmarkLists();

            let eventProps = {
                img: eventInfo.eventImage,
                title: eventInfo.eventName,
                date: new Date(eventInfo.eventDate),
                city: eventInfo.eventLocation.split(',')[1],
                state: eventInfo.eventLocation.split(',')[2],
                eventOriginalLink: eventInfo.eventOriginalLink,
                tags: await formatTags(eventInfo.eventTags), // This property doesn't exist in the provided JSON object
                bookmarkListNames: bookmarkLists.map(bookmarkList => bookmarkList.title),
                ticketLinks : eventInfo.ticketLinks,
                venueName: eventInfo.venueName,
                venuePhoneNumber: eventInfo.venuePhoneNumber,
                venueRating: eventInfo.venueRating,
                venueWebsite: eventInfo.venueWebsite,
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

});

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
        img: eventInfo.eventImage,
        title: eventInfo.eventName,
        description: (eventInfo.eventDescription ?? 'No description') + '...',
        date: new Date(eventInfo.eventDate),
        fullAddress: eventInfo.eventLocation,
        eventOriginalLink: eventInfo.eventOriginalLink,
        ticketLinks : eventInfo.ticketLinks,
        venueName: eventInfo.venueName,
        venuePhoneNumber: eventInfo.venuePhoneNumber,
        venueRating: eventInfo.venueRating,
        venueWebsite: eventInfo.venueWebsite,
        tags: [] // TODO: tags should be stored on event
        
    }

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();
    };
}
