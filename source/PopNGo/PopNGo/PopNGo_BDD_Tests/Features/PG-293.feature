Feature: Recaptcha verification on explore page

Should appear when making enough queries while not logged in on the explore page to help prevent abuse

Background:
	Given the following users exist
	  | UserName         | Email                 | FirstName  | LastName | Password  |
	  | Joshua Weiss     | knott@example.com     | Joshua     | Weiss    | FAKE PW   |

Scenario: Recaptcha modal appears after making over ten searches on the explore page while not logged in
  Given I am on the "Explore" page
  When I make over ten searches on the explore page
  Then I should see the recaptcha modal appear

Scenario: Recaptcha modal does not appear after making over ten searches on the explore page while logged in
  Given I am a user with first name 'Joshua'
   And  I login
  And I am on the "Explore" page
  When I make over ten searches on the explore page
  Then I should not see the recaptcha modal appear

