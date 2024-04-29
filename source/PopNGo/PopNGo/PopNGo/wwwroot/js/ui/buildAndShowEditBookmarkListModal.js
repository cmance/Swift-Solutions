
/**
 * Shows and builds the edit bookmark list modal
 * @param {String} listName 
 * @param {Function} onClickSave - (listName: String) => {} Function to call when the user saves
 * @param {Array[String]} bookmarkListNames - List of all bookmark list names to check for duplicates
 */
export function buildAndShowEditBookmarkListModal(listName, onClickSave, bookmarkListNames) {
    const modal = document.getElementById('edit-bookmark-list-modal');
    if (!modal) {
        console.error('Modal element #edit-bookmark-list-modal not found.');
        return;
    }
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    // Set the bookmark list name
    modal.querySelector('#edit-bookmark-list-modal-title').textContent = "Enter a new name for \"" + listName + "\"";


    const closeButton = document.getElementById("cancel-edit-bookmark-list-button");
    closeButton.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
    }
    
    // Set the input field value
    const input = document.getElementById('edit-bookmark-list-modal-title-input');
    input.value = listName;

    const saveButton = document.getElementById("save-edit-bookmark-list-button");
    saveButton.disabled = true; // Disable the save button by default
    // Add event listener to the input field to check for duplicates or empty input
    input.oninput = function () {
        if (bookmarkListNames.includes(input.value) || input.value === '') {
            saveButton.disabled = true;
        } else {
            saveButton.disabled = false;
        }
    }

    saveButton.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
        onClickSave(input.value);
    }

    window.onclick = function (event) { // Close the modal if the user clicks outside of it
        if (event.target == modal) {
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    }
}
