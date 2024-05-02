
/**
 * Shows and the delete favorite event confirmation modal
 * @param {Function} onDelete
 */
export function showDeleteFavoriteEventConfirmationModal(onDelete) {
    const modal = document.getElementById('delete-favorite-event-confirmation-modal');
    if (!modal) {
        console.error('Modal element #delete-favorite-event-confirmation-modal not found.');
        return;
    }
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    const closeButton = document.getElementById("cancel-delete-favorite-event-confirmation-button");
    closeButton.onclick = function () { // Close the modal if the user clicks on the close button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
    }

    const deleteButton = document.getElementById("delete-favorite-event-confirmation-button");
    deleteButton.onclick = function () { // Close the modal if the user clicks on the delete button
        modal.style.display = 'none';
        document.body.style.overflow = 'auto'; // Restore scrolling
        onDelete();
    }

    window.onclick = function (event) { // Close the modal if the user clicks outside of it
        if (event.target == modal) {
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    }
}
