@regression @smoke
Feature: eBay Product Search
  As an eBay customer
  I want to search for products and add them to cart
  So that I can purchase items I need

  Background:
    Given I navigate to eBay home page
    And I verify home page is loaded
    And I set shipping country to "Bulgaria"

  @product-search
  Scenario: Search and add Monopoly game to cart
    When I select "Toys & Hobbies" category
    And I search for "Monopoly: Elf Edition Board Game"
    And I switch to List view
    Then I should see product 1 with title "Monopoly: Elf Edition Board Game"
    And I should see product 1 with price "$39.99"
    
    When I open product 1 details
    Then product details should show title "Monopoly: Elf Edition Board Game"
    And product details should show price "$39.99"
    
    When I set quantity to 2
    And I view shipping details
    Then shipping should be available to "Bulgaria"
    
    When I view payment methods
    Then "Visa" should be accepted
    And I close shipping modal
    
    When I add to cart
    And I open cart
    Then cart should contain 2 items
    And cart total should be "US $79.98"
