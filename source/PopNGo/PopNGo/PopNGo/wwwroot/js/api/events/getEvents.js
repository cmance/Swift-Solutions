/**
 * Purpose: Fetches event data from the server based on query string.
 * 
 * Query string is the search term that the user enters in the search bar,
 * just like searching on Google. Example: "Sports events in Monmouth, Oregon"
 * 
 * Start is the index of the first event to return. This is used for pagination.
 * @async
 * @function getEvents
 * @param {string} query
 * @param {number} start - The index of the first event to return
 * @returns {Object[]}
 */
export async function getEvents(query, start) {
    try {
        const response = await fetch(`/api/search/events?q=${query}&start=${start}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();

        console.log(data);
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

