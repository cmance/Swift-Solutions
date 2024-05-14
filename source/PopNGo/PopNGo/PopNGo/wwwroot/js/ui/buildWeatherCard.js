import { validateObject } from "../validation.js";
import { formatDate } from "../util/formatStartTime.js";
// Props object example:
// {
//     img: String link,
//     title: String,
//     date: DateTime,
//     city: String,
//     state: String,
//     tags: Array[String],
// }


/**
 * Takes in the weather card element and the weather info props and updates the weather card element
 * Props:
{
     img: String link,
     title: String,
     date: DateTime,
     city: String,
     state: String,
     tags: Array[Tag],
     distance: Number,
     distanceUnit: String,
     bookmarkListNames: Array[String]
     onPressBookmarkList: (bookmarkListName: String) => void (optional),
     onPressWeather: Function
 }

 Tag: {
    tagName: String,
    tagTextColor: String,
    tagBackgroundColor: String,
 }

 * @function buildWeatherCard
 * @param {HTMLElement} weatherCardElement 
 * @param {Object} props 
 */

export const buildWeatherCard = (weatherCardElement, props) => {
    const dateElemnt = weatherCardElement.querySelector('#weather-date');
    const conditionElement = weatherCardElement.querySelector('#condition');
    const temperatureUnitElement = weatherCardElement.querySelector('#temperature-unit');
    const minTempElement = weatherCardElement.querySelector('#temperature-min');
    const maxTempElement = weatherCardElement.querySelector('#temperature-max');
    const humidityElement = weatherCardElement.querySelector('#humidity');
    const cloudCoverElement = weatherCardElement.querySelector('#cloud-cover');
    const precipitationElement = weatherCardElement.querySelector('#precipitation');

    // Set the date
    dateElemnt.textContent = formatDate(props.date);

    // Set the condition
    conditionElement.textContent = props.condition;

    // Set the temperature data
    temperatureUnitElement.textContent = props.temperatureUnit.toUpperCase();
    minTempElement.textContent = `${props.minTemp}`;
    maxTempElement.textContent = `${props.maxTemp}`;

    // Set the humidity
    humidityElement.textContent = props.humidity;

    // Set the cloud cover
    cloudCoverElement.textContent = props.cloudCover;

    // Set the precipitation
    if (props.precipitationType === null) {
        precipitationElement.textContent = 'None';
    } else {
        if(props.precipitationType === "" || props.precipitationType === null){
            precipitationElement.textContent = `${props.precipitationAmount} ${props.measurementUnit}, ${props.precipitationChance}%`;
        } else {
            precipitationElement.textContent = `${props.precipitationType}, ${props.precipitationAmount} ${props.measurementUnit}, ${props.precipitationChance}%`;
        }
    }
}


/**
 * Takes in weather card props and returns a boolean indicating if the props are valid
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildWeatherCardProps(data) {
    if (data === undefined || data === null) {
        return false;
    }
    const schema = {
        date: x => x instanceof Date,
        condition: x => typeof x === 'string',
        minTemp: x => typeof x === 'number',
        maxTemp: x => typeof x === 'number',
        humidity: x => typeof x === 'number',
        cloudCover: x => typeof x === 'number',
        precipitationType: x => typeof x === 'string' || x === null,
        precipitationAmount: x => typeof x === 'string',
        precipitationChance: x => typeof x === 'number',
        temperatureUnit: x => typeof x === 'string',
        measurementUnit: x => typeof x === 'string',
    }

    // console.log(validateObject(data, schema));
    return validateObject(data, schema).length === 0;
}
