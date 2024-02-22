

// //Not implemented yet

// //For each favorited event, clone a card and append it to the favorite-events-container section

// function fetchUserFavorites() {
//     fetch("/api/FavoritesApi/GetUserFavorites").then(response => response.json())
//         .then(data => console.log(data))
//         .catch(error => console.error('Error fetching favorite data:', error));
// }

// function displayFavorites(favorites) {
//     const container = document.getElementById('favorite-events-container');
//     if (!container) {
//         console.error('Container element #favorite-events-container not found.');
//         return;
//     }

//     // Clear the container
//     container.innerHTML = '';

//     // Append event cards to the container
//     favorites.forEach(favorite => {
//         const eventCard = constructEventCard(favorite);
//         container.appendChild(eventCard);
//     });
// }

// function constructEventCard(event) {
//     const template = document.getElementById('event-card-template');
//     const eventCard = template.content.cloneNode(true);
//     eventCard.querySelector('#event-name').textContent = event.eventName || 'Event Name Not Available';
//     eventCard.querySelector('#event-description').textContent = event.eventDescription || 'No description available.';
//     eventCard.querySelector('#event-datetime').textContent = event.eventDate || 'Event date not found';
//     eventCard.querySelector('#event-location').textContent = event.eventLocation || 'Location information not available';
//     return eventCard;
// }

// document.addEventListener('DOMContentLoaded', function () {
//     console.log("Favorites page loaded.")
//     fetchUserFavorites();
// });