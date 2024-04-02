export function showToast(message) {
    // Create a toast
    const toast = document.createElement('div');
    toast.classList.add('toast');
    toast.textContent = message;

    // Append the toast to the toast container div
    const toastContainer = document.getElementById('toastContainer');
    toastContainer.appendChild(toast);

    // Show the toast
    toast.classList.add('show');

    // Remove the toast after 3 seconds
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => {
            toastContainer.removeChild(toast);
        }, 500); // Wait for the transition to finish before removing the toast
    }, 3000);
}