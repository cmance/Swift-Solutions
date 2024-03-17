// Purpose: Contains the javascript functions for the history page.

// Fetch event data and display it on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log("History page loaded.")
    fetchAndDisplayEvents();
});


function constructEventCard(event) {
    const template = document.getElementById('event-card-template');
    const eventCard = template.content.cloneNode(true);
    eventCard.querySelector('#event-name').textContent = event.eventName || 'Event Name Not Available';
    eventCard.querySelector('#event-description').textContent = event.eventDescription || 'No description available.';
    eventCard.querySelector('#event-datetime').textContent = event.eventDate || 'Event date not found';
    eventCard.querySelector('#event-location').textContent = event.eventLocation || 'Location information not available';
    return eventCard;
}

// Append event cards to the container element
function displayEvents(events) {
    const container = document.getElementById('event-history-card-container');
    if (!container) {
        console.error('Container element #event-history-card-container not found.');
        return;
    }

    // Clear the container
    container.innerHTML = '';

    // Append event cards to the container
    events.forEach(event => {
        const eventCard = constructEventCard(event);
        container.appendChild(eventCard);
    });
}

// Fetch event data and display it
async function fetchAndDisplayEvents() {
    fetch("/api/EventHistoryApi/EventHistory")
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    document.getElementById("login-prompt").style.display = "block";
                    throw new Error('Unauthorized');
                } else if
                    (response.status === 404) {
                    document.getElementById("no-history-message").style.display = "block";
                    throw new Error('No history found');
                }
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
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
}