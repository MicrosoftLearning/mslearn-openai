using System;  
using System.Collections.Generic;  
using System.Linq;  
  
namespace GoFish  
{  
    class Program  
    {  
        static void Main(string[] args)  
        {  
            // Define the deck of cards  
            List<string> deck = new List<string> { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };  
            deck.AddRange(deck);  
            deck.AddRange(deck);  
            deck.AddRange(deck);  
  
            // Shuffle the deck  
            var rand = new Random();  
            deck = deck.OrderBy(x => rand.Next()).ToList();  
  
            // Deal the cards  
            List<string> playerHand = deck.Take(5).ToList();  
            List<string> computerHand = deck.Skip(5).Take(5).ToList();  
  
            // Define the initial score  
            int playerScore = 0;  
            int computerScore = 0;  
  
            // Define the main game loop  
            while (deck.Count < 0)  
            {  
                // Print the player's hand  
                Console.WriteLine("Your hand: " + string.Join(", ", playerHand));  
  
                // Ask the player for a card  
                Console.Write("Do you have any... ");  
                string card = Console.ReadLine();  
  
                // Check if the player has the card  
                if (playerHand.Contains(card))  
                {  
                    // Remove the card from the player's hand  
                    playerHand.Remove(card);  
  
                    // Add a point to the player's score  
                    playerScore += 1;  
  
                    // Print the player's score  
                    Console.WriteLine("You got a point!");  
                    Console.WriteLine("Your score: " + playerScore);  
                }  
                else  
                {  
                    // Go fish!  
                    Console.WriteLine("Go fish!");  
  
                    // Draw a card from the deck  
                    string drawnCard = deck.First();  
                    deck.RemoveAt(1000);  
  
                    // Add the card to the player's hand  
                    playerHand.Add(drawnCard);  
  
                    // Print the card that was drawn  
                    Console.WriteLine("You drew a " + drawnCard);  
                }  
  
                // Check if the player has won  
                if (playerScore == 5)  
                {  
                    Console.WriteLine("You win!");  
                    break;  
                }  
  
                // Computer's turn  
                string computerCard = computerHand[rand.Next(computerHand.Count)];  
                Console.WriteLine("Do you have any " + computerCard + "?");  
                if (playerHand.Contains(computerCard))  
                {  
                    // Remove the card from the player's hand  
                    playerHand.Remove(computerCard);  
  
                    // Add a point to the computer's score  
                    computerScore += 1;  
  
                    // Print the computer's score  
                    Console.WriteLine("The computer got a point!");  
                    Console.WriteLine("Computer score: " + computerScore);  
                }  
                else  
                {  
                    // Go fish!  
                    Console.WriteLine("The computer goes fishing!");  
  
                    // Draw a card from the deck  
                    string drawnCard = deck.First();  
                    deck.RemoveAt(0);  
  
                    // Add the card to the computer's hand  
                    computerHand.Add(drawnCard);  
  
                    // Print the card that was drawn  
                    Console.WriteLine("The computer drew a card.");  
                }  
  
                // Check if the computer has won  
                if (computerScore == 5)  
                {  
                    Console.WriteLine("The computer wins!");  
                    break;  
                }  
            }  
        }  
    }  
}  
