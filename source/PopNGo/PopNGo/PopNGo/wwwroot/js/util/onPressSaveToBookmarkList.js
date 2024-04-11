import { addEventToFavorites } from '../api/favorites/addEventToFavorites.js';
import { showLoginSignupModal } from './showUnauthorizedLoginModal.js';
import { showToast } from './toast.js';

/**
 * Takes in an eventApiBody and a bookmark list name, then updates the favorite status of the event via http
 * 
 * eventApiBody: {
        ApiEventID: eventInfo.eventID || "No ID available",
        EventDate: eventInfo.eventStartTime || "No date available",
        EventName: eventInfo.eventName || "No name available",
        EventDescription: eventInfo.eventDescription || "No description available",
        EventLocation: eventInfo.full_Address || "No location available",
        EventImage: eventInfo.eventImage,
    };
 * 
 * @async
 * @function onPressSaveToBookmarkList
 * @param {object} eventApiBody
 * @param {string} bookmarkListName
 * @returns {Promise<void>}
 */
export async function onPressSaveToBookmarkList(eventInfo, bookmarkListName) {
    addEventToFavorites(bookmarkListName, eventInfo).catch((error) => {
        // TODO: check that it is an unauthorized error
        // Unauthorized, show the login/signup modal
        showLoginSignupModal();
    })
    showToast('Event saved to ' + bookmarkListName + '!');
}
