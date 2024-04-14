@Cameron
Feature: Tickets and Venue information on the event cards in the explore page

@EventCards
Scenario: Visitor clicks on the event card
    Given the visitor navigates to the explore page
    When the visitor clicks on any event card
    Then the visitor should see a modal with a "View Venue" and "Buy Tickets" button

@Venue
Scenario: Visitor clicks on the "View Venue" button
    Given the visitor navigates to the explore page
    When the visitor clicks on any event card
    And the visitor clicks on the "View Venue" button
    Then the visitor should be shown the venue information modal

@Tickets
# This scenario is hard to test for because it is dependent on the event and the tickets available
Scenario: Visitor clicks on the "Buy Tickets" button
    Given the visitor navigates to the explore page
    When the visitor clicks on any event card
    And the visitor clicks on the "Buy Tickets" button
    Then the visitor should activate a dropdown with ticket options or the button will be disabled if no ticket options are available

@Tickets
Scenario: Visitor hovers over the "Buy Tickets" button
    Given the visitor navigates to the explore page
    When the visitor clicks on any event card
    And the visitor hovers over the "Buy Tickets" button
    Then the button should have a button title appear

@Venue
Scenario: Visitor hovers over the "View Venue" button
    Given the visitor navigates to the explore page
    When the visitor clicks on any event card
    And the visitor hovers over the "View Venue" button
    Then the button should have a button title appear

# These tests dont work because our website asks for location services to be enabled and we are unable to bypass this