/*
    Authors: Victor Leung and Dondon Herrera
    SODV1202: Introduction to OOP
    April 21, 2025
*/


using System;

class GameMenu
{
    public void ShowMenu()
    {
        Console.WriteLine("Please select game mode.");
        Console.WriteLine("1. Human VS Human");
        Console.WriteLine("2. Human VS AI");
        Console.WriteLine("3. EXIT");
    }
    public string PlayerChoice()
    {
        return Console.ReadLine();
    }

}
//I temperary create this class for the main, you can edit it. --Victor 
class GameMode
{
    public void HumanVSHuman()
    {
        Console.WriteLine("You choose Human VS Human");
        Console.WriteLine("Game Start!");
    }
    public void HumanVSAI()
    {
        Console.WriteLine("You choose Human VS AI");
        Console.WriteLine("Game Start!");
    }
    public void Exit()
    {
        return;
    }
}

namespace Connect_Four
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            GameMenu menu = new GameMenu();
            
            GameMode mode=new GameMode();
            bool chosing = true;
            

            while (chosing) 
            {
                menu.ShowMenu();
                string choice = menu.PlayerChoice();
                if (choice == "1")
                {
                    mode.HumanVSHuman();
                    chosing = false;
                }
                else if (choice == "2")
                {
                    mode.HumanVSAI();
                    chosing = false;
                }
                else if (choice == "3") 
                {
                    mode.Exit();
                    chosing = false;
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
