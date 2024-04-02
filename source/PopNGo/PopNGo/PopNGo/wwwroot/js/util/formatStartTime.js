export function formatStartTime(dateString) {
    const options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(dateString).toLocaleDateString(undefined, options);
}

export function formatDateWithWeekday(startTime) {
    const date = new Date(startTime);
    return date.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' });
}

export function formatHourMinute(startTime) {
    const date = new Date(startTime);
    return date.toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric' });
}
