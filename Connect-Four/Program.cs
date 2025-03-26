/*
    Authors: Victor Leung and Dondon Herrera
    SODV1202: Introduction to OOP
    April 21, 2025
*/

using System;

namespace Connect_Four
{
    public static class GameMenu
    {
        public static void ShowMenu()
        {
            Console.WriteLine("Please select game mode.");
            Console.WriteLine("1. Human VS Human");
            Console.WriteLine("2. Human VS AI");
            Console.WriteLine("3. EXIT");
        }
        public static string PlayerChoice()
        {
            return Console.ReadLine();
        }

    }
    
    public static class GameMode
    {
        public static void HumanVSHuman()
        {
            Console.WriteLine("You choose Human VS Human");
            Console.WriteLine("Game Start!");
        }
        public static void HumanVSAI()
        {
            Console.WriteLine("You choose Human VS AI");
            Console.WriteLine("Game Start!");
        }
        public static void Exit()
        {
            Console.WriteLine("Exiting Game, Bye!");
            return;
        }
    }//I temperary create this class for the main, you can edit it. --Victor 
    internal class Program
    {
        static void Main(string[] args)
        {           
            bool choosing = true;           

            while (choosing) 
            {
                GameMenu.ShowMenu();
                string choice = GameMenu.PlayerChoice();
                if (choice == "1")
                {
                    GameMode.HumanVSHuman();
                    choosing = false;
                }
                else if (choice == "2")
                {
                    GameMode.HumanVSAI();
                    choosing = false;
                }
                else if (choice == "3") 
                {
                    GameMode.Exit();
                    choosing = false;
                }
                else 
                {
                    Console.WriteLine("Invaild number, please enter 1, 2 or 3");
                }

                
            }
        }
    }
}


/*****************************Task 1****************************

                            Victor
 
 * To create Menu with option 1,2,3 and exit to enter the game board
 
*****************************************************************

                             Dondon
 
 * To create the game board
 
 * *************************************************************/
