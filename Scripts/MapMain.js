var map, places, iw;
var markers = [];
var localFavs = [];
var searchTimeout;
var centerMarker;
var autocomplete;
var hostnameRegexp = new RegExp('^https?://.+?/');

function initMap() {
    var myLatlng = new google.maps.LatLng(38.2157472, -85.7590743);
    var myOptions = {
        zoom: 17,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);
    places = new google.maps.places.PlacesService(map);
    google.maps.event.addListener(map, 'tilesloaded', tilesLoaded);

    document.getElementById('keyword').onkeyup = function (e) {
        if (!e) var e = window.event;
        if (e.keyCode != 13) return;
        document.getElementById('keyword').blur();
        search(document.getElementById('keyword').value);
    }

    var typeSelect = document.getElementById('type');
    typeSelect.onchange = function () {
        search();
    };

    var rankBySelect = document.getElementById('rankBy');
    rankBySelect.onchange = function () {
        search();
    };
}

function tilesLoaded() {
    search();
    google.maps.event.clearListeners(map, 'tilesloaded');
    google.maps.event.addListener(map, 'zoom_changed', searchIfRankByProminence);
    google.maps.event.addListener(map, 'dragend', search); 
}

function searchIfRankByProminence() {
    if (document.getElementById('rankBy').value == 'prominence' || document.getElementById('fav').checked) {
        search();
    }
}

function search() {
    clearResults();
    clearMarkers();
    clearLocalFavs();

    if (document.getElementById('fav').checked) {
        var actionUrl = GetActionURL();
        var bounds = map.getBounds();
        var neCoords = bounds.getNorthEast();
        var swCoords = bounds.getSouthWest();

        // get action url from view and pass bounds of map.
        actionUrl += ('?neLat=' + neCoords.lat() + '&neLong=' + neCoords.lng() + '&swLat=' + swCoords.lat() + '&swLong=' + swCoords.lng());

        var srchtype = parseInt(document.getElementById('RestaurantType').value);
        var searchterm = document.getElementById('favKeyword').value;

        //alert(srchtype + ' ' + searchterm);

            actionUrl += '&srchType=' + srchtype;
            actionUrl += '&srchTerm=' + searchterm;

        // Async to server and get favs in the bounds.
        $.getJSON(actionUrl, displayFavs);
    }
    else {
        if (searchTimeout) {
            window.clearTimeout(searchTimeout);
        }
        searchTimeout = window.setTimeout(reallyDoSearch, 500);
    }
}

function reallyDoSearch() {
    var type = document.getElementById('type').value;
    var keyword = document.getElementById('keyword').value;
    var rankBy = document.getElementById('rankBy').value;

    var search = {};

    if (keyword) {
        search.keyword = keyword;
    }

    if (type != 'establishment') {
        search.types = [type];
    }

    if (rankBy == 'distance' && (search.types || search.keyword)) {
        search.rankBy = google.maps.places.RankBy.DISTANCE;
        search.location = map.getCenter();
        centerMarker = new google.maps.Marker({
            position: search.location,
            animation: google.maps.Animation.DROP,
            map: map
        });
    } else {
        search.bounds = map.getBounds();
    }

    places.search(search, function (results, status) {
        if (status == google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {
                var icon = '/Content/Images/icons/numbered/number_' + (i + 1) + '.png';
                markers.push(new google.maps.Marker({
                    position: results[i].geometry.location,
                    animation: google.maps.Animation.DROP,
                    icon: icon
                }));
                google.maps.event.addListener(markers[i], 'click', getDetails(results[i], i));
                window.setTimeout(dropMarker(i), i * 100);
                addResult(results[i], i);
            }
        }
    });
}

function displayFavs(response) {
    if (response != null) {

        for (var i = 0; i < response.length; i++) {
            var icon = '/Content/Images/icons/restaurant/' + response[i].IconName + '.png';
            var mylatlng = { lat: response[i].GeoLat, lng: response[i].GeoLong }
            var favmarker = new google.maps.Marker({
                position: mylatlng,
                map: map,
                animation: google.maps.Animation.DROP,
                icon: icon
            });
            localFavs.push(favmarker);
            google.maps.event.addListener(localFavs[i], 'click', getFavDetails(response[i], i));
            //window.setTimeout(dropMarker(i), i * 100);
            addFavResult(response[i], i);
        }
    }
}

function clearMarkers() {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
    if (centerMarker) {
        centerMarker.setMap(null);
    }
}

function clearLocalFavs() {

    for (var i = 0; i < localFavs.length; i++) {
        localFavs[i].setMap(null);
    }
    localFavs = [];
}

function dropMarker(i) {
    return function () {
        if (markers[i]) {
            markers[i].setMap(map);
        }
    }
}

// Adds a result to the place table listing
function addResult(result, i) {
    var results = document.getElementById('results');
    var tr = document.createElement('tr');
    tr.onclick = function () {
        google.maps.event.trigger(markers[i], 'click');
    };

    var iconTd = document.createElement('td');
    var nameTd = document.createElement('td');
    var icon = document.createElement('img');
    icon.src = '/Content/Images/icons/numbered/number_' + (i + 1) + '.png';
    icon.setAttribute('class', 'placeIcon');
    icon.setAttribute('className', 'placeIcon');
    var name = document.createTextNode(result.name);
    iconTd.appendChild(icon);
    nameTd.appendChild(name);
    tr.appendChild(iconTd);
    tr.appendChild(nameTd);
    results.appendChild(tr);
}

// Adds a result to the fav table listing
function addFavResult(result, i) {
    var results = document.getElementById('results');
    var tr = document.createElement('tr');
    tr.onclick = function () {
        google.maps.event.trigger(localFavs[i], 'click');
    };

    var iconTd = document.createElement('td');
    var nameTd = document.createElement('td');
    var icon = document.createElement('img');
    icon.src = '/Content/Images/icons/restaurant/' + result.IconName + '.png';
    icon.setAttribute('class', 'placeIcon');
    icon.setAttribute('className', 'placeIcon');
    var name = document.createTextNode(result.PlaceName);
    iconTd.appendChild(icon);
    nameTd.appendChild(name);
    tr.appendChild(iconTd);
    tr.appendChild(nameTd);
    results.appendChild(tr);
}

function clearResults() {
    var results = document.getElementById('results');
    while (results.childNodes[0]) {
        results.removeChild(results.childNodes[0]);
    }
}

function getDetails(result, i) {
    return function () {
        places.getDetails({
            reference: result.reference
        }, showInfoWindow(i));
    }
}

function getFavDetails(result, i) {
    return function (place, status) {
        if (iw) {
            iw.close();
            iw = null;
        }
            iw = new google.maps.InfoWindow({
                content: getFavIWContent(result)
            });
            iw.open(map, localFavs[i]);
    }
}

function showInfoWindow(i) {
    return function (place, status) {
        if (iw) {
            iw.close();
            iw = null;
        }

        if (status == google.maps.places.PlacesServiceStatus.OK) {
            iw = new google.maps.InfoWindow({
                content: getIWContent(place)
            });
            iw.open(map, markers[i]);
        }
    }
}

function getIWContent(place) {
    var content = '';
    content += '<table>';
    content += '<tr class="iw_table_row">';
    content += '<td style="text-align: right"><img class="hotelIcon" src="' + place.icon + '"/></td>';
    content += '<td><b><a href="' + place.url + '">' + place.name + '</a></b></td></tr>';
    content += '<tr class="iw_table_row"><td class="iw_attribute_name">Address:</td><td>' + place.vicinity + '</td></tr>';
    if (place.formatted_phone_number) {
        content += '<tr class="iw_table_row"><td class="iw_attribute_name">Telephone:</td><td>' + place.formatted_phone_number + '</td></tr>';
    }
    if (place.rating) {
        var ratingHtml = '';
        for (var i = 0; i < 5; i++) {
            if (place.rating < (i + 0.5)) {
                ratingHtml += '&#10025;';
            } else {
                ratingHtml += '&#10029;';
            }
        }
        content += '<tr class="iw_table_row"><td class="iw_attribute_name">Rating:</td><td><span id="rating">' + ratingHtml + '</span></td></tr>';
    }
    if (place.website) {
        var fullUrl = place.website;
        var website = hostnameRegexp.exec(place.website);
        if (website == null) {
            website = 'http://' + place.website + '/';
            fullUrl = website;
        }
        content += '<tr class="iw_table_row"><td class="iw_attribute_name">Website:</td><td><a href="' + fullUrl + '">' + website + '</a></td></tr>';
    }
    content += '<tr class="iw_table_row"><td class="iw_attribute_name">Favoritizer:</td><td><a href="/Restaurant/Place?placeID=' + place.place_id
        + '&name=' + place.name
        + '&address=' + place.vicinity
        + '&phone=' +place.formatted_phone_number
        + '&lat=' + place.geometry.location.lat()
        + '&lng=' + place.geometry.location.lng()
        + '&url=' + place.url
        + '&site=' + place.website
        + '" >Restaurant Page</a></td></tr>';
    content += '</table>';
    return content;
}

function getFavIWContent(place) {
    var content = '';
    content += '<table>';
    content += '<tr class="iw_table_row">';
    content += '<td colspan="2" style="text-align: left"><img class="hotelIcon" src="/Content/Images/icons/restaurant/'
               + place.IconName + '.png"/><b><a href="' + place.website + '">'
               + place.PlaceName 
               + '</a></b>&nbsp;<i style="color:gold;" class="fa fa-star fa-lg"> </i> ' + place.favcount + '</td>';
    content += '</tr>';
    if (place.vicinity) {
        content += '<tr class="iw_table_row"><td class="iw_attribute_name">Address:</td><td>' + place.vicinity + '</td></tr>';
    }
    if (place.phone) {
        content += '<tr class="iw_table_row"><td class="iw_attribute_name">Telephone:</td><td>' + place.phone + '</td></tr>';
    }
    // Add row here for favorites count and likes
    //
    content += '<tr class="iw_table_row"><td class="iw_attribute_name">Food Type:</td><td>' + place.rtype + '</td></tr>';
    content += '<tr class="iw_table_row"><td class="iw_attribute_name">Favoritizer:</td><td><a href="/Restaurant/Details/' + place.id + '" >Restaurant Page</a></td></tr>';
    content += '</table>';
    return content;
}