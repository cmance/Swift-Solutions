export async function getPlaceSuggestions(query, latitude, longitude) {
    let url = new URL(`/api/search/places`, window.location.origin);
    url.searchParams.append('q', query);
    url.searchParams.append('coordinates', `@${latitude},${longitude},14z`);

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Error ${response.status}: ${errorText}`);
    }

    return await response.json();
}
