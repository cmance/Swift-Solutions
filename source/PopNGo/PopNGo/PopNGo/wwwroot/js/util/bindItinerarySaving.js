import { createNewItinerary, addEventToItinerary, getAllUserEventsFromItinerary } from '../api/itinerary/itineraryApi.js';

async function bindItinerarySaving() {
    const saveButton = document.getElementById('save-new-itinerary');

    saveButton.addEventListener('click', async function () {
        const titleInput = document.getElementById('itinerary-title');
        const itineraryTitle = titleInput.value.trim();

        if (itineraryTitle) {
            // Assume createNewItinerary is an async function and waits for API call to complete
            await createNewItinerary(itineraryTitle);
            
            // Close the modal
            const modalElement = document.getElementById('new-itinerary-modal');
            titleInput.value = ''; // Clear the input field
            const bootstrapModal = bootstrap.Modal.getInstance(modalElement);
            bootstrapModal.hide();

            // Refresh the dropdown to include the new itinerary
            await populateItineraryDropdown();
        } else {
            alert('Please enter a title for the itinerary.');
        }
    });
};

async function populateItineraryDropdown() {
    try {
        const apiEventID = document.getElementById('event-id').value;

        const itineraries = await getAllUserEventsFromItinerary();
        const dropdownMenu = document.getElementById('itinerary-list');

        // Remove previous items except for the first item, which might be a default or title
        while (dropdownMenu.children.length > 1) {
            dropdownMenu.removeChild(dropdownMenu.lastChild);
        }

        // Check if there are itineraries before attempting to populate the dropdown
        if (itineraries.length === 0) {
            console.log('No itineraries available.');
            const item = document.createElement('li');
            const link = document.createElement('a');
            link.className = 'dropdown-item';
            link.textContent = 'No itineraries available';
            link.href = "#";
            item.appendChild(link);
            dropdownMenu.appendChild(item);
        } else {
            // Populate the dropdown with itineraries
            itineraries.forEach(itinerary => {
                const item = document.createElement('li');
                const link = document.createElement('a');
                link.className = 'dropdown-item';
                link.textContent = itinerary.itineraryTitle;
                link.href = "#";
                link.dataset.itineraryId = itinerary.id;  // Assuming each itinerary has an 'id' property
                link.addEventListener('click', function () {
                    addEventToItinerary(this.dataset.itineraryId, apiEventID);
                });
                item.appendChild(link);
                dropdownMenu.appendChild(item);
            });
        }
    } catch (error) {
        console.error('Failed to fetch itineraries:', error);
    }
}

export { bindItinerarySaving, populateItineraryDropdown };