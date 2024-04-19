

/**
 * Helper function
 * Purpose: Get the filter values.
 * @returns {Object}
 */
function getFilterValues() {
    /* 
    Example of filterValues object
    filterValues = {
        'start-date': '2021-01-01',
        'end-date': '2021-12-31',
        'date-asc-desc': 'asc',
        'venue-rating-asc-desc': 'asc',
        'event-alphabetical-asc-desc': 'asc'
    }
    */

    let startDate = document.getElementById('start-date');
    let endDate = document.getElementById('end-date');
    let sortDate = document.getElementById('sort-date');
    let sortRating = document.getElementById('sort-rating');
    let sortAlphabetical = document.getElementById('sort-alphabetical');

    let filterValues = {};

    if(startDate && endDate) {
        filterValues['start-date'] = startDate.value || '';
        filterValues['end-date'] = endDate.value || '';
    }

    if(sortDate) {
        filterValues['date-asc-desc'] = sortDate ? sortDate.value : '';
    }

    if(sortRating) {
        filterValues['venue-rating-asc-desc'] = sortRating ? sortRating.value : '';
    }

    if(sortAlphabetical) {
        filterValues['event-alphabetical-asc-desc'] = sortAlphabetical ? sortAlphabetical.value : '';
    }

    validateFilterValues(filterValues);
    
    return filterValues;
}

/**
 * Helper function
 * Purpose: Validate the filter values.
 * @param {Object} filterValues
 * @returns {boolean}
 */
function validateFilterValues(filterValues) {
    // Check if the date range is valid
    let startDate = validateAndReturnDate(filterValues['start-date']);
    let endDate = validateAndReturnDate(filterValues['end-date']);
    if(startDate === false || endDate === false) {
        console.error('Invalid date range. Please select a valid date range.');
        return false;
    }

    let dateAscDesc = filterValues['date-asc-desc'];
    if (dateAscDesc !== null && dateAscDesc !== undefined && dateAscDesc !== '' && dateAscDesc !== 'asc' && dateAscDesc !== 'desc') {
        console.error('Invalid date sort order. Please select a valid date sort order.');
        return false;
    }
    
    let venueRatingAscDesc = filterValues['venue-rating-asc-desc'];
    if (venueRatingAscDesc !== null && venueRatingAscDesc !== undefined && venueRatingAscDesc !== '' && venueRatingAscDesc !== 'asc' && venueRatingAscDesc !== 'desc') {
        console.error('Invalid venue rating sort order. Please select a valid venue rating sort order.');
        return false;
    }
    
    let eventAlphabeticalAscDesc = filterValues['event-alphabetical-asc-desc'];
    if (eventAlphabeticalAscDesc !== null && eventAlphabeticalAscDesc !== undefined && eventAlphabeticalAscDesc !== '' && eventAlphabeticalAscDesc !== 'asc' && eventAlphabeticalAscDesc !== 'desc') {
        console.error('Invalid event name sort order. Please select a valid event name sort order.');
        return false;
    }

    return true;
}

/**
 * Helper function
 * Purpose: Validate the date range.
 * @param {string} dateRange
 * @returns {boolean|string}
 */
function validateAndReturnDate(date) {
    if(date) {
        let dateObj = new Date(date);
        if(dateObj instanceof Date && !isNaN(dateObj)) {
            return date;
        } else {
            console.error('Invalid date. Please select a valid date.');
            return false;
        }
    
    }
    return date;
}

function checkForValidDateRange(startDate, endDate) {
    if(startDate && endDate) {
        let startDateObj = new Date(startDate);
        let endDateObj = new Date(endDate);
        if(startDateObj > endDateObj) {
            return false;
        }
    }
    return true;
}

/**
 * Purpose: Combine the getFilterValues and validateFilterValues functions into one function that returns the filter values.
 * @returns {Object}
 */
function getAndValidateFilterValues() {
    let filterValues = getFilterValues();

    let startDate = validateAndReturnDate(filterValues['start-date']);
    let endDate = validateAndReturnDate(filterValues['end-date']);
    if(startDate === false || endDate === false) {
        console.error('Invalid date range. Please select a valid date range.');
        return false;
    }

    // Check if start date is not after end date
    if(!checkForValidDateRange(startDate, endDate)) {
        console.error('Invalid date range. Start date cannot be after end date.');
        return false;
    }

    validateFilterValues(filterValues);
    console.log(filterValues);
    return filterValues;
}

function sortByAlphabeticalAscDesc(events, ascDesc) {
    if(ascDesc === 'asc') {
        events.sort((a, b) => a.eventName.localeCompare(b.eventName));
    } else {
        events.sort((a, b) => b.eventName.localeCompare(a.eventName));
    }
    return events;
}

function sortByDateAscDesc(events, ascDesc) {
    if(ascDesc === 'asc') {
        events.sort((a, b) => new Date(a.eventDate) - new Date(b.eventDate));
    } else {
        events.sort((a, b) => new Date(b.eventDate) - new Date(a.eventDate));
    }
    return events;
}

function sortByDateRange(events, startDate, endDate) {
    return events.filter(event => {
        let eventDate = new Date(event.eventDate);
        return eventDate >= new Date(startDate) && eventDate <= new Date(endDate);
    });
}

function sortByVenueRatingAscDesc(events, ascDesc) {
    if(ascDesc === 'asc') {
        events.sort((a, b) => a.venueRating - b.venueRating);
    } else {
        events.sort((a, b) => b.venueRating - a.venueRating);
    }
    return events;
}

function returnSortedEvents(events, filterValues) {
    let sortedEvents = events;

    if(filterValues['event-alphabetical-asc-desc']) {
        sortedEvents = sortByAlphabeticalAscDesc(sortedEvents, filterValues['event-alphabetical-asc-desc']);
    }

    if(filterValues['date-asc-desc']) {
        sortedEvents = sortByDateAscDesc(sortedEvents, filterValues['date-asc-desc']);
    }

    if(filterValues['start-date'] && filterValues['end-date']) {
        sortedEvents = sortByDateRange(sortedEvents, filterValues['start-date'], filterValues['end-date']);
    }

    if(filterValues['venue-rating-asc-desc']) {
        sortedEvents = sortByVenueRatingAscDesc(sortedEvents, filterValues['venue-rating-asc-desc']);
    }

    return sortedEvents;
}

/**
 * Purpose: Apply filters and sort events.
 * @param {Array} events
 * @returns {Array}
 */
export function applyFiltersAndSortEvents(events) {
    let filterValues = getAndValidateFilterValues();

    // If getAndValidateFilterValues returned false, stop execution
    if(filterValues === false) {
        return;
    }

    let sortedEvents = returnSortedEvents(events, filterValues);
    
    console.log(sortedEvents);
    return sortedEvents;
}