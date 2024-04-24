@Tristan
Feature: Scheduled Notifications for Admin Control

As an admin, I'd like to be able to see and delete scheduled notifications

Sometimes the site may go down or be unresponsive at midnight when the notification emails are supposed to be prepared. Currently, this would delay the next batch until the following midnight. An adminstrator should be able to instead alter the time of the scheduled job, or even delete it if they so wish. This involves converting timed notifications for emails and texts into scheduled jobs to be processed at the appointed times.

@scheduledNotifications
Scenario: Find the Scheduled Notifications area
Given I am an admin
When I am on the "Admin" page
Then I should see a way to navigate to the 'Notifications' page

@scheduledNotifications
Scenario: See the Scheduled Notifications area
Given I am an admin
When I am on the "Notifications" page
Then I should see all of the scheduled notifications

@scheduledNotifications
Scenario: See buttons for Editing and Deleting Scheduled Notifications
Given I am an admin
And I am on the "Notifications" page
Then I should be able to edit or delete a scheduled notification

@scheduledNotifications
Scenario: I have edited a Scheduled Notification
Given I am an admin
 And I am on the "Notifications" page
When I have edited a scheduled notification
Then I should see the new time on the page

@scheduledNotifications
Scenario: I have deleted a Scheduled Notification
Given I am an admin
 And I am on the "Notifications" page
When I have deleted a scheduled notification
Then I should not see it in the list

@scheduledNotifications
Scenario: I have deleted a Scheduled Notification and see it rescheduled
Given I am an admin
 And I am on the "Notifications" page
When I have deleted a scheduled notification
Then I should see a new scheduled notification for the next time period after the previously-scheduled one