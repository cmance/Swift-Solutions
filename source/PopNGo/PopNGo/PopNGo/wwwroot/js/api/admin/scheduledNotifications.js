import { getUserInfo } from './users.js';

export async function sendNotificationEmail(notificationId) {
    try {
        const response = await fetch(`/api/admin/scheduledNotifications/sendEmail/notificationId=${notificationId}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function getNotificationInfo(notificationId) {
    try {
        const response = await fetch(`/api/admin/scheduledNotifications/getNotificationInfo/notificationId=${notificationId}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        let data = await response.json();
        const userInfo = await getUserInfo(data.userId);
        delete userInfo.id;
        delete userInfo.email;
        delete userInfo.notifyDayOf;
        delete userInfo.notifyWeekBefore;
        delete userInfo.notifyDayBefore;

        data = {...data, ...userInfo}
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function updateNotification(notificationId, data) {
    try {
        const response = await fetch(`/api/admin/scheduledNotifications/updateNotification/notificationId=${notificationId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const responseData = await response.json();
        return responseData;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function deleteNotification(notificationId) {
    try {
        const response = await fetch(`/api/admin/scheduledNotifications/deleteNotification/notificationId=${notificationId}`, {
            method: 'DELETE'
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}