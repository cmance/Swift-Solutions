async function loadScript() {
    // Fetch the API key from the server
    const response = await fetch('/api/MapApi/GetApiKey');
    const apiKey = await response.text();

    var script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&loading=async&callback=initMap&libraries=maps,marker&v=beta`;
    script.async = true;
    document.body.appendChild(script); // Append the script to the body instead of the head
}

function initMap() {
    var monmouth = { lat: 44.848, lng: -123.229 };
    var map = new google.maps.Map(document.getElementById('DEMO_MAP_ID'), {
        zoom: 15,
        center: monmouth
    });
    var marker = new google.maps.Marker({
        position: monmouth,
        map: map
    });

    marker.addListener('click', function() {
        alert('Marker was clicked!');
    });
}

window.onload = function() {
    loadScript();
};