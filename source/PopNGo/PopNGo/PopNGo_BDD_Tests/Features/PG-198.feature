Feature: Bookmark Lists Image Preview

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Joshua Weiss     | knott@example.com     | Joshua     | Weiss    | FAKE PW   |

Scenario: New bookmark list should have a default image
	Given I am a user with first name 'Joshua'
	 And  I login
	 And I am on the "Favorites" page
	When I fill out and submit the new bookmark list form with a unique title
# with key newBookmarkListTitle
	Then I should see the new bookmark list displayed
    And The new bookmark list should have a default image
