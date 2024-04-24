/**
 * 
 * @param {number} lat 
 * @param {number} long 
 * @returns 
 */
export async function getForecastForLocation(lat, long) {
    try {
        const response = await fetch(`/api/weather/forecast/lat=${lat}&long=${long}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        console.error('There was a problem with the fetching distances to events:', error);
        return {};
    }
}