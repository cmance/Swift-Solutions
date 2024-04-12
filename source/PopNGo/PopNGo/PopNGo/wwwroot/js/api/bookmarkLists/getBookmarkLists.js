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
    try {
        const res = await fetch(`/api/BookmarkListApi/BookmarkLists`);
        return await res.json();
    } catch (error) {
        throw new Error('Network response was not ok');
    }
}
