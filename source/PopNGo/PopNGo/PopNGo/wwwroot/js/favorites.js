import { formatStartTime } from './Helper-Functions/formatStartTime.js';


function fetchUserFavorites() {
    fetch('/api/FavoritesApi/GetUserFavorites')
        .then(response => {
            if (response.status === 401) {
                // Unauthorized, tell the user to log in or sign up
                document.getElementById('favorite-events-title').style.display = 'none'; // Hide the title if the user is not logged in
                document.getElementById('login-prompt').style.display = 'block';
                throw new Error('Unauthorized');
            } else if (response.status === 404) {
                // Resource not found, tell the user they have no favorites
                document.getElementById('favorite-events-title').style.display = 'none';
                document.getElementById('no-favorites-message').style.display = 'block';
                throw new Error('No favorites found');
            }
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            data.forEach(event => {
                const eventCard = constructEventCard(event);
                document.getElementById('favorite-events-container').appendChild(eventCard);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function constructEventCard(event) {
    const eventCard = document.createElement('div');
    eventCard.classList.add('event-card');

    const eventInfo = document.createElement('div');
    eventInfo.classList.add('event-info');
    eventCard.appendChild(eventInfo);

    const eventName = document.createElement('h2');
    eventName.textContent = event.eventName;
    eventInfo.appendChild(eventName);

    const eventDescription = document.createElement('p');
    eventDescription.textContent = event.eventDescription;
    eventInfo.appendChild(eventDescription);

    const eventDate = document.createElement('p');
    eventDate.textContent = formatStartTime(event.eventDate);
    eventInfo.appendChild(eventDate);

    const heart = new Image();
    heart.alt = 'Unfavorite Event';
    heart.classList.add('heart', 'heart-position');
    heart.src = '/media/images/heart-filled.svg'; // Path to the filled heart image
    heart.style.cursor = 'pointer';

    heart.addEventListener('click', () => {
        fetch(`/api/FavoritesApi/RemoveFavorite`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ApiEventID: event.apiEventID || "No ID available",
                EventDate: event.eventDate || "No date available",
                EventName: event.eventName || "No name available",
                EventDescription: event.eventDescription || "No description available",
                EventLocation: event.full_Address || "No location available",
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                // Remove the event card from the DOM
                eventCard.remove();
                if (document.getElementById('favorite-events-container').childElementCount === 0) { // If there are no more favorite events, show the no-favorites-message
                    document.getElementById('favorite-events-title').style.display = 'none';
                    document.getElementById('no-favorites-message').style.display = 'block';
                }
                
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    });

    eventCard.appendChild(heart);


    return eventCard;
}

document.addEventListener('DOMContentLoaded', function () {
    console.log("Favorites page loaded.")
    fetchUserFavorites();
});