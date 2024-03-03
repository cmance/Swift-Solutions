// Adds event to user's favorites

/*  Event:
    public int id
    public string apiEventID
    public DateTime eventDate
    public string eventName
    public string eventDescription
    public string eventLocation
}*/

export async function addEventToFavorites(event) {
    let url = "/api/FavoritesApi/AddFavorite";
    await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })
}
