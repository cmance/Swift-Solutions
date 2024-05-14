import { createBookmarkList } from './api/bookmarkLists/createBookmarkList.js';
import { getBookmarkLists } from './api/bookmarkLists/getBookmarkLists.js';
import { buildBookmarkListCard } from './ui/buildBookmarkListCard.js';
import { buildNewBookmarkListCard } from './ui/buildNewBookmarkListCard.js';
import { getFavoriteEvents } from './api/favorites/getFavoriteEvents.js';
import { buildEventCard, validateBuildEventCardProps } from './ui/buildEventCard.js';
import { buildEventDetailsModal, validateBuildEventDetailsModalProps } from './ui/buildEventDetailsModal.js';
import { showToast } from './util/toast.js';
import { applyFiltersAndSortEvents } from './util/filter.js';
import { showDeleteBookmarkListConfirmationModal } from './util/showDeleteBookmarkListConfirmationModal.js';
import { deleteBookmarkList } from './api/bookmarkLists/deleteBookmarkList.js';
import { buildAndShowEditBookmarkListModal } from './ui/buildAndShowEditBookmarkListModal.js';
import { updateBookmarkListName } from './api/bookmarkLists/updateBookmarkListName.js';
import { showDeleteFavoriteEventConfirmationModal } from './ui/showDeleteFavoriteEventConfirmationModal.js';
import { removeEventFromFavorites } from './api/favorites/removeEventFromFavorites.js';
import { getAllUserEventsFromItinerary } from './api/itinerary/itineraryApi.js'; // Adjust the import path as necessary

let currentBookmarkList = null;
let currentApiEventID = null;
let favoritedEvents = null;

document.addEventListener('DOMContentLoaded', function () {
    initPage();
});

function initPage() {
    getBookmarkLists().then(bookmarkLists => {
        displayBookmarkLists(bookmarkLists);
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

/// Displaying bookmark lists

/**
 * Returns populated bookmark list card element. Element partial must be defined in the HTML file.
 * @param {String} name
 * @param {Number} eventQuantity
 * @param {String | null | undefined} image
 * @param {String[]} bookmarkListNames
 * @returns {HTMLElement}
 */
function createBookmarkListCard(name, eventQuantity, image, bookmarkListNames) {
    const props = {
        bookmarkListName: name,
        eventQuantity: eventQuantity,
        image: image,
        onClick: () => {
            // If the user clicks on the bookmark list, display the events from that list
            initDisplayEventsFromBookmarkList(name);
            currentBookmarkList = name;
        },
        onClickDelete: (event) => {
            event.stopPropagation();
            showDeleteBookmarkListConfirmationModal(name, (listName) => {
                deleteBookmarkList(listName).then(() => {
                    initPage();
                    showToast(`Bookmark list "${name}" deleted`);
                }).catch((error) => {
                    console.error('Failed to delete bookmark list, ', error);
                    showToast('Failed to delete bookmark list');
                });
            });
        },
        onClickEdit: (event) => {
            event.stopPropagation();

            const onClickSave = (newName) => {
                if (newName === name) {
                    return;
                }
                
                updateBookmarkListName(name, newName).then(() => {
                    initPage();
                    showToast(`Bookmark list "${name}" renamed to "${newName}"`);
                }).catch((error) => {
                    console.error('Failed to update bookmark list name, ', error);
                    showToast('Failed to update bookmark list name');
                });
            }

            // Show the edit bookmark list modal
            buildAndShowEditBookmarkListModal(name, onClickSave, bookmarkListNames);
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
            const card = createBookmarkListCard(bookmarkList.title, bookmarkList.favoriteEventQuantity, bookmarkList.image, bookmarkLists.map(list => list.title));
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

/// Called just once you click a bookmark list
async function initDisplayEventsFromBookmarkList(bookmarkList) {
    favoritedEvents = await getFavoriteEvents(bookmarkList);
    document.getElementById('invalid-feedback').style.display = 'none';
    document.getElementById("no-events-found-filter-message").style.display = "none";
    document.getElementById('filter-dropdown-container').style.display = 'flex';

    // Set title of page to the bookmark list name and number of events
    document.getElementById('favorite-events-title').innerText = `${bookmarkList} (${favoritedEvents.length ?? "0"} events)`;


    var filterTagDropdown = document.getElementById('filter-tag-dropdown')
    filterTagDropdown.value = ''; // Reset the tag filter
    filterTagDropdown.innerHTML = '<option value="" disabled selected>Filter by Tag</option>';
    // Populate filter dropdown with tags names from the events
    let tags = [];
    favoritedEvents.forEach(event => {
        tags = tags.concat(event.tags);
    });
    // Replace tag objects with tag names
    tags = tags.map(tag => tag.name);
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

    displayEventsFromBookmarkList(favoritedEvents, bookmarkList);
}

/// Displaying events from a bookmark list
async function displayEventsFromBookmarkList(favoriteEvents, bookmarkList) {
    // Apply filters and sort the events
    const filteredFavoriteEvents = applyFiltersAndSortEvents(favoriteEvents);

    // Clear the favorites and the bookmark list cards containers
    document.getElementById('favorite-events-container').innerHTML = '';
    document.getElementById('bookmark-list-cards-container').innerHTML = '';

    if (filteredFavoriteEvents.length === 0) {
        document.getElementById("no-events-found-filter-message").style.display = "block";
        return;
    }

    if (!filteredFavoriteEvents) { // Validation failed
        document.getElementById('invalid-feedback').style.display = 'block';
        return;
    } 
    // Display the favorite events
    const eventCardTemplate = document.getElementById('event-card-template');
    const favoriteEventsContainer = document.getElementById('favorite-events-container');
    console.log(filteredFavoriteEvents);
    filteredFavoriteEvents.forEach(async eventInfo => {
        let eventProps = {
            img: eventInfo.eventImage,
            title: eventInfo.eventName,
            date: new Date(eventInfo.eventDate),
            city: eventInfo.eventLocation.split(',')[1],
            state: eventInfo.eventLocation.split(',')[2],
            eventOriginalLink: eventInfo.eventOriginalLink,
            tags: eventInfo.tags, // This property doesn't exist in the provided JSON object
            ticketLinks: eventInfo.ticketLinks,
            venueName: eventInfo.venueName,
            venuePhoneNumber: eventInfo.venuePhoneNumber,
            venueRating: eventInfo.venueRating,
            venueWebsite: eventInfo.venueWebsite,
            distanceUnit: null,
            distance: null,
            onPressEvent: () => onClickDetailsAsync(eventInfo),
            onPressDelete: () => {
                showDeleteFavoriteEventConfirmationModal(() => {
                    removeEventFromFavorites(eventInfo.apiEventID, bookmarkList).then(() => {
                        displayEventsFromBookmarkList(favoritedEvents, bookmarkList);
                        showToast('Event removed from favorites');
                    }).catch((error) => {
                        console.error('Failed to remove event from favorites, ', error);
                        showToast('Failed to remove event from favorites');
                    });
                })
            }
        };

        // Clone the template
        const eventCard = eventCardTemplate.content.cloneNode(true);

        if (validateBuildEventCardProps(eventProps)) {
            buildEventCard(eventCard, eventProps);
            favoriteEventsContainer.appendChild(eventCard);
        } else {
            console.error('Invalid event card props:', eventProps);
        }
    });
}

/**
 * Opens the event details modal
 * @param {Object} eventInfo
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
        tags: eventInfo.tags
    }
    if (validateBuildEventDetailsModalProps(eventDetailsModalProps)) {
        buildEventDetailsModal(document.getElementById('event-details-modal'), eventDetailsModalProps);
        currentApiEventID = eventInfo.apiEventID;
        populateItineraryDropdown(currentApiEventID);
        const modal = new bootstrap.Modal(document.getElementById('event-details-modal'));
        modal.show();
    };
}

// Listener for filter button
document.getElementById('filter-button').addEventListener('click', function () {
    displayEventsFromBookmarkList(favoritedEvents, currentBookmarkList);
});


async function createNewItinerary(itineraryTitle) {
    console.log('Creating new itinerary with title:', itineraryTitle);
    let url = `/api/ItineraryApi/Itinerary?itineraryTitle=${itineraryTitle}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        const errorText = await response.text();
        console.error(`Error ${response.status}: ${errorText}`);
        alert(`Failed to create new itinerary: ${errorText}`);
        throw new Error(`Error ${response.status}: ${errorText}`);
    }

    // Check if the response body is not empty before attempting to parse it as JSON
    const text = await response.text();
    let result;
    try {
        result = text ? JSON.parse(text) : {}; // Default to an empty object if there is no response text
    } catch (error) {
        console.error('Failed to parse response as JSON:', error);
        alert('Failed to process the response from the server.');
        throw new Error('Failed to process the response from the server.');
    }

    console.log('Itinerary created:', result);
    alert('New itinerary successfully created!');
    return result;
}
async function populateItineraryDropdown(apiEventID) {
    try {
        const itineraries = await getAllUserEventsFromItinerary();
        const dropdownMenu = document.getElementById('dropdownMenuButton1').nextElementSibling;
      
        while (dropdownMenu.children.length > 1) {
            dropdownMenu.removeChild(dropdownMenu.lastChild);
        }
      
        itineraries.forEach(itinerary => {
            const item = document.createElement('li');
            const link = document.createElement('a');
            link.className = 'dropdown-item';
            link.textContent = itinerary.itineraryTitle;
            link.href = "#";

            // Use IIFE to capture current apiEventID correctly for each link
            (function (apiEventID, itineraryId) {
                link.addEventListener('click', function () {
                    console.log("Itinerary ID:", itineraryId, "API Event ID:", apiEventID);
                    addEventToItinerary(itineraryId, apiEventID);
                });
            })(apiEventID, itinerary.id);  // Pass the current apiEventID and itinerary ID

            item.appendChild(link);
            dropdownMenu.appendChild(item);
        });
    } catch (error) {
        console.error('Failed to fetch itineraries:', error);
    }
}

async function addEventToItinerary(itineraryId, apiEventId) {
    const url = `/api/ItineraryEventApi/ItineraryEvent/${apiEventId}/${itineraryId}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (!response.ok) {
            const errorText = await response.text();
            console.error(`Error ${response.status}: ${errorText}`);
            return;
        }
        alert('Event successfully added to the itinerary!');
    } catch (error) {
        console.error('Error adding event to itinerary:', error);
    }
}

document.addEventListener('DOMContentLoaded', async function () {
    const saveButton = document.getElementById('save-new-itinerary');
    if (saveButton) {
        saveButton.addEventListener('click', async function () { // Make this function async
            const titleInput = document.getElementById('itinerary-title');
            const itineraryTitle = titleInput.value.trim();
            console.log("Captured itinerary title: ", itineraryTitle);
            if (itineraryTitle) {
                // Assume createNewItinerary is an async function and waits for API call to complete
                await createNewItinerary(itineraryTitle);
                // Close the modal
                const modalElement = document.getElementById('exampleModal');
                const bootstrapModal = bootstrap.Modal.getInstance(modalElement);
                bootstrapModal.hide();

                // Refresh the dropdown to include the new itinerary
                await populateItineraryDropdown(currentApiEventID);
            } else {
                alert('Please enter a title for the itinerary.');
            }
        });
    } else {
        console.error('Save button not found!');
    }
});
