/**
 * Takes in an api event id and returns a boolean indicating if the event is favorited
 * @param {String} apiEventId
 * @returns {boolean}
 */
export async function getEventIsFavorited(apiEventId) {
    let res = await fetch(`/api/FavoritesApi/IsFavorite?eventId=${apiEventId}`)
    return await res.json();
}