import { getNearestCityAndStateAndCountry } from '../util/getNearestCityAndStateAndCountry.js';
import { formatDate, formatHourMinute } from '../util/formatStartTime.js';
import { getBookmarkLists } from '../api/bookmarkLists/getBookmarkLists.js';
import { UnauthorizedError } from '../util/errors.js';
import { onPressSaveToBookmarkList } from '../util/onPressSaveToBookmarkList.js';
import { buildVenueDetailsModal } from './buildEventDetailsModal.js';
// import { getRecommendedEvents } from '../recommendations/getRecommendedEvents.js';

/**
 * Returns list of recommended events
 * @async
 * @function getRecommendedEvents
 * @returns {Promise<Array<object>>}
 */
export async function getRecommendedEvents(queryString) {
    let res = await fetch(`/api/RecommendationsApi/RecommendedEvents`)
    return await res.json();
}

let userLocation = {}
let recommendedEvents = [
    {
        apiEventID: 1,
        eventName: 'Coding Workshop',
        eventDescription: 'Join us for a fun and interactive coding workshop!Join us for a fun and interactive coding workshop!Join us for a fun and interactive coding workshop!Join us for a fun and interactive coding workshop!Join us for a fun and interactive coding workshop!',
        eventDate: '2024-05-24T16:00:00Z',
        eventLocation: '4225 North Pacific Highway West, Rickreall, OR 97371, United States',
        eventImage: 'https://via.placeholder.com/500',
        eventOriginalLink: 'https://www.google.com',
        venueName: 'Coding School',
        venuePhoneNumber: '123-456-7890',
        ticketLinks: [{ source: 'Ticketmaster', link: 'https://www.ticketmaster.com' }, { source: 'Eventbrite', link: 'https://www.eventbrite.com' }]
    },
    {
        apiEventID: 2,
        eventName: 'Yoga in the Park',
        eventDescription: 'Relax and unwind with a yoga session in the park!',
        eventDate: '2023-11-15T10:00:00Z',
        eventLocation: '1955 Salem Dallas Highway Northwest, Salem, OR 97304, United States',
        eventImage: 'https://via.placeholder.com/1000',
        eventOriginalLink: 'https://www.google.com',
        ticketLinks: [{ source: 'Woopoas', link: 'https://www.ticketmaster.com' }]
    },
    {
        apiEventID: 3,
        eventName: 'Art Class',
        eventDescription: 'Express your creativity with our art class!',
        eventDate: '2023-12-01T18:00:00Z',
        eventLocation: '4225 North Pacific Highway West, Rickreall, OR 97371, United States',
        eventImage: 'https://via.placeholder.com/100',

    }
];

document.addEventListener("DOMContentLoaded", async function () {
    configureCarousel();

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async function (position) {

            userLocation["lat"] = position.coords.latitude;
            userLocation["long"] = position.coords.longitude;

            const { city, state, country } = await getNearestCityAndStateAndCountry(position.coords.latitude, position.coords.longitude);
            let queryString = `${city}, ${state}, ${country}`;
            // let recommendedEvents = await getRecommendedEvents(queryString);
            buildRecommendationCard(recommendedEvents);

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
        clone.querySelector('.recommended-events-description').textContent = event.eventDescription;
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
                bookmarkItem.addEventListener('click', (event) => {
                    // Prevent the event from bubbling up (stop the event from triggering the card click event)
                    if (event && event.stopPropagation) event.stopPropagation();
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
        // View Venue
        clone.getElementById("view-venue-btn").addEventListener('click', async () => {
            buildVenueDetailsModal(document, event);
        });
        // const viewVenueButtons = clone.querySelectorAll('.view-venue-btn');
        // Array.from(viewVenueButtons).forEach(button => {
        //     button.addEventListener('click', async () => {
        //         buildVenueDetailsModal(document, event);
        //     });
        // });

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
            console.log(eventDate)
        
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
    bsCarousel.pause(); // Pause the carousel
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