@Cameron
Feature: Accessibility

As a user, I want a screen reader to read images and buttons so that I can navigate the site without seeing it.

@accessibility
Scenario: See the home page
    Given the user is on the home page 2
    Then all images should have an alt attribute and all buttons should have an aria label