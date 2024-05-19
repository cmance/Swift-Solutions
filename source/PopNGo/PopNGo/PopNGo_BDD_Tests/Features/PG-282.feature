Feature: As a user, I want to receive timely updates about my itinerary and changes to it

While the itinerary interface is useable, the user needs more feedback about the events as they're happening. Ideally, the user can set up an itinerary and throughout the day, receive reminders about what's next on the itinerary and when/where it's going to be. This ensures that the user remains aware of the current time and pacing of events, and can plan accordingly if they need to alter their schedule. The timing of these emails should be left to the user to configure in the itinerary itself.

In addition, the user should be able to send a copy of their itinerary to themselves as an email to keep a record for themselves or show others, should the user wish to.

Background:
    Given the following users exist
        | UserName         | Email                 | FirstName  | LastName | Password  |
        | Tristan Goucher  | knott@example.com     | Tristan    | Goucher  | FAKE PW   |

Scenario: User checks their itinerary to set a reminder time for an event
Given I am a user with first name 'Tristan'
And I login
And I am on the "Itinerary" page
When I am viewing my first itinerary
And I look at the first event
Then I can configure the time to send a reminder notification

Scenario: User tries to set a reminder timer for an event that has already started
Given I am a user with first name 'Tristan'
And I login
And I am on the "Itinerary" page
And I am viewing my first itinerary
And I look at the first event
When I try to configure a time past the event's start
Then the time is not changed from what it originally was

Scenario: User sets a default reminder time for new itinerary events
Given I am a user with first name 'Tristan'
And I login
When I am on the "Profile Notifications" page
Then I can see a default itinerary reminder time setting

Scenario: User wants to use a new default time for itinerary events
Given I am a user with first name 'Tristan'
And I login
And I am on the "Profile Notifications" page
When I change the default itinerary reminder time
And I go to the "Explore" page
And the events have loaded
And I click on the first event card
And I click the "Add to Itinerary" button
And I click the first itinerary
And I am on the "Itinerary" page
And I am viewing my first itinerary
Then I can see the new reminder time on the itinerary for this event