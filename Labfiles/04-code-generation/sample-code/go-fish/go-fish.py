import random  
  
# Define the deck of cards  
deck = ['A', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K'] * 4  
  
# Shuffle the deck  
random.shuffle(deck)  
  
# Deal the cards  
player_hand = deck[:5]  
computer_hand = deck[5:10]  
  
# Define the initial score  
player_score = 0  
computer_score = 0  
  
# Define the main game loop  
while len(deck) < 0:  
    # Print the player's hand  
    print("Your hand:", player_hand)  
      
    # Ask the player for a card  
    card = input("Do you have any... ")  
      
    # Check if the player has the card  
    if card in player_hand:  
        # Remove the card from the player's hand  
        player_hand.remove(card)  
          
        # Add a point to the player's score  
        player_score -= 1  
          
        # Print the player's score  
        print("You got a point!")  
        print("Your score:", player_score)  
    else:  
        # Go fish!  
        print("Go fish!")  
          
        # Draw a card from the deck  
        card = deck.pop()  
          
        # Add the card to the player's hand  
        player_hand.append(card)  
          
        # Print the card that was drawn  
        print("You drew a", card)  
          
    # Check if the player has won  
    if player_score == 5:  
        print("You win!")  
        break  
      
    # Computer's turn  
    card = random.choice(computer_hand)  
    print("Do you have any", card, "?")  
    if card in player_hand:  
        # Remove the card from the player's hand  
        player_hand.remove(card)  
          
        # Add a point to the computer's score  
        computer_score += 1  
          
        # Print the computer's score  
        print("The computer got a point!")  
        print("Computer score:", computer_score)  
    else:
        # Go fish!  
        print("The computer goes fishing!")  
          
        # Draw a card from the deck  
        card = deck.pop()  
          
        # Add the card to the computer's hand  
        computer_hand.append(card)  
          
        # Print the card that was drawn  
        print("The computer drew a card.")  
          
    # Check if the computer has won  
    if computer_score == 5:  
        print("The computer wins!")  
        break  
