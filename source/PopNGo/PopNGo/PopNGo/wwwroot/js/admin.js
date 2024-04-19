import { buildAdminUserDetailsModal } from './ui/buildAdminUserDetailsModal.js';
import { sendVerificationEmail, sendPasswordResetEmail, confirmUserAccount, deleteUser, getUserInfo } from './api/admin/users.js';
import { buildAdminScheduleDetailsModal } from './ui/buildAdminScheduleDetailsModal.js';
import { sendNotificationEmail, getNotificationInfo, deleteNotification } from './api/admin/scheduledNotifications.js';
import { formatStartTime } from './util/formatStartTime.js';

document.addEventListener('DOMContentLoaded', async function () {
    // Check which tab is active and load the appropriate data
    const activeTab = document.querySelector('.nav-link.active').innerText;
    if(activeTab === "Users") ProcessUsersTab();
    else if(activeTab === "Scheduled Notifications") ProcessScheduledNotificationsTab();

});

function ProcessUsersTab() {
    Array.from(document.getElementsByName("admin-users-table")[0].querySelector('tbody').getElementsByTagName('tr')).forEach(t => {
        if(t.dataset.id) {
            t.querySelector('#resend-verification-email').addEventListener('click', async function() {
                try {
                    const data = await sendVerificationEmail(t.dataset.id);
                    console.log(data);
                } catch (error) {
                    console.error(error);
                }
            });
            t.querySelector('#send-password-reset').addEventListener('click', async function() {
                try {
                    const data = await sendPasswordResetEmail(t.dataset.id);
                    console.log(data);
                } catch (error) {
                    console.error(error);
                }
            });
            t.querySelector('#confirm-account').addEventListener('click', async function() {
                try {
                    const data = await confirmUserAccount(t.dataset.id);
                    console.log(data);
                } catch (error) {
                    console.error(error);
                }
            });
            t.querySelector('#edit-user').addEventListener('click', async function() {
                buildAdminUserDetailsModal(document.getElementById('admin-user-details-modal'), await getUserInfo(t.dataset.id));
                const modal = new bootstrap.Modal(document.getElementById('admin-user-details-modal'));
                modal.show();
                while(document.getElementById('admin-user-details-modal').classList.contains('show')) {
                    await new Promise(resolve => setTimeout(resolve, 100));
                }
                console.log("modal closed");
                const userData = await getUserInfo(t.dataset.id);
                const previousRow = t.previousElementSibling;
                previousRow.querySelector(':nth-child(1)').innerText = userData.userName;
                previousRow.querySelector(':nth-child(2)').innerText = userData.email;
                previousRow.querySelector(':nth-child(3)').innerText = userData.notificationEmail;
    
            });
            t.querySelector('#delete-user').addEventListener('click', async function() {
                try {
                    const data = await deleteUser(t.dataset.id);
                    console.log(data);
                    if(data) {
                        t.previousElementSibling.remove();
                        t.remove();
                    }
                } catch (error) {
                    console.error(error);
                }
            });
        }
    });
}

function ProcessScheduledNotificationsTab() {
    Array.from(document.getElementsByName("admin-schedules-table")[0].querySelector('tbody').getElementsByTagName('tr')).forEach(t => {
        if(t.dataset.userid) {
            const buttonRow = t.nextElementSibling;
            if(buttonRow === null) console.error("No button row found for schedule id: " + t.dataset.scheduleid);
            buttonRow.querySelector('#send-notification-email').addEventListener('click', async function() {
                console.log("send-notification-email clicked");
                try {
                    const data = await sendNotificationEmail(t.dataset.scheduleid);
                    console.log(data);
                } catch (error) {
                    console.error(error);
                }
            });
            buttonRow.querySelector('#edit-notification').addEventListener('click', async function() {
                buildAdminScheduleDetailsModal(document.getElementById('admin-schedule-details-modal'), await getNotificationInfo(t.dataset.scheduleid));
                const modal = new bootstrap.Modal(document.getElementById('admin-schedule-details-modal'));
                modal.show();
                while(document.getElementById('admin-schedule-details-modal').classList.contains('show')) {
                    await new Promise(resolve => setTimeout(resolve, 500));
                }
                const scheduleData = await getNotificationInfo(t.dataset.scheduleid);
                t.querySelector(':nth-child(3)').innerText = scheduleData.type;
                t.querySelector(':nth-child(4)').innerText = formatStartTime(scheduleData.time);
            });
            buttonRow.querySelector('#delete-notification').addEventListener('click', async function() {
                try {
                    const data = await deleteNotification(t.dataset.scheduleid);
                    if(data > 0) {
                        t.dataset.scheduleid = +data;
                        const scheduleData = await getNotificationInfo(t.dataset.scheduleid);
                        t.querySelector(':nth-child(3)').innerText = scheduleData.type;
                        t.querySelector(':nth-child(4)').innerText = formatStartTime(scheduleData.time);
                    }
                } catch (error) {
                    console.error(error);
                }
            });
        }
    });
}
