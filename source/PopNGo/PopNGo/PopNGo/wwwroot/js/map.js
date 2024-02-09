function loadScript() {
    var script = document.createElement('script');
    script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyC58HwYVwxWpP12pGtE6-B7k3sFcsIyPwM&callback=initMap&libraries=maps,marker&v=beta';
    script.async = true;
    document.head.appendChild(script);
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