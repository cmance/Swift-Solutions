// Props object example:
// {
//     img: String link,
//     title: String,
//     date: DateTime,
//     city: String,
//     state: String,
//     favorited: Boolean
//     tags: Array[String],
// }

/**
 * Takes in the event card element and the event info props and updates the event card element
 * Props:
{
     img: String link,
     title: String,
     date: DateTime,
     city: String,
     state: String,
     tags: Array[Tag],
     favorited: Boolean
     onPressFavorite: Function
 }

 Tag: {
    tagName: String,
    tagTextColor: String,
    tagBackgroundColor: String,
 }

 * @function buildEventCard
 * @param {HTMLElement} eventCardElement 
 * @param {Object} props 
 */

export const buildEventCard = (eventCardElement, props) => {
    console.log(props)
    // Set the image
    if (props.img === null || props.img === undefined) {
        eventCardElement.querySelector('#event-card-image').src = '/media/images/placeholder_event_card_image.png';
    } else {
        eventCardElement.querySelector('#event-card-image').src = props.img;
    }

    // Set the title
    eventCardElement.querySelector('#event-card-title').textContent = props.title;

    // Set the date
    eventCardElement.querySelector('#day-number').textContent = props.date.getDate();
    eventCardElement.querySelector('#month').textContent = props.date.toLocaleString('default', { month: 'short' });

    // Set the location
    eventCardElement.querySelector('#event-card-location').textContent = `${props.city}, ${props.state}`;

    // Set the favorite status
    const bookmarkContainer = eventCardElement.querySelector('#event-card-bookmark-container');
    const bookmarkImage = eventCardElement.querySelector('#event-card-bookmark-icon');
    bookmarkImage.src = props.favorited ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';

    bookmarkContainer.addEventListener('click', () => {
        // Update the favorite status and the image source
        props.onPressFavorite();
        props.favorited = !props.favorited;
        bookmarkImage.src = props.favorited ? '/media/images/heart-filled.svg' : '/media/images/heart-outline.svg';
    });

    // Set the tags:
    const tagsElement = eventCardElement.querySelector('#event-card-tags-container');
    tagsElement.innerHTML = '';
    props.tags.forEach(tag => {
        const tagEl = document.createElement('span');
        tagEl.classList.add('event-tag');
        tagEl.classList.add('rounded-pill');
        tagEl.textContent = tag.tagName;
        tagEl.style.color = tag.tagTextColor;
        tagEl.style.backgroundColor = tag.tagBackgroundColor;
        tagsElement.appendChild(tagEl);
    });
}
