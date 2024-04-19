import { addEventToFavorites } from '../api/favorites/addEventToFavorites.js';
import { showLoginSignupModal } from './showUnauthorizedLoginModal.js';
import { showToast } from './toast.js';

/**
 * Takes in an eventApiBody and a bookmark list name, then updates the favorite status of the event via http
 * 
 * @async
 * @function onPressSaveToBookmarkList
 * @param {string} eventApiId
 * @param {string} bookmarkListName
 * @returns {Promise<void>}
 */
export async function onPressSaveToBookmarkList(apiEventId, bookmarkListName) {
    addEventToFavorites(bookmarkListName, apiEventId).catch((error) => {
        // TODO: check that it is an unauthorized error
        // Unauthorized, show the login/signup modal
        showLoginSignupModal();
    })
    showToast('Event saved to ' + bookmarkListName + '!');
}
