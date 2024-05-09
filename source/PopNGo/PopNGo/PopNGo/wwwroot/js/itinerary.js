import { getAllUserEventsFromItinerary } from './api/itinerary/itineraryApi.js';
import { formatStartTime } from './util/formatStartTime.js';

document.addEventListener('DOMContentLoaded', async () => {
    try {
        await initPage();

        const accordionExample = document.querySelector('#accordionExample');

        let accordionHtml = '';
        itineraries.forEach((itinerary, index) => {
            accordionHtml += createAccordionHtml(itinerary, index);
        });

        accordionExample.innerHTML = accordionHtml;
        attachDeleteEventListeners();
    } catch (error) {
        console.error('There was an error fetching the itinerary data:', error);
    }
});

async function initPage() {
    try {
        const itineraries = await getAllUserEventsFromItinerary();
        if (itineraries && itineraries.length > 0) {
            const accordionExample = document.querySelector('#accordionExample');
            let accordionHtml = '';
            itineraries.forEach((itinerary, index) => {
                accordionHtml += createAccordionHtml(itinerary, index);
            });
            accordionExample.innerHTML = accordionHtml;
            attachDeleteEventListeners();
            document.getElementById("login-prompt").style.display = "none";
            document.getElementById("no-itinerary-message").style.display = "none";
            accordionExample.style.display = ""; // Ensure it's visible if there are items
        } else {
            displayNoItineraryMessage();
        }
    } catch (error) {
        if (error.message.includes('Unauthorized')) {
            displayLoginPrompt();
        } else {
            displayNoItineraryMessage();
        }
    }
}

async function displayLoginPrompt() {
    document.getElementById("login-prompt").style.display = "block";
    document.getElementById("accordionExample").style.display = "block";
}

async function displayNoItineraryMessage() {
    const noItineraryMsg = document.getElementById("no-itinerary-message");
    if (noItineraryMsg) {
        noItineraryMsg.style.display = "block";
    }
    document.getElementById("accordionExample").style.display = "none"; // Also hide if no itineraries are found
}
function createAccordionHtml(itinerary, index) {
    let eventsHtml = itinerary.events.map(event => createEventHtml(event, itinerary.id)).join(''); // Ensure itinerary.id is the correct ID
    return `
        <div class="accordion-item pb-3" style="background-color: transparent; border: none;">
            <h2 class="accordion-header" id="heading${index}">
                <button aria-label="View itinerary dropdown button" class="accordion-button collapsed" id="accordion-header-bg" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${index}" aria-expanded="false" aria-controls="collapse${index}">
                    <span class="text-light" contenteditable="true">${itinerary.itineraryTitle}</span>
                    <button aria-label="Delete itinerary button." class="btn btn-danger float-end delete-itinerary-btn mx-4" data-itinerary-id="${itinerary.id}"><i class="fa fa-trash" aria-hidden="true"></i></button>
                </button>
            </h2>
            <div id="collapse${index}" class="accordion-collapse collapse" aria-labelledby="heading${index}" data-bs-parent="#accordionExample">
                <div class="accordion-body">
                    ${eventsHtml}
                </div>
            </div>
        </div>
    `;
}

function createEventHtml(eventData, itineraryId) {
    return `
        <div class="single-timeline-area" data-event-id="${eventData.apiEventID}" data-itinerary-id="${itineraryId}">
            <div class="timeline-date">
                <p>${new Date(eventData.eventDate).toLocaleDateString()}</p>
            </div>
            <div class="row">
                <div class="col">
                    <div class="card mb-3 bg-dark text-white">
                        <div class="row g-0 align-items-center">
                            <div class="col-md-4">
                                <img src="${eventData.eventImage}" class="img-fluid rounded-start img-event" alt="${eventData.eventName} Image">
                            </div>
                            <div class="col-md-7">
                                <div class="card-body px-4 mx-4">
                                    <h5 class="card-title pt-4">${eventData.eventName}</h5>
                                    <p class="text-muted">${formatStartTime(eventData.eventDate)}</p>
                                    <p class="card-text">${eventData.eventLocation}</p>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <button aria-label="Delete event in itinerary button" class="btn btn-danger delete-event-btn" data-event-id="${eventData.apiEventID}" data-itinerary-id="${itineraryId}">
                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

function attachDeleteEventListeners() {
    // Attach event listeners to delete individual events
    document.querySelectorAll('.delete-event-btn').forEach(button => {
        button.addEventListener('click', function () {
            const apiEventID = this.getAttribute('data-event-id');
            const itineraryId = this.getAttribute('data-itinerary-id');
            if (!apiEventID || !itineraryId) {
                console.error('Missing event or itinerary ID');
                return;
            }

            console.log(itineraryId, apiEventID);
            deleteEvent(apiEventID, itineraryId);
        });
    });

    // Attach event listeners to delete itineraries
    document.querySelectorAll('.delete-itinerary-btn').forEach(button => {
        button.addEventListener('click', function () {
            const itineraryId = this.getAttribute('data-itinerary-id');
            deleteItinerary(itineraryId);
        });
    });
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
async function deleteItinerary(itineraryId) {
    if (!confirm('Are you sure you want to delete this itinerary?')) return;

    try {
        let response = await fetch(`/api/ItineraryApi/${itineraryId}`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        });

        if (!response.ok) throw new Error(`Error ${response.status}: ${await response.text()}`);

        // Remove the itinerary element from DOM after successful deletion
        const itemToRemove = document.querySelector(`button[data-itinerary-id="${itineraryId}"]`).closest('.accordion-item');
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