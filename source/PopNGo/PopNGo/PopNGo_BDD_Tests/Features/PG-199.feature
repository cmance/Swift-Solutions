@Tristan
Feature: Admin Metrics

Building upon the prevoius work, an admin needs to be able to see an accounting of the various metrics related to the site. With this knowledge, the admin can monitor the traffic and activity to watch for any anomalies and patterns in operations, such as the emails sent out or the times of searches.

Scenario: Seeing the Metrics page
Given I am an admin
And I login
When I am on the "Admin" page
Then I should see a way to navigate to the 'Metrics' page

Scenario: Seeing the Email Activity Chart
Given I am an admin
And I login
When I am on the "Metrics" page
Then I should see a graph of metrics with the id 'email-activity-metrics-chart'

Scenario: Seeing the User Accounts Chart
Given I am an admin
And I login
When I am on the "Metrics" page
Then I should see a graph of metrics with the id 'user-account-metrics-chart'

Scenario: Seeing the Event Activity Chart
Given I am an admin
And I login
When I am on the "Metrics" page
Then I should see a graph of metrics with the id 'event-activity-metrics-chart'