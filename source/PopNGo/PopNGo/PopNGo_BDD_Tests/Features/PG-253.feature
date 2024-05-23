Feature: Event tag filter button in favorites and history page

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Tristan Goucher  | knott@example.com     | Tristan    | Goucher  | FAKE PW   |

Scenario: Bookmark list events section should have filter by tag dropdown button
  Given I am a user with first name 'Tristan'
   And  I login
   And I am on the "Favorites" page
   And I have created a new bookmark list
  When I click on the new bookmark list
  Then I should see the filter by tag dropdown button 

Scenario: History list events section should have filter by tag dropdown button
  Given I am a user with first name 'Tristan'
   And  I login
   And I am on the "History" page
  Then I should see the filter by tag dropdown button
