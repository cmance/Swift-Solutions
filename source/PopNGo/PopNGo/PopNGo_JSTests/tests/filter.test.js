import {
    validateFilterValues, validateAndReturnDate, checkForValidDateRange,
    sortByAlphabeticalAscDesc, sortByDateAscDesc, sortByDateRange,
    sortByVenueRatingAscDesc
} from '../../PopNGo/wwwroot/js/util/filter.js';

beforeEach(() => { // Suppress console.error calls
    jest.spyOn(console, 'error').mockImplementation(() => { });
});

describe('validateFilterValues', () => {
    it('should return true for valid filter values', () => {
        const validFilterValues = {
            'start-date': '2022-01-01',
            'end-date': '2022-12-31',
            'date-asc-desc': 'asc',
            'venue-rating-asc-desc': 'asc',
            'event-alphabetical-asc-desc': 'asc',
        };
        expect(validateFilterValues(validFilterValues)).toBe(true);
    });

    it('should return false for invalid date range', () => {
        const invalidFilterValues = {
            'start-date': 'invalid-date',
            'end-date': '2022-12-31',
            'date-asc-desc': 'asc',
            'venue-rating-asc-desc': 'asc',
            'event-alphabetical-asc-desc': 'asc',
        };
        expect(validateFilterValues(invalidFilterValues)).toBe(false);
    });

    it('should return false for invalid date sort order', () => {
        const invalidFilterValues = {
            'start-date': '2022-01-01',
            'end-date': '2022-12-31',
            'date-asc-desc': 'invalid-order',
            'venue-rating-asc-desc': 'asc',
            'event-alphabetical-asc-desc': 'asc',
        };
        expect(validateFilterValues(invalidFilterValues)).toBe(false);
    });

    it('should return false for invalid venue rating sort order', () => {
        const invalidFilterValues = {
            'start-date': '2022-01-01',
            'end-date': '2022-12-31',
            'date-asc-desc': 'asc',
            'venue-rating-asc-desc': 'invalid-order',
            'event-alphabetical-asc-desc': 'asc',
        };
        expect(validateFilterValues(invalidFilterValues)).toBe(false);
    });

    it('should return false for invalid event alphabetical sort order', () => {
        const invalidFilterValues = {
            'start-date': '2022-01-01',
            'end-date': '2022-12-31',
            'date-asc-desc': 'asc',
            'venue-rating-asc-desc': 'asc',
            'event-alphabetical-asc-desc': 'invalid-order',
        };
        expect(validateFilterValues(invalidFilterValues)).toBe(false);
    });
});

describe('validateAndReturnDate', () => {
    it('should return the date for valid date', () => {
        const validDate = '2022-01-01';
        expect(validateAndReturnDate(validDate)).toBe(validDate);
    });

    it('should return false for invalid date', () => {
        const invalidDate = 'invalid-date';
        expect(validateAndReturnDate(invalidDate)).toBe(false);
    });

    it('should return undefined for undefined date', () => {
        const undefinedDate = undefined;
        expect(validateAndReturnDate(undefinedDate)).toBe(undefined);
    });
});

describe('checkForValidDateRange', () => {
    it('should return true for valid date range', () => {
        const startDate = '2022-01-01';
        const endDate = '2022-12-31';
        expect(checkForValidDateRange(startDate, endDate)).toBe(true);
    });

    it('should return false for invalid date range', () => {
        const startDate = '2022-12-31';
        const endDate = '2022-01-01';
        expect(checkForValidDateRange(startDate, endDate)).toBe(false);
    });

    it('should return true for undefined dates', () => {
        const startDate = undefined;
        const endDate = undefined;
        expect(checkForValidDateRange(startDate, endDate)).toBe(true);
    });
});

describe('sortByAlphabeticalAscDesc', () => {
    const events = [
        { eventName: 'Event B' },
        { eventName: 'Event A' },
        { eventName: 'Event C' },
    ];

    it('should sort events in ascending order', () => {
        const sortedEventsAsc = sortByAlphabeticalAscDesc([...events], 'asc');
        expect(sortedEventsAsc).toEqual([
            { eventName: 'Event A' },
            { eventName: 'Event B' },
            { eventName: 'Event C' },
        ]);
    });

    it('should sort events in descending order', () => {
        const sortedEventsDesc = sortByAlphabeticalAscDesc([...events], 'desc');
        expect(sortedEventsDesc).toEqual([
            { eventName: 'Event C' },
            { eventName: 'Event B' },
            { eventName: 'Event A' },
        ]);
    });
});

describe('sortByDateAscDesc', () => {
    const events = [
        { eventDate: '2022-02-01' },
        { eventDate: '2022-01-01' },
        { eventDate: '2022-03-01' },
    ];

    it('should sort events in ascending order by date', () => {
        const sortedEventsAsc = sortByDateAscDesc([...events], 'asc');
        expect(sortedEventsAsc).toEqual([
            { eventDate: '2022-01-01' },
            { eventDate: '2022-02-01' },
            { eventDate: '2022-03-01' },
        ]);
    });

    it('should sort events in descending order by date', () => {
        const sortedEventsDesc = sortByDateAscDesc([...events], 'desc');
        expect(sortedEventsDesc).toEqual([
            { eventDate: '2022-03-01' },
            { eventDate: '2022-02-01' },
            { eventDate: '2022-01-01' },
        ]);
    });
});

describe('sortByDateRange', () => {
    const events = [
        { eventDate: '2022-02-01' },
        { eventDate: '2022-01-01' },
        { eventDate: '2022-03-01' },
    ];

    it('should filter events within the date range', () => {
        const startDate = '2022-01-15';
        const endDate = '2022-02-15';
        const filteredEvents = sortByDateRange([...events], startDate, endDate);
        expect(filteredEvents).toEqual([
            { eventDate: '2022-02-01' },
        ]);
    });

    it('should return all events if the date range includes all events', () => {
        const startDate = '2022-01-01';
        const endDate = '2022-03-01';
        const filteredEvents = sortByDateRange([...events], startDate, endDate);
        expect(filteredEvents).toEqual(events);
    });

    it('should return no events if the date range does not include any events', () => {
        const startDate = '2021-01-01';
        const endDate = '2021-12-31';
        const filteredEvents = sortByDateRange([...events], startDate, endDate);
        expect(filteredEvents).toEqual([]);
    });
});

describe('sortByVenueRatingAscDesc', () => {
    const events = [
        { venueRating: 3 },
        { venueRating: 1 },
        { venueRating: 2 },
    ];

    it('should sort events in ascending order by venue rating', () => {
        const sortedEventsAsc = sortByVenueRatingAscDesc([...events], 'asc');
        expect(sortedEventsAsc).toEqual([
            { venueRating: 1 },
            { venueRating: 2 },
            { venueRating: 3 },
        ]);
    });

    it('should sort events in descending order by venue rating', () => {
        const sortedEventsDesc = sortByVenueRatingAscDesc([...events], 'desc');
        expect(sortedEventsDesc).toEqual([
            { venueRating: 3 },
            { venueRating: 2 },
            { venueRating: 1 },
        ]);
    });
});


afterEach(() => { // Restore console.error calls
    console.error.mockRestore();
});