import { addEventToFavorites } from './addEventToFavorites.js';
import { removeEventFromFavorites } from './removeEventFromFavorites.js';
import { getEventIsFavorited } from './getEventIsFavorited.js';
import { showLoginSignupModal } from '../../util/showUnauthorizedLoginModal.js';

export async function onClickFavorite(favoriteIcon, eventInfo) {
    let favorited = await getEventIsFavorited(eventInfo.ApiEventID);

    // If the event is already favorited, unfavorite it
    if (favorited) {
        removeEventFromFavorites(eventInfo).then(() => {
            favoriteIcon.src = '/media/images/heart-outline.svg';
        })
    } else {
        addEventToFavorites(eventInfo).then(() => {
            favoriteIcon.src = '/media/images/heart-filled.svg';
        }).catch((error) => {
            // TODO: Should only happen if the error is 401
            showLoginSignupModal();
        });
    }
}