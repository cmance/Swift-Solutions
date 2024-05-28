import { validateObject } from '../validation.js';
import { populateItineraryDropdown } from '../util/bindItinerarySaving.js';
import { formatTagName } from "../util/tags.js";

export const buildEventDetailsModal = (eventDetailsModalElement, props) => {

    // Set the hidden event id
    eventDetailsModalElement.querySelector('#event-id').value = props.apiEventID;

    // Set the image
    if (props.img === null || props.img === undefined) {
        eventDetailsModalElement.querySelector('#event-modal-img').src = '/media/images/placeholder_event_card_image.png';
    } else {
        eventDetailsModalElement.querySelector('#event-modal-img').src = props.img;
    }

    // Set the title
    eventDetailsModalElement.querySelector('#event-modal-title').textContent = props.title;

    // Set the datetime
    const time = props.date.toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    eventDetailsModalElement.querySelector('#event-modal-date').textContent = props.date.toLocaleString('default', { month: 'long' }) + " " + props.date.getDate() + ", " + props.date.getFullYear() + " at " + time;

    // Set the location
    eventDetailsModalElement.querySelector('#event-modal-location').textContent = props.fullAddress;

    // Set the tags
    const tagsContainer = eventDetailsModalElement.querySelector('#modal-event-card-tags-container');
    tagsContainer.innerHTML = '';

    for (let tag of props.tags) {
        const tagElement = document.createElement('span');
        tagElement.classList.add('tag');
        tagElement.textContent = formatTagName(tag.name);
        tagElement.style.color = tag.textColor;
        tagElement.style.backgroundColor = tag.backgroundColor;
        tagsContainer.appendChild(tagElement);
    }

    // Set the description
    eventDetailsModalElement.querySelector('#modal-event-description').textContent = props.description;

    // Add button to add event to Google Calendar
    const addToCalendarButtonContainer = eventDetailsModalElement.querySelector('#add-to-calendar-btn');
    addToCalendarButtonContainer.innerHTML = ''; // Remove existing button to prevent duplicates

    const addToCalendarButton = document.createElement('button');
    addToCalendarButton.textContent = 'Add Event To Google Calendar';
    addToCalendarButton.className = 'btn btn-warning'; // Add Bootstrap classes
    addToCalendarButton.addEventListener('click', () => {
        const eventTitle = props.title;
        const eventDate = props.date;
        const eventDescription = props.description;

        const startDateTime = eventDate.toISOString().replace(/-|:|\.\d+/g, '');
        const endDateTime = eventDate.toISOString().replace(/-|:|\.\d+/g, '');

        const calendarUrl = 'https://calendar.google.com/calendar/render?action=TEMPLATE&text=' + encodeURIComponent(eventTitle) +
            '&dates=' + encodeURIComponent(startDateTime) + '/' + encodeURIComponent(endDateTime) +
            '&details=' + encodeURIComponent(eventDescription) +
            '&location=&sf=true&output=xml';
        window.open(calendarUrl, '_blank');
    });
    addToCalendarButtonContainer.appendChild(addToCalendarButton);

    // Buy Tickets Dropdown
    const buyTicketsDropdownContainer = eventDetailsModalElement.querySelector('#buy-tickets-btn');
    // console.log(props);
    createBuyTicketsDropdown(buyTicketsDropdownContainer, props);

    // Itinerary Dropdown
    populateItineraryDropdown();

    // View Venue Btn
    buildVenueDetailsModal(eventDetailsModalElement, props);

    // Populate view original post link
    if (!props.eventOriginalLink) {
        eventDetailsModalElement.querySelector('#original-post-link').disabled = true;
    }
    else {
        const viewOriginalPostLink = eventDetailsModalElement.querySelector('#original-post-link');
        viewOriginalPostLink.href = props.eventOriginalLink;
        viewOriginalPostLink.target = '_blank';
    }
}

/**
 * Builds the Venue details modal
 * @function buildVenueDetailsModal
 * @param {HTMLElement} venueDetailsModalElement
 * @param {any} props
 * @returns {void}
 */
export function buildVenueDetailsModal(venueDetailsModalElement, props) {
    console.log(props);
    const viewVenueButtonContainer = venueDetailsModalElement.querySelector('#view-venue-btn');

    viewVenueButtonContainer.innerHTML = ''; // Remove existing button to prevent duplicates

    const viewVenueButton = document.createElement('button');
    viewVenueButton.textContent = 'View Venue';
    viewVenueButton.className = 'btn btn-warning'; // Add Bootstrap classes
    viewVenueButton.setAttribute('aria-label', 'View Venue Details');

    viewVenueButtonContainer.appendChild(viewVenueButton);

    console.log(props);

    document.getElementById('venue-modal-title').textContent = props.venueName || 'No Venue Name Provided';
    document.getElementById('venue-modal-full-address').textContent = props.eventLocation || 'No Venue Address Provided';
    document.getElementById('venue-modal-phone').textContent = props.venuePhoneNumber || 'No Venue Phone Provided';
    if (props.venueWebsite) {
        document.querySelector('#venue-modal-website').style.display = 'block';
        document.querySelector('#venue-modal-website .venue-website-link').href = props.venueWebsite;
    } else {
        document.querySelector('#venue-modal-website').style.display = 'none';
    }
    const rating = Math.floor(props.venueRating);

    // Get the rating container
    const ratingContainer = document.getElementById('venue-modal-rating');

    // Clear the rating container
    ratingContainer.innerHTML = '';

    // Create a new span element for the rating string
    const ratingString = document.createElement('span');

    // Set the text content of the rating string to the venue rating
    ratingString.textContent = `(${props.venueRating})`;

    // Append the rating string to the rating container
    if (rating) {
        ratingContainer.appendChild(ratingString);
    }

    // Loop as many times as the rating
    for (let i = 0; i < rating; i++) {
        // Create a new span element for the star
        const star = document.createElement('span');

        // Set the HTML content of the star to the given HTML string
        star.innerHTML = `<span class="star" data-value="${i + 1}">&#9733;</span>`;

        // Append the star to the rating container
        ratingContainer.appendChild(star);
    }

    viewVenueButton.addEventListener('click', () => {
        const viewVenueModal = document.getElementById('view-venue-modal');
        viewVenueModal.style.display = 'block';

        // Add a function to populate the modal with venue details

        // Add an event listener to the modal that hides the modal when the user clicks outside of it
        viewVenueModal.addEventListener('click', (event) => {
            if (event.target === viewVenueModal) {
                viewVenueModal.style.display = 'none';
            }
        });
    });
}

/**
 * Validates the props for the buildEventDetailsModal function, returns true if the props are valid, false otherwise
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildEventDetailsModalProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        img: x => (typeof x === 'string' || x === undefined || x === null),
        title: x => typeof x === 'string',
        description: x => typeof x === 'string',
        date: x => x instanceof Date,
        fullAddress: x => typeof x === 'string',
        tags: x => Array.isArray(x),
    }

    return validateObject(data, schema).length === 0;
}

/**
 * Creates a buy tickets dropdown button for the event details modal
 * @param {any} buyTicketsDropdownContainer
 * @param {any} props
 * @returns {void}
 */
export function createBuyTicketsDropdown(buyTicketsDropdownContainer, props) {
    const buyTicketsButton = buyTicketsDropdownContainer.querySelector('#buyTicketsBtn');
    const dropdownMenu = buyTicketsDropdownContainer.querySelector('.dropdown-menu');

    // Clear any existing dropdown items
    dropdownMenu.innerHTML = '';
    console.log(props.ticketLinks);
    if (props.ticketLinks.length === 0) {
        buyTicketsButton.classList.add('disabled'); // Disable the button
        buyTicketsDropdownContainer.title = "No ticket links available for this event.";
    } else {
        buyTicketsButton.classList.remove('disabled'); // Enable the button
        buyTicketsDropdownContainer.title = "Click to view ticket options for this event.";
        props.ticketLinks.forEach(ticketLink => {
            const ticketLinkElement = document.createElement('a');
            ticketLinkElement.textContent = ticketLink.source;
            ticketLinkElement.href = ticketLink.link;
            ticketLinkElement.target = '_blank';
            ticketLinkElement.className = 'dropdown-item';

            dropdownMenu.appendChild(ticketLinkElement);
        });
    }
}