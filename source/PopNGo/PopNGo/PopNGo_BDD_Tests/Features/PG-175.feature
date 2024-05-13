Feature: Bookmark Lists Deletion

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Joshua Weiss     | knott@example.com     | Joshua     | Weiss    | FAKE PW   |

Scenario: I see a delete button on bookmark lists
	Given I am a user with first name 'Joshua'
	 And  I login
	 And I am on the "Favorites" page
	When I fill out and submit the new bookmark list form with a unique title
# with key newBookmarkListTitle
	Then I should see the new bookmark list displayed
    And I should see a delete button for the new bookmark list

Scenario: I see a confirmation dialog when I click the delete button
  Given I am a user with first name 'Joshua'
   And  I login
   And I am on the "Favorites" page
   And I have created a new bookmark list
  When I click the delete button for the new bookmark list
  Then I should see a delete bookmark list confirmation dialog

Scenario: I can cancel the deletion of a bookmark list
  Given I am a user with first name 'Joshua'
   And  I login
   And I am on the "Favorites" page
   And I have created a new bookmark list
   And I have clicked the delete button for the new bookmark list
  When I click the cancel button in the delete bookmark list confirmation dialog
  Then I should not see the bookmark list confirmation dialog
   And I should see the new bookmark list

Scenario: I can delete a bookmark list
  Given I am a user with first name 'Joshua'
   And  I login
   And I am on the "Favorites" page
   And I have created a new bookmark list
   And I have clicked the delete button for the new bookmark list
  When I click the confirm button in the delete bookmark list confirmation dialog
  Then I should not see the new bookmark list
   And I should not see the delete bookmark list confirmation dialog
