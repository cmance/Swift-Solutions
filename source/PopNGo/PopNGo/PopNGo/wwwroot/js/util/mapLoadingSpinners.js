export function addMapLoadingSpinner() {
    let loadingOverlay = document.getElementById('loading-overlay');
    if (!loadingOverlay) return; // If the element doesn't exist, exit the function
    loadingOverlay.style.display = 'flex';
}

export function removeMapLoadingSpinner() {
    document.getElementById('loading-overlay').style.display = 'none';
}