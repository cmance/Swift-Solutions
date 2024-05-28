import { getAllUserEventsFromItinerary } from './api/itinerary/itineraryApi.js';
import { capitalizeFirstLetter } from './util/capitalizeFirstLetter.js';
import { getPlaceSuggestions } from './api/itinerary/placeSuggestion.js';
import { buildItinerary } from './ui/buildItineraryAccordion.js';
import { buildItineraryEvent } from './ui/buildItineraryEvent.js';
import { showToast } from './util/toast.js';

let notificationIndex = -1;
let currentEvent = null; // To store the current event details
let selectedPlace = null; // To store the selected place suggestion

document.addEventListener('DOMContentLoaded', async () => {
    try {
        const itineraries = await getAllUserEventsFromItinerary();
        console.log('Itineraries:', itineraries);
        if (itineraries && itineraries.length > 0) {
            const accordionExample = document.getElementById('itinerary-content');

            document.getElementById("login-prompt").style.display = "none";
            document.getElementById("no-itinerary-message").style.display = "none";
            accordionExample.style.display = "block"; // Ensure it's visible if there are items

            itineraries.forEach((itinerary, index) => {
                createAccordionHtml(itinerary, index);
            });
        } else {
            displayNoItineraryMessage();
        }
    } catch (error) {
        if (error.message.includes('Unauthorized')) {
            displayLoginPrompt();
        } else {
            displayNoItineraryMessage();
        }

        console.error('Failed to fetch itineraries:', error);
    }

    attachEventListeners(); // Make sure this is called after updating the DOM
});

async function displayLoginPrompt() {
    document.getElementById("login-prompt").style.display = "block";
    document.getElementById("itinerary-content").style.display = "block";
}

async function displayNoItineraryMessage() {
    const noItineraryMsg = document.getElementById("no-itinerary-message");
    if (noItineraryMsg) {
        noItineraryMsg.style.display = "block";
    }
    document.getElementById("itinerary-content").style.display = "none"; // Also hide if no itineraries are found
}

function createAccordionHtml(itinerary, index) {
    const itineraryContainer = document.getElementById('itinerary-content');

    try {
        let itineraryElement = buildItinerary({
            index: index,
            title: itinerary.itineraryTitle,
            onDelete: (button) => deleteItinerary(button, itinerary.id),
            onAddNotification: () => createNotificationEntry(index, null),
            onConfirmNotifications: async () => await confirmNotifications(index, itinerary.id),
        });

        itineraryContainer.appendChild(itineraryElement);
        itineraryElement = document.getElementById(`collapse${index}`);
    
        itinerary.events.forEach(event => {
            const eventElement = buildItineraryEvent({
                itineraryId: itinerary.id,
                apiEventID: event.eventDetails.apiEventID,
                img: event.eventDetails.eventImage,
                title: event.eventDetails.eventName,
                date: new Date(event.eventDetails.eventDate),
                tags: event.eventDetails.tags,
                address: event.eventDetails.eventLocation,
                latitude: event.eventDetails.latitude,
                longitude: event.eventDetails.longitude,
                reminderTime: event.reminderTime,
                reminderCustomTime: event.reminderCustomTime,
                onDelete: () => deleteEvent(event.eventDetails.apiEventID, itinerary.id),
                onSaveReminder: (element, originalTime, time, customTime) => saveReminder(event.eventDetails.apiEventID, itinerary.id, originalTime, time, customTime, element),
            });

            itineraryElement.querySelector('#events-container').appendChild(eventElement);

            // Set the current event to the last one (or change this logic as needed)
            currentEvent = {
                address: event.eventDetails.eventLocation,
                latitude: event.eventDetails.latitude,
                longitude: event.eventDetails.longitude
            };
        });

    } catch (error) {
        console.error('Failed to create accordion HTML:', error);
    }

    itinerary.notifications.forEach(notification => {
        createNotificationEntry(index, notification);
    });
}

function createNotificationEntry(itineraryId, notification) {
    const itineraryElement = document.getElementById(`heading${itineraryId}`).nextElementSibling;

    const notificationElement = document.createElement('div');
    notificationElement.className = 'notification';

    const notificationAddressElement = document.createElement('input');
    notificationAddressElement.className = 'col-sm-8';
    notificationAddressElement.id = 'notification-address';
    notificationAddressElement.type = 'text';
    notificationAddressElement.value = notification?.address || "";
    notificationAddressElement.placeholder = 'Email Address';

    const notificationDeleteButton = document.createElement('button');
    notificationDeleteButton.className = 'btn btn-danger col-sm-2 offset-4 offset-sm-2';
    notificationDeleteButton.textContent = 'Delete';
    notificationDeleteButton.addEventListener('click', () => {
        notificationElement.remove();
    });

    notificationElement.appendChild(notificationAddressElement);
    notificationElement.appendChild(notificationDeleteButton);

    const notificationsContainer = itineraryElement.querySelector('#notification-container');
    notificationsContainer.appendChild(notificationElement);
}

async function confirmNotifications(index, itineraryId) {
    const notifications = document.getElementById(`heading${index}`)
                            .nextElementSibling
                            .querySelector('#notification-container')
                            .querySelectorAll('.notification');
    const emailAddresses = Array.from(notifications).map(notification => notification.querySelector('input').value);

    console.log('Email addresses:', emailAddresses);
    
    try {
        let response = await fetch(`/api/ItineraryApi/ConfirmNotifications/${itineraryId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(emailAddresses),
        });

        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        showToast('Notifications confirmed successfully');
    } catch (error) {
        console.error('Failed to confirm notifications:', error);
        showToast('Failed to confirm notifications');
    }
}

async function saveReminder(apiEventID, itineraryId, originalTime, time, customTime, element) {
    try {
        let response = await fetch(`/api/ItineraryEventApi/SaveReminder/eventId=${apiEventID}&itineraryId=${itineraryId}&reminderTime=${time}&customTime=${customTime}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        showToast('Reminder saved successfully');
    } catch (error) {
        console.error('Failed to save the reminder:', error);
        showToast('Failed to save the reminder');

        // Reset the reminder time to the original value if the save fails
        element.value = originalTime;
    }
}

async function deleteEvent(apiEventID, itineraryId) {
    try {
        let response = await fetch(`/api/ItineraryEventApi/DeleteEventFromItinerary/${apiEventID}/${itineraryId}`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        document.querySelector(`div[data-event-id="${apiEventID}"]`).remove();
    } catch (error) {
        console.error('Failed to delete the event:', error);
        alert('Failed to delete the event');
    }
}

async function deleteItinerary(button, itineraryId) {
    if (!confirm('Are you sure you want to delete this itinerary?')) return;

    try {
        let response = await fetch(`/api/ItineraryApi/${itineraryId}`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        // Remove the itinerary element from DOM after successful deletion
        const itemToRemove = button.closest('.accordion-item');
        itemToRemove.remove();

        // Check if there are any itineraries left
        if (!document.querySelector('.accordion-item')) {
            displayNoItineraryMessage();
        }
        alert('Itinerary deleted successfully');
    } catch (error) {
        console.error('Failed to delete the itinerary:', error);
        alert('Failed to delete the itinerary');
    }
}

function attachEventListeners() {
    document.querySelectorAll('[id^="hotels-button-"], [id^="food-button-"], [id^="coffee-button-"]').forEach(button => {
        console.log('Adding event listener to button:', button.id);
        const debouncedClick = debounce(async function () {
            const index = this.id.split('-').slice(-2).join('-'); // Get the latitude and longitude from the button ID
            const parent = this.closest('.single-timeline-area');
            const latitude = parent.getAttribute('data-latitude');
            const longitude = parent.getAttribute('data-longitude');
            const type = this.id.split('-')[0].split('button')[0]; // Parse the type from ID
            const suggestionsContainer = document.getElementById(`suggestions-container-${latitude}-${longitude}`);

            // If the button clicked is the same as the previous button, toggle the visibility
            if (this.classList.contains('active-button')) {
                suggestionsContainer.style.display = suggestionsContainer.style.display === 'block' ? 'none' : 'block';
                return;
            }

            // Remove active class from all buttons and hide all suggestion containers
            document.querySelectorAll('.active-button').forEach(btn => btn.classList.remove('active-button'));
            document.querySelectorAll('[id^="suggestions-container-"]').forEach(container => container.style.display = 'none');

            // Set the current button as active
            this.classList.add('active-button');

            if (latitude && longitude) {
                // Fetch and display only if the container was previously hidden
                await fetchAndDisplayPlaceSuggestions(type.charAt(0).toUpperCase() + type.slice(1), latitude, longitude, `${latitude}-${longitude}`);
                suggestionsContainer.style.display = 'block';
            } else {
                console.error(`Latitude or longitude is null for event at index ${index}: Lat ${latitude}, Lon ${longitude}`);
                alert('Location data is not available for this event.');
            }
        }, 300); // Adjust debounce delay as needed

        button.addEventListener('click', debouncedClick);
    });

    document.getElementById('getDirectionsBtn').addEventListener('click', async () => {
        if (selectedPlace && currentEvent) {
            await displayDirectionsInModal(currentEvent.address, selectedPlace.address);
        } else {
            alert('Please select a place and ensure the current event is loaded.');
        }
    });
}

function debounce(func, wait) {
    let timeout;
    return function () {
        const context = this, args = arguments;
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(context, args), wait);
    };
}

async function fetchAndDisplayPlaceSuggestions(query, latitude, longitude, index) {
    console.log(`Fetching places for ${query} at ${latitude}, ${longitude}`);
    try {
        const suggestions = await getPlaceSuggestions(query, latitude, longitude);
        if (suggestions.length === 0) {
            console.log('No suggestions found for the given location.');
            alert('No suggestions available at this location.');
        } else {
            displaySuggestions(suggestions, index);
        }
    } catch (error) {
        console.error(`Error fetching place suggestions: ${error.message}`);
        alert(`Error: ${error.message}`);
    }
}

function displaySuggestions(suggestions, index) {
    const suggestionsContainer = document.getElementById(`suggestions-container-${index}`);
    if (!suggestionsContainer) {
        console.error(`Suggestions container not found for index ${index}`);
        return;
    }
    suggestionsContainer.innerHTML = '';  // Clear previous content

    const fragment = document.createDocumentFragment();

    suggestions.forEach(suggestion => {
        const card = document.createElement('div');
        card.className = 'card mb-3 clickable';  // Make sure 'clickable' styles the cursor appropriately
        card.style = 'cursor: pointer;';  // Ensure the card is clickable
        card.setAttribute('data-bs-toggle', 'modal');
        card.setAttribute('data-bs-target', '#exampleModal');

        const imagePath = suggestion.thumbnail && suggestion.thumbnail.trim() !== '' ? suggestion.thumbnail : '/media/images/placeholder_event_card_image.png';

        card.innerHTML = `
            <div class="row g-0 align-items-center">
                <div class="col-md-4">
                    <img src="${imagePath}" onerror="this.onerror=null; this.src='/media/images/placeholder_event_card_image.png';" class="img-fluid rounded-start" alt="Image of ${suggestion.title}" style="height: 100px; width: 90%; object-fit: cover;">
                </div>
                <div class="col-md-8">
                    <div class="card-body">
                        <p class="card-title">${suggestion.title}</p>
                    </div>
                </div>
            </div>
        `;
        fragment.appendChild(card);

        // Event listener to update modal content on click
        card.addEventListener('click', () => {
            const modalTitle = document.getElementById('exampleModalLabel');
            const modalBody = document.querySelector('.modal-body');
            modalTitle.textContent = suggestion.title;
            modalBody.innerHTML = formatModalBodyContent(suggestion);
            selectedPlace = suggestion; // Store selected place
        });
    });

    suggestionsContainer.appendChild(fragment);
}

function formatModalBodyContent(suggestion) {
    let operatingHoursHTML = '';
    if (suggestion.operating_hours) {
        operatingHoursHTML = Object.keys(suggestion.operating_hours).map(day => `<p>${capitalizeFirstLetter(day)}: ${suggestion.operating_hours[day] || 'no information'}</p>`).join('');
    }
    return `
        <p>Description: ${suggestion.description || 'no description available'}</p>
        <p>Address: ${suggestion.address || 'no address available'}</p>
        ${operatingHoursHTML}
        <p>Phone: ${suggestion.phone || 'no phone available'}</p>
        <p>Website: <a href="${suggestion.website}" target="_blank">${suggestion.website || 'no website available'}</a></p>
    `;
}

async function displayDirectionsInModal(startAddress, endAddress) {
    try {
        const response = await fetch(`/api/directions?startAddress=${encodeURIComponent(startAddress)}&endAddress=${encodeURIComponent(endAddress)}`);
        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        const directions = await response.json();
        console.log('Directions:', directions);
        showDirectionsInModal(directions);
    } catch (error) {
        console.error('Failed to fetch directions:', error);
        alert('Failed to fetch directions.');
    }
}

function showDirectionsInModal(directions) {
    const modalBody = document.getElementById('directionsModalBody');
    let directionsHTML = '';

    directions.forEach(direction => {
        directionsHTML += `
            <div class="direction-detail">
                ${direction.travel_Mode ? `<p class="travel-mode"><strong>Travel Mode:</strong> ${direction.travel_Mode}</p>` : ''}
                ${direction.via ? `<p class="via"><strong>Via:</strong> ${direction.via}</p>` : '<p class="via"><strong>Via:</strong> N/A</p>'}
                ${direction.formatted_Distance ? `<p class="distance"><strong>Distance:</strong> ${direction.formatted_Distance}</p>` : ''}
                ${direction.formatted_Duration ? `<p class="duration"><strong>Duration:</strong> ${direction.formatted_Duration}</p>` : ''}
                ${direction.trips ? direction.trips.map(trip => `
                    <div class="trip">
                        ${trip.travel_mode ? `<p class="trip-mode"><strong>Travel Mode:</strong> ${trip.travel_mode}</p>` : ''}
                        ${trip.title ? `<p class="trip-title"><strong>Title:</strong> ${trip.title}</p>` : '<p class="trip-title"><strong>Title:</strong> N/A</p>'}
                        ${trip.formatted_distance ? `<p class="trip-distance"><strong>Distance:</strong> ${trip.formatted_distance}</p>` : ''}
                        ${trip.formatted_duration ? `<p class="trip-duration"><strong>Duration:</strong> ${trip.formatted_duration}</p>` : ''}
                        ${trip.details ? trip.details.map(detail => `
                            <div class="detail">
                                ${detail.title ? `<p class="detail-title"><strong>Detail Title:</strong> ${detail.title}</p>` : ''}
                                ${detail.action ? `<p class="detail-action"><strong>Action:</strong> ${detail.action}</p>` : ''}
                                ${detail.extensions ? `<p class="detail-extensions"><strong>Extensions:</strong> ${detail.extensions.join(', ')}</p>` : ''}
                            </div>
                        `).join('') : ''}
                    </div>
                `).join('') : ''}
            </div>
        `;
    });

    modalBody.innerHTML = directionsHTML;
}


// Utility functions to get current event and selected place, replace with actual logic
function getCurrentEvent() {
    const currentEventElement = document.querySelector('.current-event'); // Example class to identify the current event
    if (currentEventElement) {
        return {
            address: currentEventElement.getAttribute('data-address')
        };
    }
    return null;
}
// Get the modal
const modal = document.getElementById('directionsModal');

// Get the <span> element that closes the modal
const span = document.getElementsByClassName('close')[0];

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = 'none';
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = 'none';
    }
}

// Function to open the modal
function openModal() {
    modal.style.display = 'block';
}
