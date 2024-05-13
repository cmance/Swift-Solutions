/**
 * Removes an event from the user's bookmark list
 * @param {string} apiEventId 
 * @param {string} bookmarkListTitle
 */
export async function removeEventFromFavorites(apiEventId, bookmarkListTitle) {
    let url = "/api/FavoritesApi/RemoveFavorite";
    let res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            apiEventId: apiEventId,
            bookmarkListTitle: bookmarkListTitle,
        })
    })

    if (!res.ok) {
        throw new Error(res.status)
    }
}
