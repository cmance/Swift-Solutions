import { UnauthorizedError } from '../../util/errors.js';

/**
 * Takes in an api event id and returns a boolean indicating if the event is favorited
 * 
 * BookmarkList example:
 * 
        id = Int
        title = String
        favoriteEventQuantity = int
        userId = int
 * }
 * 
 * @async
 * @function getBookmarkLists
 * @returns {Promise<BookmarkList[]>}
 */
export async function getBookmarkLists() {
    const res = await fetch(`/api/BookmarkListApi/BookmarkLists`);
    if (res.status === 401) {
        throw new UnauthorizedError('Unauthorized');
    }
    if (!res.ok) {
        throw new Error('Network response was not ok');
    }
    return await res.json();
}
