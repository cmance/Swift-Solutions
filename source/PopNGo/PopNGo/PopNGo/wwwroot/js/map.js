import { fetchEvents } from './eventsAPI.js';


window.initMap = function () {
    var monmouth = { lat: 44.848, lng: -123.229 }; //Hardcoded Monmouth, Oregon coordinates for now
    var map = new google.maps.Map(document.getElementById('DEMO_MAP_ID'), {
        zoom: 15,
        center: monmouth
    });

    let selectedEvent = null;

    fetchEvents().then(response => {
        // Access the data property of the response
        const events = response.data;
        // console.log(events);
        events.forEach(event => {
            console.log(event);

            // Add a marker on the map for the event
            const lat = event.venue && event.venue.latitude ? event.venue.latitude : 44.848;
            const lng = event.venue && event.venue.longitude ? event.venue.longitude : -123.229;
            // console.log(lat, lng);
            const position = { lat, lng };
            const marker = new google.maps.Marker({
                position,
                map,
                title: event.name
            });

            marker.addListener('click', function () {
                var mapDiv = document.getElementById('DEMO_MAP_ID');
                var eventInfoDiv = document.getElementById('EVENT_INFO');

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

                    var nameElement = document.getElementById('NAME');
                    var descriptionElement = document.getElementById('DESCRIPTION');
                    var dateElement = document.getElementById('DATE');
                    var imageElement = document.getElementById('IMAGE');

                    nameElement.textContent = event.name;
                    descriptionElement.textContent = event.description;
                    dateElement.textContent = formatStartTime(event.start_time);

                    if (event.thumbnail) {
                        imageElement.src = event.thumbnail;
                    } else {
                        imageElement.src = '../media/images/400X400_placeholder.png';
                    }

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
    if (document.getElementById('DEMO_MAP_ID')) {
        loadMapScript();
    }
};