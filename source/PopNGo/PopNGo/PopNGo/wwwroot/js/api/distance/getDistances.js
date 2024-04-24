/**
 * 
 * @param {number} lat 
 * @param {number} long 
 * @param {*[]} eventDetails 
 * @returns 
 */
export async function getDistancesForEvents(lat, long, eventDetails, unit) {
    if(eventDetails.length === 0) {
        return [];
    }

    const events = eventDetails.map(event => `(${event.latitude},${event.longitude})`);

    try {
        const response = await fetch(`/api/distances/calculate/startingLat=${lat}&startingLong=${long}&events=${events}&unit=${unit}`);
        if (!response.ok) {
            if(response.status === 400) {
                return [];
            }
            throw new Error(`Network response was not ok: ${response.status}`);
        }

        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        console.error(`There was a problem with the fetching distances to events: ${error}`);
        return [];
    }
}

export async function getDistanceUnit() {
    try {
        const response = await fetch('/api/distances/unit');
        if (!response.ok) {
            if(response.status === 401) {
                return 'miles';
            }
            
            throw new Error(`Network response was not ok: ${response.status}`);
        }

        const data = await response.text();
        return data;
    } catch (error) {
        console.error(`There was a problem with the fetching distance unit: ${error}`);
        return 'miles';
    }
}

export function convertDistance(distance, targetUnit, originalUnit) {
    if (originalUnit === targetUnit) {
        return distance;
    }

    let newDistance = null;
    if (originalUnit === 'miles') {
        newDistance = targetUnit === 'kilometers' ? distance * 1.60934 : distance;
    } else {
        newDistance = targetUnit === 'miles' ? distance * 0.621371 : distance;
    }

    return newDistance.toFixed(2);
}