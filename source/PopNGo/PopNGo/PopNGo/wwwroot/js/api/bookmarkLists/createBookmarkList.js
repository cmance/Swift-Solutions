/**
 * Create a new bookmark list
 * @async
 * @function createBookmarkList
 * @param {string} bookmarkListName
 * @returns {Promise<void>}
 */
export async function createBookmarkList(bookmarkListName) {
    let url = "/api/BookmarkListApi/BookmarkList";
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Title: bookmarkListName,
        })
    })

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }

        throw new Error('Network response was not ok');
    }
}
