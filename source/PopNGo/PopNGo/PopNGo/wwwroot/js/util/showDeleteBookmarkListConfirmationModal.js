
/**
 * Shows and builds the delete bookmark list confirmation modal
 * @param {String} listName 
 * @param {Function} onDelete - (listName: String) => {} Function to call when the user confirms the deletion
 */
export function showDeleteBookmarkListConfirmationModal(listName, onDelete) {
    const modal = document.getElementById('delete-bookmark-list-confirmation-modal');
    if (!modal) {
        console.error('Modal element #delete-bookmark-list-confirmation-modal not found.');
        return;
    }
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    // Set the bookmark list name
    modal.querySelector('#delete-bookmark-list-confirmation-modal-title').textContent = "Are you sure you want to remove \"" + listName + "\"?";


    const closeButton = document.getElementById("cancel-delete-bookmark-list-confirmation-button");
    closeButton.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
    }

    const deleteButton = document.getElementById("delete-bookmark-list-confirmation-button");
    deleteButton.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
        onDelete(listName);
    }

    window.onclick = function (event) { // Close the modal if the user clicks outside of it
        if (event.target == modal) {
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    }
}
