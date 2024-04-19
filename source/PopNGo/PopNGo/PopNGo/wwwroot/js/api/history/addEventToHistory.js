/**
 * Adds event to user's history
 * @param {string} apiEventId 
 */
export async function addEventToHistory(apiEventId) {
    let url = `/api/EventHistoryApi/EventHistory?apiEventId=${apiEventId}`;
    const res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }

        throw new Error('Network response was not ok');
    }
}
