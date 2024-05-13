import { validateObject } from '../validation.js';
import { updateNotification } from '../api/admin/scheduledNotifications.js';

let notificationId = null;
function getNotificationId() {
    return notificationId;
}
function setNotificationId(id) {
    notificationId = id;
}

/**
 * Takes in the admin's schedule details modal element and the schedule info props and updates the schedule template
 * Props:
 * {
 *   firstName: String,
 *   lastName: String,
 *   userName: String,
 *   notificationEmail: String,
 *   type: String,
 *   time: DateTime-local,
 * }
 * 
 * @function buildAdminScheduleDetailsModal
 * @param {HTMLElement} adminScheduleDetailElement 
 * @param {Object} props 
 */
export const buildAdminScheduleDetailsModal = (adminDetailsModalElement, props) => {
    setNotificationId(props.id);

    // Set up modal element variables
    const firstNameElement = adminDetailsModalElement.querySelector('#schedule-details-first-name');
    const lastNameElement = adminDetailsModalElement.querySelector('#schedule-details-last-name');
    const userNameElement = adminDetailsModalElement.querySelector('#schedule-details-username');
    const notificationEmailElement = adminDetailsModalElement.querySelector('#schedule-details-notification-email');
    const typeElement = adminDetailsModalElement.querySelector('#schedule-details-type');
    const timeElement = adminDetailsModalElement.querySelector('#schedule-details-time');
    const saveButtonElement = adminDetailsModalElement.querySelector('#schedule-details-save');

    // Set the first and last name
    firstNameElement.value = props.firstName;
    lastNameElement.value = props.lastName;

    // Set the username
    userNameElement.value = props.userName;
    
    // Set the notification email
    notificationEmailElement.value = props.notificationEmail;

    // Set the type and time
    typeElement.value = props.type;
    timeElement.value = props.time;

    // Bind the save button
    saveButtonElement.addEventListener('click', async function() {
        try {
            const data = await updateNotification(getNotificationId(), {
                type: typeElement.value,
                time: timeElement.value,
                id: getNotificationId(),
            });
        } catch (error) {
            console.error(error);
        }

        const modal = bootstrap.Modal.getInstance(adminDetailsModalElement);
    });
}


/**
 * Validates the props for the buildAdminUserDetailsModal function, returns true if the props are valid, false otherwise
 * @param {any} data
 * @returns {boolean}
 */
export function validateBuildAdminScheduleDetailsModalProps(data) {
    if (data === undefined || data === null) {
        return false;
    }

    const schema = {
        firstName: x => typeof x === 'string',
        lastName: x => typeof x === 'string',
        userName: x => typeof x === 'string',
        notificationEmail: x => typeof x === 'string',
        type: x => typeof x === 'string',
        time: x => typeof x === 'datetime-local',
    }

    return validateObject(data, schema).length === 0;
}