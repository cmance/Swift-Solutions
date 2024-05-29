import { getNearestCityAndStateAndCountry } from '../util/getNearestCityAndStateAndCountry.js';
import { formatDate, formatHourMinute } from '../util/formatStartTime.js';
import { getBookmarkLists } from '../api/bookmarkLists/getBookmarkLists.js';
import { UnauthorizedError } from '../util/errors.js';
import { onPressSaveToBookmarkList } from '../util/onPressSaveToBookmarkList.js';
import { getRecommendedEvents } from "../api/recommendations/getRecommendedEvents.js";

let userLocation = {}

document.addEventListener("DOMContentLoaded", async function () {
    configureCarousel();

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {

            userLocation["lat"] = position.coords.latitude;
            userLocation["long"] = position.coords.longitude;

            const { city, state, country } = await getNearestCityAndStateAndCountry(position.coords.latitude, position.coords.longitude);
            let queryString = `${city}, ${state}, ${country}`;
            let recommendedEvents = await getRecommendedEvents(queryString);
            console.log(recommendedEvents);
            buildRecommendationCard(recommendedEvents);

            // Remove the loading screen from the carousel
            const loadingScreen = document.getElementById('loadingScreen');
            loadingScreen.parentNode.removeChild(loadingScreen);

            // Start the carousel from the first event
            $('.carousel').carousel(0);

        }, async function (error) {
            if (error.code === error.PERMISSION_DENIED) {
                document.getElementsByClassName("location-permissions-alert")[0].style.display = "block";
            }
            
        });
    }
}, { once: true });



async function buildRecommendationCard(recommendedEvents) {
    // Get the template
    const template = document.getElementById('recommended-events-template');

    // Get the container where the cards will be added
    const container = document.querySelector('.carousel-inner');

    // Get the "Out of new events!" slide
    const outOfEventsSlide = document.querySelector('.out-of-recommended-events-container').parentNode;

    // Iterate over the recommended events
    for (const event of recommendedEvents) {
        // Create a new div for the carousel item
        const carouselItem = document.createElement('div');
        carouselItem.className = 'carousel-item recommended-event-card-container';

        // Clone the template
        const clone = template.content.cloneNode(true);

        // Populate the clone with the event data
        clone.querySelector('.recommended-events-title').textContent = event.eventName;
        if (event.eventDescription === null) {
            clone.querySelector('.recommended-events-description').textContent = "No description available";
        }
        else {
            clone.querySelector('.recommended-events-description').textContent = event.eventDescription;
        }

        clone.querySelector('.recommended-events-date-1').textContent = formatDate(event.eventDate);
        clone.querySelector('.recommended-events-date-2').textContent = formatHourMinute(event.eventDate);
        clone.querySelector('.recommended-events-address-1').textContent = splitAddress(event.eventLocation)[0];
        clone.querySelector('.recommended-events-address-2').textContent = splitAddress(event.eventLocation)[1];
        if (event.eventImage === null | event.eventImage === "" | event.eventImage === undefined) {
            clone.querySelector('.recommended-events-image-container').style.backgroundImage = `url('/media/images/placeholder_event_card_image.png')`;
        }
        else {
            clone.querySelector('.recommended-events-image-container').style.backgroundImage = `url('${event.eventImage}')`;
        }

        //Bookmark
        const bookmarkDropdownMenu = clone.getElementById("recommended-event-card-bookmark-dropdown-menu");
        bookmarkDropdownMenu.innerHTML = ''

        let bookmarkLists = [];
        try {
            bookmarkLists = await getBookmarkLists();
        } catch (error) {
            if (error instanceof UnauthorizedError) {
                console.error("Unauthorized to get bookmark lists");
            } else {
                console.error("Error getting bookmark lists", error);
            }
        }
        if (!bookmarkLists || bookmarkLists.length === 0) {
            // If no bookmark list names are provided, remove the dropdown menu
            clone.querySelector('#recommended-event-card-bookmark-button').remove();
        } else {
            console.log(bookmarkLists);
            bookmarkLists.forEach(bookmarkList => {
                const bookmarkItem = document.createElement('a');
                bookmarkItem.classList.add('dropdown-item');
                bookmarkItem.textContent = bookmarkList.title;
                bookmarkItem.addEventListener('click', (mouseEvent) => {
                    // Prevent the event from bubbling up (stop the event from triggering the card click event)
                    if (mouseEvent && mouseEvent.stopPropagation) mouseEvent.stopPropagation();
                    onPressSaveToBookmarkList(event.apiEventID, bookmarkList.title)
                });
                bookmarkDropdownMenu.appendChild(bookmarkItem);
            });
        }
        // View Original Post Link
        if (!event.eventOriginalLink | event.eventOriginalLink === "" | event.eventOriginalLink === undefined) {
            clone.querySelector('#original-post-link').style.display = 'none';
        }
        else {
            const viewOriginalPostLink = clone.getElementById('original-post-link');
            viewOriginalPostLink.href = event.eventOriginalLink;
            viewOriginalPostLink.target = '_blank';
        }

        // Buy Tickets
        const buyTicketsButton = clone.getElementById('buyTicketsBtn');
        buyTicketsButton.innerHTML = 'Buy Tickets'; // Set the button title

        // Select the existing dropdown menu
        const dropdownMenu = clone.getElementById('buyTicketsDropdownMenu');

        if (!event.ticketLinks || event.ticketLinks.length === 0) {
            buyTicketsButton.classList.add('disabled'); // Disable the button
            buyTicketsButton.title = "No ticket links available for this event.";
        } else {
            buyTicketsButton.classList.remove('disabled'); // Enable the button
            buyTicketsButton.title = "Click to view ticket options for this event.";
            dropdownMenu.innerHTML = ''; // Clear the default dropdown options
            event.ticketLinks.forEach(ticketLink => {
                const ticketLinkElement = document.createElement('a');
                ticketLinkElement.classList.add('dropdown-item');
                ticketLinkElement.textContent = ticketLink.source;
                ticketLinkElement.href = ticketLink.link;
                ticketLinkElement.target = '_blank';

                // Append the dropdown item to the dropdown menu, not the button
                dropdownMenu.appendChild(ticketLinkElement);
            });
        }

        // Add to Google Calendar
        const addToCalendarButton = clone.getElementById('recommended-event-google-cal');

        addToCalendarButton.addEventListener('click', () => {
            const eventTitle = event.eventName;
            const eventDate = new Date(event.eventDate); // Convert string to Date
            const eventDescription = event.eventDescription;


            const startDateTime = eventDate.toISOString().replace(/-|:|\.\d+/g, '');
            const endDateTime = eventDate.toISOString().replace(/-|:|\.\d+/g, '');

            const calendarUrl = 'https://calendar.google.com/calendar/render?action=TEMPLATE&text=' + encodeURIComponent(eventTitle) +
                '&dates=' + encodeURIComponent(startDateTime) + '/' + encodeURIComponent(endDateTime) +
                '&details=' + encodeURIComponent(eventDescription) +
                '&location=&sf=true&output=xml';
            window.open(calendarUrl, '_blank');
        });

        // Add the clone to the carousel item
        carouselItem.appendChild(clone);

        // Add the carousel item to the container
        container.appendChild(carouselItem);
    }

    // If there are any events, make the first event slide active and remove the active class from the "Out of new events!" slide
    if (container.children.length > 1) {
        outOfEventsSlide.classList.remove('active');
        container.children[1].classList.add('active');
    }
}

function configureCarousel() {
    var carousel = document.getElementById('carouselExampleIndicators');
    var bsCarousel = new bootstrap.Carousel(carousel, {
        interval: false, // Disable auto sliding
        touch: true, // Enable swiping
        ride: false // Disable auto-start
    });

    carousel.addEventListener('slid.bs.carousel', function () {
        bsCarousel.pause(); // Pause the carousel after a slide transition
    });
}
function splitAddress(address) {
    const commaIndices = [];
    for (let i = 0; i < address.length; i++) {
        if (address[i] === ',') commaIndices.push(i);
    }
    const secondLastCommaIndex = commaIndices[commaIndices.length - 2];  // Find the index of the second last comma
    const part1 = address.substring(0, secondLastCommaIndex).trim();  // Get the substring before the second last comma
    const part2 = address.substring(secondLastCommaIndex + 1).trim();  // Get the substring after the second last comma
    return [part1, part2];  // Return the parts as an array
}
