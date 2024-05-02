Feature: Bookmark Lists Name Editing

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Joshua Weiss     | knott@example.com     | Joshua     | Weiss    | FAKE PW   |

Scenario: I see an edit button on the bookmark lists
	Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
  When I have created a new bookmark list
  Then I should see a button to edit the bookmark list

Scenario: I can see a way to edit the bookmark list name
  Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
    And I have created a new bookmark list
  When I click the edit button on the new bookmark list
  Then I should see a form to edit the bookmark list name

Scenario: I can edit the bookmark list name
  Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
    And I have created a new bookmark list
    And I click the edit button on the new bookmark list
  When I fill in the form with a new name
    And I click the save button in the edit bookmark list form
  Then I should see the new name on the bookmark list

Scenario: I cant edit the bookmark list name to be blank
  Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
    And I have created a new bookmark list
    And I click the edit button on the new bookmark list
  When I fill in the update bookmark list name form with a blank name
  Then The save edit button should be disabled

Scenario: I cant edit the bookmark list name to be a duplicate
  Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
    And I have created a new bookmark list
    And I click the edit button on the new bookmark list
  When I fill in the update bookmark list name form with the same name
  Then The save edit button should be disabled

Scenario: I can cancel editing the bookmark list name
  Given I am a user with first name 'Joshua'
    And I login
    And I am on the "Favorites" page
    And I have created a new bookmark list
    And I click the edit button on the new bookmark list
  When  I click the cancel button in the edit bookmark list form
  Then  I should see the original name on the bookmark list
    And I should not see the edit bookmark list form
