Feature: Navigation Bar

@navbar
Scenario: See navigation bar
    Given I am on any page
    Then I should see the navigation bar

@navbar
Scenario: Click on a navigation bar link
    Given I am on any page
    When I click on a navigation bar link
    Then I should be redirected to the corresponding page

@navbar
Scenario: Link color on navigation bar changes when clicked
    Given I am on any page
    When I click on a navigation bar link
    Then the link color should change

@navbar
Scenario: Link color on navigation bar changes when hovered
    Given I am on any page
    When I hover over a navigation bar link
    Then the link color should change