function initMap() {

    var belnap = { lat: 38.2157472, lng: -85.7590743 }
    var bounds = new google.maps.LatLngBounds();
    var geocoder = new google.maps.Geocoder;
    var infowindow = new google.maps.InfoWindow;
    var hasPoints = false;
    var marker;
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 38.2157472, lng: -85.7590743 },
        zoom: 17
    });

    $("#restaurantModal").on("shown.bs.modal", function () {
        google.maps.event.trigger(map, "resize");
    });


    addSearchBox(map);

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);
        geocodeLatLng(geocoder, map, infowindow, event, marker);       
        document.getElementById("Latitude").value = event.latLng.lat();
        document.getElementById("Longitude").value = event.latLng.lng();
    });

    if (hasPoints) {
        map.fitBounds(bounds);
    }

    function placeMarker(location) {
        if (marker) {
            marker.setPosition(location);
        } else {
            var icon = '/Content/Images/icons/restaurant/restaurant.png';
            marker = new google.maps.Marker({
                position: location,
                map: map,
                draggable: true,
                animation: google.maps.Animation.BOUNCE,
                icon: icon
            });
        }
    }
}

function geocodeLatLng(geocoder, map, infowindow, event, marker) {
  geocoder.geocode({
'location': event.latLng
  }, function (results, status) {
    if (status === google.maps.GeocoderStatus.OK) {
      if (results[1]) {
        //map.setZoom(11);
            infowindow.setContent(results[1].formatted_address);
        infowindow.open(map, marker);
            } else {
        window.alert('No results found');
        }
        } else {
      window.alert('Geocoder failed due to: ' +status);
      }
      });
}


function addSearchBox(map) {
    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers = [];
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        markers.forEach(function (marker) {
            marker.setMap(null);
        });
        markers = [];

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            // Create a marker for each place.
            markers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}