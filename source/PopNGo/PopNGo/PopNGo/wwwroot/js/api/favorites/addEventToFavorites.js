// Adds event to user's favorites

export async function addEventToFavorites(bookmarkListName, apiEventId) {
    let url = "/api/FavoritesApi/AddFavorite";
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            apiEventId: apiEventId,
            bookmarkListTitle: bookmarkListName,
        })
    })

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }

        throw new Error('Network response was not ok');
    }
}
