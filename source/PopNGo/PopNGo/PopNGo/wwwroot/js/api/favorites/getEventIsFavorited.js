export async function getEventIsFavorited(apiEventId) {
    let res = await fetch(`/api/FavoritesApi/IsFavorite?eventId=${apiEventId}`)
    return await res.json();
}