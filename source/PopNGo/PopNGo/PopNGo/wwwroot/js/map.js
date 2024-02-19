import { fetchEventData } from './eventsAPI.js';


window.initMap = function () {
    var monmouth = { lat: 44.848, lng: -123.229 }; //Hardcoded Monmouth, Oregon coordinates for now
    var map = new google.maps.Map(document.getElementById('demo-map-id'), {
        zoom: 15,
        center: monmouth
    });

    const loadingOverlay = document.createElement('div');
    loadingOverlay.id = 'loading-overlay';

    document.getElementById('demo-map-id').appendChild(loadingOverlay);

    let selectedEvent = null;

    fetchEventData("Events in Monmouth, Oregon").then(response => {
        console.log(response);
        // Access the data property of the response
        const events = response;

        document.getElementById('demo-map-id').removeChild(loadingOverlay);
        // console.log(events);
        events.forEach(event => {
            console.log(event);

            // Add a marker on the map for the event
            const lat = event.latitude ? event.latitude : 44.848;
            const lng = event.longitude ? event.longitude : -123.229;
            // console.log(lat, lng);
            const position = { lat, lng };
            const marker = new google.maps.Marker({
                position,
                map,
                title: event.eventName
            });

            marker.addListener('click', function () {
                var mapDiv = document.getElementById('demo-map-id');
                var eventInfoDiv = document.getElementById('event-info');

                if (selectedEvent === event) {
                    // If the same event is clicked again, reset the map and event info divs to their initial sizes
                    mapDiv.style.width = '100vw';
                    eventInfoDiv.style.width = '0';
                    eventInfoDiv.style.display = 'none';
                    selectedEvent = null;
                } else {
                    // If a different event is clicked, resize the map and event info divs and populate the event info div with the event details
                    mapDiv.style.width = '50vw';
                    eventInfoDiv.style.width = '50vw';

                    var nameElement = document.getElementById('name');
                    var descriptionElement = document.getElementById('description');
                    var dateElement = document.getElementById('date');
                    var imageElement = document.getElementById('image');

                    nameElement.textContent = event.eventName;
                    descriptionElement.textContent = event.eventDescription;
                    dateElement.textContent = formatStartTime(event.eventStartTime);

                    imageElement.src = '/media/images/gifs/loadingspinner.gif';

                    // Add an onload event listener to the image element
                    imageElement.onload = function() {
                        // The image has loaded
                        if (event.eventThumbnail) {
                            imageElement.src = event.eventThumbnail;
                        } else {
                            imageElement.src = '../media/images/400X400_placeholder.png';
                        }
                    };

                    eventInfoDiv.style.display = 'block';

                    setTimeout(function () {
                        google.maps.event.trigger(map, 'resize');
                    }, 500);

                    selectedEvent = event;
                }
            });
        });
    });
}

function formatStartTime(dateString) {
    const options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(dateString).toLocaleDateString(undefined, options);
}

async function loadMapScript() {
    // Fetch the API key from the server
    const response = await fetch('/api/MapApi/GetApiKey');
    const apiKey = await response.text();
    var script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&loading=async&callback=initMap&libraries=maps,marker&v=beta`;
    script.async = true;
    document.body.appendChild(script); // Append the script to the body instead of the head
}

window.onload = function () {
    if (document.getElementById('demo-map-id')) {
        loadMapScript();
    }
};