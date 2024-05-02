﻿export async function getAllUserEventsFromItinerary() {
    let url = `/api/ItineraryApi`;
    const res = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    });

    if (!res.ok) {
        // If the response is unauthorized or not OK, throw an error.
        const errorText = await res.text();
        throw new Error(`Error ${res.status}: ${errorText}`);
    }

    // Parse the JSON response and return.
    return await res.json();
}

export async function addEventToItinerary(itineraryId, apiEventId) {
    const url = `/api/ItineraryEventApi/ItineraryEvent/${apiEventId}/${itineraryId}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (!response.ok) {
            const errorText = await response.text();
            console.error(`Error ${response.status}: ${errorText}`);
            alert(`Failed to add event to itinerary: ${errorText}`);
            return;
        }
        alert('Event successfully added to the itinerary!');
    } catch (error) {
        console.error('Error adding event to itinerary:', error);
        alert('Error adding event to itinerary. Please try again.');
    }
}

export async function createNewItinerary(itineraryTitle) {
    console.log('Creating new itinerary with title:', itineraryTitle);
    let url = `/api/ItineraryApi/Itinerary?itineraryTitle=${itineraryTitle}`;
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        const errorText = await response.text();
        console.error(`Error ${response.status}: ${errorText}`);
        alert(`Failed to create new itinerary: ${errorText}`);
        throw new Error(`Error ${response.status}: ${errorText}`);
    }

    // Check if the response body is not empty before attempting to parse it as JSON
    const text = await response.text();
    let result;
    try {
        result = text ? JSON.parse(text) : {}; // Default to an empty object if there is no response text
    } catch (error) {
        console.error('Failed to parse response as JSON:', error);
        alert('Failed to process the response from the server.');
        throw new Error('Failed to process the response from the server.');
    }

    console.log('Itinerary created:', result);
    alert('New itinerary successfully created!');
    return result;
}