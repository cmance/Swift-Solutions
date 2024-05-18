import { getAllUserEventsFromItinerary } from './api/itinerary/itineraryApi.js';
import { capitalizeFirstLetter } from './util/capitalizeFirstLetter.js';
import { getPlaceSuggestions } from './api/itinerary/placeSuggestion.js';
import { buildItinerary } from './ui/buildItineraryAccordion.js';
import { buildItineraryEvent } from './ui/buildItineraryEvent.js';

document.addEventListener('DOMContentLoaded', async () => {
    // const imgElement = document.createElement('img');
    // imgElement.src = "https://lh5.googleusercontent.com/p/AF1QipN0DMh5JuH9llw9JY3XHaq0HmclMYv28eJB03Yu=w128-h92-k-no"; // Test URL, replace with your own if needed
    // imgElement.alt = "Testing Image Load";
    // imgElement.style.display = "none"; // Hide since it's only for testing

    // imgElement.onerror = () => {
    //     console.log("Image failed to load");
    // };
    // imgElement.onload = () => {
    //     console.log("Image loaded successfully");
    // };

    // document.body.appendChild(imgElement);

    try {
        const itineraries = await getAllUserEventsFromItinerary();
        
        if (itineraries && itineraries.length > 0) {
            const accordionExample = document.getElementById('itinerary-content');
            // let accordionHtml = '';
            itineraries.forEach((itinerary, index) => {
                // accordionHtml += createAccordionHtml(itinerary, index);
                createAccordionHtml(itinerary, index);
            });
            // accordionExample.innerHTML = accordionHtml;
            document.getElementById("login-prompt").style.display = "none";
            document.getElementById("no-itinerary-message").style.display = "none";
            accordionExample.style.display = "block"; // Ensure it's visible if there are items
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

    // attachDeleteEventListeners();
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

    const itineraryElement = buildItinerary({
        index: index,
        title: itinerary.itineraryTitle,
        onDelete: (button) => deleteItinerary(button, itinerary.id),
    });

    itinerary.events.forEach(event => {
        const eventElement = buildItineraryEvent({
            itineraryId: itinerary.id,
            apiEventID: event.apiEventID,
            img: event.eventImage,
            title: event.eventName,
            date: new Date(event.eventDate),
            tags: event.tags,
            address: event.eventLocation,
            latitude: event.latitude,
            longitude: event.longitude,
            onDelete: () => deleteEvent(event.apiEventID, itinerary.id),
        });

        itineraryElement.getElementById('events-container').appendChild(eventElement);
    });

    itineraryContainer.appendChild(itineraryElement);

    // let eventsHtml = itinerary.events.map(event => createEventHtml(event, itinerary.id)).join(''); // Ensure itinerary.id is the correct ID
    // return `
    //     <div class="accordion-item pb-3" style="background-color: transparent; border: none;">
    //         <h2 class="accordion-header" id="heading${index}">
    //             <button aria-label="View itinerary dropdown button" class="accordion-button collapsed" id="accordion-header-bg" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${index}" aria-expanded="false" aria-controls="collapse${index}">
    //                 <span class="text-light" contenteditable="true">${itinerary.itineraryTitle}</span>
    //                 <button aria-label="Delete itinerary button." class="btn btn-danger float-end delete-itinerary-btn mx-4" data-itinerary-id="${itinerary.id}"><i class="fa fa-trash" aria-hidden="true"></i></button>
    //             </button>
    //         </h2>
    //         <div id="collapse${index}" class="accordion-collapse collapse" aria-labelledby="heading${index}" data-bs-parent="#accordionExample">
    //             <div class="accordion-body">
    //                 ${eventsHtml}
    //             </div>
    //         </div>
    //     </div>
    // `;
}

// function createEventHtml(eventData, itineraryId) {
//     const eventImage = eventData.eventImage || '/media/images/placeholder_event_card_image.png'; // Default image if not provided
//     return `
//         <div class="single-timeline-area" data-event-id="${eventData.apiEventID}" data-itinerary-id="${itineraryId}">
//             <div class="row">
//                 <div class="col">
//                     <div class="card mb-3 bg-dark text-white">
//                         <div class="row g-0 align-items-center">
//                             <div class="col-md-4">
//                                 <img src="${eventImage}" onerror="this.onerror=null; this.src='/media/images/placeholder_event_card_image.png';" class="img-fluid rounded-start img-event" alt="${eventData.eventName} Image">
//                             </div>
//                             <div class="col-md-7">
//                                 <div class="card-body px-4 mx-4">
//                                     <h5 class="card-title pt-4">${eventData.eventName}</h5>
//                                     <p class="text-muted">${formatStartTime(eventData.eventDate)}</p>
//                                     <p class="card-text">${eventData.eventLocation}</p>
//                                 </div>
//                             </div>
//                             <div class="col-md-1">
//                                 <button aria-label="Delete event in itinerary button" class="btn btn-danger delete-event-btn" id="delete-event-button" data-event-id="${eventData.apiEventID}" data-itinerary-id="${itineraryId}">
//                                     <i class="fa fa-trash" aria-hidden="true"></i>
//                                 </button>
//                             </div>
//                         </div>
//                     </div>
//                     <div class="single-timeline-area p-0 px-0  pb-3" data-event-id="${eventData.apiEventID}" data-itinerary-id="${itineraryId}" data-latitude="${eventData.latitude || ''}" data-longitude="${eventData.longitude || ''}">
//                         <p class="text-light">Recommended Places Near the event</p>
//                         <div class="btn btn-warning text-light" id="hotels-button-${eventData.latitude}-${eventData.longitude}">Hotels</div>
//                         <div class="btn btn-warning text-light" id="food-button-${eventData.latitude}-${eventData.longitude}">Food</div>
//                         <div class="btn btn-warning text-light" id="coffee-button-${eventData.latitude}-${eventData.longitude}">Coffee</div>
//                         <div class="pt-4 text-light" id="suggestions-container-${eventData.latitude}-${eventData.longitude}"></div>
//                     </div>
//                 </div>
//             </div>
//         </div>
//     `;
// }

// function attachDeleteEventListeners() {
//     // Attach event listeners to delete individual events
//     document.querySelectorAll('.delete-event-btn').forEach(button => {
//         button.addEventListener('click', function () {
//             const apiEventID = this.getAttribute('data-event-id');
//             const itineraryId = this.getAttribute('data-itinerary-id');
//             if (!apiEventID || !itineraryId) {
//                 console.error('Missing event or itinerary ID');
//                 return;
//             }

//             console.log(itineraryId, apiEventID);
//             deleteEvent(apiEventID, itineraryId);
//         });
//     });

//     // Attach event listeners to delete itineraries
//     document.querySelectorAll('.delete-itinerary-btn').forEach(button => {
//         button.addEventListener('click', function () {
//             const itineraryId = this.getAttribute('data-itinerary-id');
//             deleteItinerary(itineraryId);
//         });
//     });
// }

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
        // const itemToRemove = document.querySelector(`button[data-itinerary-id="${itineraryId}"]`).closest('.accordion-item');
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
