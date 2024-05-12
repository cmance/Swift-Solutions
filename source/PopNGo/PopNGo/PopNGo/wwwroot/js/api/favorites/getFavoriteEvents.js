/* Data return example:
[
    { 
        eventDescription: "DEERHOOF \\*MIRACLE-LEVEL TOUR\\*\n\nAFTER 28 YEARS, DEERHOOF RECORDS THEIR STUDIO DEBUT AND IT�S ALL IN JAPANESE"
        eventEndTime: "2024-02-16T03:45:00"
        eventIsVirtual: true
        eventLanguage: "en"
        eventLink: null
        eventName: "Deerhoof"
        eventStartTime: "2024-02-15T21:45:00"
        eventThumbnail: "https://dice-media.imgix.net/attachments/2023-06-05/1ae87a1a-92dd-45e5-bd62-afe41ffad83a.jpg?rect=0%2C0%2C3000%2C3000"
        full_Address: null
        latitude: 41.903908
        longitude: 12.538744
        phone_Number: "+393515211938"
    }, 
    {...}, ...
]
*/

/**
 * Fetches favorited events data from bookmark list
 * 
 * @async
 * @function getEvents
 * @param {string} bookmarkListName
 * @returns {Promise<Object[]>}
 */
export async function getFavoriteEvents(bookmarkListName) {
    try {
        const response = await fetch(`/api/FavoritesApi/Favorites?bookmarkListTitle=${bookmarkListName}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

