@Cameron
Feature: Accessibility

As a user, I want a screen reader to read images and buttons so that I can navigate the site without seeing it.

@accessibility
Scenario: See the home page
    Given the user is on the home page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the explore page
    Given the user is on the explore page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the favorites page
    Given the user is on the favorites page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the history page
    Given the user is on the history page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the itinerary page
    Given the user is on the itinerary page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the profile page
    Given the user is on the profile page 2
    Then all images should have an alt attribute and all buttons should have an aria label

Scenario: See the recommendation page
    Given the user is on the recommendation page 2
    Then all images should have an alt attribute and all buttons should have an aria label


    
