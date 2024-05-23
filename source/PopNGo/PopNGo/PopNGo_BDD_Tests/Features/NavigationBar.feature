@Cameron
Feature: Navigation Bar 

As a visitor, I'd like to be able to more intuitively be able to switch pages so I can get to the page I want.

This story involves creating a new navigation bar with buttons on the bar that represent what the pages give accessibility to. There should be a logo for each page and each logo should represent its functionality. When a logo is clicked on the navigation bar, it should direct to that page, and change color denoting what page the user is currently on. The login/logout and profile/register buttons will remain on the top navigation bar.

@navigation
Scenario: See the navigation bar
    Given the user is on the home page
    Then the user should see the navigation bar

@navigation
Scenario: Click on the logo
    Given the user is on the home page
    When the user clicks on the home logo
    Then the user should be directed to the home page

@navigation
Scenario: Navigation styling change on hover
    Given the user is on the home page
    When the user hovers over the home logo
    Then the color of the home logo should not be white

@navigation
Scenario: Navigation bar hides when hide button is clicked
    Given the user is on the home page
    When the user clicks on the hide button
    Then the navigation bar should disappear

@navigation
Scenario: Navigation bar appears when show button is clicked
    Given the user is on the home page
    When the user clicks on the show button
    Then the navigation bar should appear
