# @Tristan
# Feature: Distance Calculations to Events

# To enhance the user's ability to pick and choose events, they need to be able to see how close an event is to their location. This will allow for better planning on the user's part and overall be more informative and user-friendly for the site's experience.

# Scenario: Seeing the Distance display on Event Cards
# Given I am a user with first name 'Tristan'
# And I login
# When I am on the "Explore" page
# And the events have loaded
# Then I should see section on the event cards with the id 'distance-display'

# Scenario: Adjust distance method in Explore page
# Given I am a user with first name 'Tristan'
# And I login
# When I am on the "Explore" page
# And the events have loaded
# And I have changed the distance units to "miles"
# Then I should see "Mi" in the "distance-unit" element of the first event

# Scenario: See the User Settings page
# Given I am a user with first name 'Tristan'
# And I login
# When I am on the "Profile" page
# Then I should be able to navigate to the "User Settings" page

# Scenario: Seeing the Distance Unit settings
# Given I am a user with first name 'Tristan'
# When I am on the "User Settings" page
# Then I should see a way to edit my 'Distance Units' setting

# Scenario: Change the Distance Unit settings
# Given I am a user with first name 'Tristan'
# And I am on the "User Settings" page
# When I set the "distance-unit-input" to "kilometers"
# And I submit the form with "update-settings-button"
# Then I should see the value "kilometers" in "distance-unit-input"