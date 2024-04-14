Feature: Bookmark Lists

This story involves expanding the functionality of the favorites feature to allow for saving events into specific user-created lists. This story does not include removing or modifying the contents of the new bookmark lists or the bookmark lists themselves.

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Joshua Weiss     | knott@example.com     | Joshua     | Weiss    | FAKE PW   |

Scenario: There is a bookmark list and a way to add a new bookmark list on the favorites page
	Given I am a user with first name 'Joshua'
	 And  I login
	When I am on the "Favorites" page
	Then I should see a bookmark list
	 And I should see a way to create a new bookmark list

Scenario: I can create a new bookmark list
	Given I am a user with first name 'Joshua'
	 And  I login
	 And I am on the "Favorites" page
	When I fill out and submit the new bookmark list form with a unique title
# with key newBookmarkListTitle
	Then I should see the new bookmark list displayed

Scenario: I cannot create a new bookmark list with an empty title
	Given I am a user with first name 'Joshua'
	 And  I login
	 And I am on the "Favorites" page
	When I fill out the new bookmark list name input with an empty value
	Then I should see the create bookmark list button is disabled

Scenario: The new bookmark list form is cleared after submission
	Given I am a user with first name 'Joshua'
	 And  I login
	 And I am on the "Favorites" page
	When I fill out and submit the new bookmark list form with a unique title
	Then I should see the new bookmark list form is cleared
	And I should see the create bookmark list button is disabled
