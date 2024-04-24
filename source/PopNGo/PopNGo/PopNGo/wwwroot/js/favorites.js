import { createBookmarkList } from './api/bookmarkLists/createBookmarkList.js';
import { getBookmarkLists } from './api/bookmarkLists/getBookmarkLists.js';
import { buildBookmarkListCard } from './ui/buildBookmarkListCard.js';
import { buildNewBookmarkListCard } from './ui/buildNewBookmarkListCard.js';
import { getFavoriteEvents } from './api/favorites/getFavoriteEvents.js';
import { buildEventCard, validateBuildEventCardProps } from './ui/buildEventCard.js';
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { formatTags } from './util/tags.js';
import { showToast } from './util/toast.js';
import { applyFiltersAndSortEvents } from './util/filter.js';

let currentBookmarkList = null;

document.addEventListener('DOMContentLoaded', function () {
    initPage();
});

function initPage() {
    getBookmarkLists().then(bookmarkLists => {
        if (bookmarkLists.length === 0) {
            displayNoBookmarkListsMessage();
        } else {
            displayBookmarkLists(bookmarkLists);
        }
    }).catch((error) => {
        // If the user is not logged in, display a login prompt
        displayLoginPrompt();
    });

    // Get the apply filters button

}

function displayLoginPrompt() {
    document.getElementById('favorite-events-title').style.display = 'none'; // Hide the title if the user is not logged in
    document.getElementById('login-prompt').style.display = 'block';
}

function displayNoBookmarkListsMessage() {
    document.getElementById('favorite-events-title').style.display = 'none';
    document.getElementById('no-favorites-message').style.display = 'block';
}

/// Displaying bookmark lists

/**
 * Returns populated bookmark list card element. Element partial must be defined in the HTML file.
 * @param {String} name
 * @param {Number} eventQuantity
 * @returns {HTMLElement}
 */
function createBookmarkListCard(name, eventQuantity) {
    const props = {
        bookmarkListName: name,
        eventQuantity: eventQuantity,
        onClick: () => {
            // If the user clicks on the bookmark list, display the events from that list
            displayEventsFromBookmarkList(name);
            currentBookmarkList = name;
        }
    };

    // Select and clone the bookmark list card template
    const bookmarkListCardTemplate = document.getElementById('bookmark-list-card-template');
    const bookmarkListCard = bookmarkListCardTemplate.content.cloneNode(true);

    // Build the card
    buildBookmarkListCard(bookmarkListCard, props);

    return bookmarkListCard;
}

/**
 * Takes BookmarkList[] as from getBookmarkLists and displays them on screen
 * @param {BookmarkList[]} bookmarkLists
 */
function displayBookmarkLists(bookmarkLists) {
    // Select the bookmark list container
    const bookmarkListContainer = document.getElementById('bookmark-list-cards-container');

    // Clear the container
    bookmarkListContainer.innerHTML = '';

    // Create a card for each bookmark list
    bookmarkLists.forEach(bookmarkList => {
        try {
            const card = createBookmarkListCard(bookmarkList.title, bookmarkList.favoriteEventQuantity);
            bookmarkListContainer.appendChild(card);
        } catch (error) {
            console.error("Props for bookmark list card was invalid, skipping...")
        }
    });

    // Create and append the "Create new bookmark list" card
    const createNewBookmarkListCardTemplate = document.getElementById('create-new-bookmark-list-card-template');
    const createNewBookmarkListCard = createNewBookmarkListCardTemplate.content.cloneNode(true);
    buildNewBookmarkListCard(createNewBookmarkListCard, { onClickCreateBookmarkList: createNewBookmarkList });
    bookmarkListContainer.appendChild(createNewBookmarkListCard);
}

/// Happens on click of "Create new bookmark list" button
function createNewBookmarkList(bookmarkListName) {
    createBookmarkList(bookmarkListName).then(() => {
        initPage();
    }).catch((error) => {
        console.error('Failed to create bookmark list, ', error);
        showToast('Bookmark list cannot be named the same as an existing list.')
    });
}

/// Displaying events from a bookmark list
async function displayEventsFromBookmarkList(bookmarkList) {
    let favoriteEvents = await getFavoriteEvents(bookmarkList);
    document.getElementById('invalid-feedback').style.display = 'none';
    document.getElementById("no-events-found-filter-message").style.display = "none";
    document.getElementById('filter-dropdown-container').style.display = 'flex';

    // Apply filters and sort the events
    favoriteEvents = applyFiltersAndSortEvents(favoriteEvents);


    // Set title of page to the bookmark list name and number of events
    document.getElementById('favorite-events-title').innerText = `${bookmarkList} (${favoriteEvents.length ?? "0"} events)`;

    // Clear the favorites and the bookmark list cards containers
    document.getElementById('favorite-events-container').innerHTML = '';
    document.getElementById('bookmark-list-cards-container').innerHTML = '';

    if (favoriteEvents.length === 0) {
        document.getElementById("no-events-found-filter-message").style.display = "block";
        return;
    }

    if (!favoriteEvents) { // Validation failed
        document.getElementById('invalid-feedback').style.display = 'block';
        return;
    } 
    // Display the favorite events
    const eventCardTemplate = document.getElementById('event-card-template');
    const favoriteEventsContainer = document.getElementById('favorite-events-container');
    console.log(favoriteEvents);
    favoriteEvents.forEach(async eventInfo => {
        let eventProps = {
            img: eventInfo.eventImage,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventDate),
            city: eventInfo.eventLocation.split(',')[1],
            state: eventInfo.eventLocation.split(',')[2],
            eventOriginalLink: eventInfo.eventOriginalLink,
            tags: await formatTags(eventInfo.eventTags), // This property doesn't exist in the provided JSON object
            ticketLinks: eventInfo.ticketLinks,
            venueName: eventInfo.venueName,
            venuePhoneNumber: eventInfo.venuePhoneNumber,
            venueRating: eventInfo.venueRating,
            venueWebsite: eventInfo.venueWebsite,
            distanceUnit: null,
            distance: null,
            onPressEvent: () => onClickDetailsAsync(eventInfo),
        };

        // Clone the template
        const eventCard = eventCardTemplate.content.cloneNode(true);

        if (validateBuildEventCardProps(eventProps)) {
            buildEventCard(eventCard, eventProps);
            favoriteEventsContainer.appendChild(eventCard);
        }
    });
}

/**
 * Opens the event details modal
 * @param {Object} eventInfo
 */
async function onClickDetailsAsync(eventInfo) {
    console.log("onClickDetailsAsync Favorites");
    console.log(eventInfo);
    const eventDetailsModalProps = {
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
        tags: [] // TODO: tags should be stored on event
    }

    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();
    };
}

// Listener for filter button
document.getElementById('filter-button').addEventListener('click', function () {
    displayEventsFromBookmarkList(currentBookmarkList);
});

