/*
    Authors: Victor Leung and Dondon Herrera
    SODV1202: Introduction to OOP
    April 21, 2025
*/

using System;

namespace Connect_Four
{
    //=========================================================
    // 1: MAIN AND MENU PROGRAM
    //=========================================================

    /// <summary>
    /// The program
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool choosing = true;

            while (choosing)
            {
                GameMenu.ShowMenu();
                Console.Write("Select from 1-3 then press Enter: ");
                string choice = GameMenu.PlayerChoice();
                if (choice == "1")
                {
                    GameMode.HumanVsHuman();
                    choosing = false;
                }
                else if (choice == "2")
                {
                    GameMode.HumanVsAI();
                    choosing = false;
                }
                else if (choice == "3")
                {
                    GameMode.Exit();
                }
                else
                {
                    Console.WriteLine("Invaild choice, please enter 1, 2, or 3");
                }
            }
        }
    }

    /// <summary>
    /// The game menu: displays the main menu and handles user selection
    /// </summary>
    public static class GameMenu
    {
        public static void ShowMenu()
        {
            Console.WriteLine("Welcome to Connect Four Game by Herrera & Leung Co.\n");
            Console.WriteLine("Please select game mode: \n");
            Console.WriteLine("1. Human vs Human mode");
            Console.WriteLine("2. Human vs AI mode");
            Console.WriteLine("3. Exit the game \n");
        }
        public static string PlayerChoice()
        {
            return Console.ReadLine();
        }
    }

    /// <summary>
    /// The game mode: handles the user selection
    /// </summary>
    public static class GameMode
    {
        public static void HumanVsHuman()
        {
            Console.Write("You chose Human vs Human \n");
            HumanVsHumanHandler game = new HumanVsHumanHandler();
            game.PlayTheGame();
        }
        public static void HumanVsAI()
        {
            Console.Write("You chose Human VS AI \n");
            Console.WriteLine("Game Start!");
        }
        public static void Exit()
        {
            Console.WriteLine("Exiting Game, Bye!");
            return;
        }
    }

    //=========================================================
    // 2: GAME MODE
    //=========================================================

    /// <summary>
    /// The human vs. human handler: manages a human vs. human game
    /// </summary>
    public class HumanVsHumanHandler
    {
        private HumanPlayer player1; // 'X'
        private HumanPlayer player2; // 'O'
        private GameBoard gameBoard;
        private bool isPlayerOneTurn;

        /// <summary>
        /// Constructor: Initialize a new human vs. human game
        /// </summary>
        public HumanVsHumanHandler()
        {
            RegisterPlayers();
            gameBoard = new GameBoard();
            gameBoard.SetupHumanVsHumanPlayers(player1, player2);
            isPlayerOneTurn = true; // make sure that player one goes first
            gameBoard.SetPlayerTurn(isPlayerOneTurn);
        }

        private void RegisterPlayers()
        {
            // Ask for Player 1's name (assigned 'X').
            Console.Write("Enter your name Player 1: ");
            string playeName1 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playeName1))
            {
                playeName1 = "Connect 4 Player 1";
                Console.WriteLine($"The system automatically set player 1 name to: '{playeName1}' ");
            }
            player1 = new HumanPlayer(playeName1, 'X');

            //Ask for Player 2's name (assigned 'O').
            Console.Write("Enter your name Player 2: ");
            string playeName2 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playeName2))
            {
                playeName2 = "Connect 4 Player 2";
                Console.WriteLine($"The system automatically set player 2 name to: '{playeName2}' ");
            }
            player2 = new HumanPlayer(playeName2, 'O');
        }

        /// <summary>
        /// Start of the Game Loop
        /// </summary>
        public void PlayTheGame()
        {
            gameBoard.AskPlayerToDropDisk();
        }
    }

    //=========================================================
    // 3: PLAYER CLASSES
    //=========================================================

    /// <summary>
    /// The player details - parent class
    /// Defines the common properties and methods for all player types
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The disk - O or X
        /// </summary>
        public char Disk { get; set; }

        /// <summary>
        /// Initializes a new player object
        /// </summary>
        /// <param name="name">The player name</param>
        /// <param name="disk">X or O</param>
        public Player(string name, char disk)
        {
            Name = name;
            Disk = disk;
        }
    }

    /// <summary>
    /// The human player (child) - inherits from the player class
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// calls the parent class constructor
        /// </summary>
        /// <param name="name">The human player name</param>
        /// <param name="disk">X or O</param>
        public HumanPlayer(string name, char disk) : base(name, disk)
        {
        }
    }

    // TODO: To implement AI Player here...

    //=========================================================
    // 4: GAME BOARD CLASSES
    //=========================================================

    /// <summary>
    /// The game board - manages the game status
    /// </summary>
    public class GameBoard
    {
        // Set the size of the game board
        public const int ROWS = 6;
        public const int COLUMNS = 7;

        public char[,] board;
        private bool _isPlayerOneTurn; // sets whose turn it is
        private HumanPlayer _player1; // first player always human
        private Player _player2; // second player can be human or AI, use the parent or common class

        public GameBoard()
        {
            board = new char[ROWS, COLUMNS];
            InitializeBoard(); // fill it with # spaces
            DisplayGameBoard(); // display the current game board
        }

        /// <summary>
        /// Fill the board with #
        /// </summary>
        public void InitializeBoard()
        {
            // loop through each cell
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    board[row, col] = '#'; // set # each cell
                }
            }
        }

        /// <summary>
        /// Displays the current game board.
        /// </summary>
        public void DisplayGameBoard()
        {
            Console.WriteLine("\nConnect 4 Game Development Project: \n");

            // loop through each cell with the #
            for (int row = 0; row < ROWS; row++)
            {
                Console.Write("|");

                for (int col = 0; col < COLUMNS; col++)
                {
                    Console.Write($" {board[row, col]} ");
                }

                Console.WriteLine("|");
            }

            // These are the column numbers at the bottom
            Console.WriteLine("  1  2  3  4  5  6  7  \n");
        }

        /// <summary>
        /// Set Player Turn
        /// </summary>
        /// <param name="isPlayerOneTurn"></param>
        public void SetPlayerTurn(bool isPlayerOneTurn)
        {
            _isPlayerOneTurn = isPlayerOneTurn;
        }

        /// <summary>
        /// To set up the Player object to use the player's name and disk in this class.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public void SetupHumanVsHumanPlayers(HumanPlayer player1, HumanPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        /// <summary>
        /// Ask the current player to select a column
        /// </summary>
        /// <returns>The column selected by player</returns>
        public int AskPlayerToDropDisk()
        {
            Player currentPlayer = _isPlayerOneTurn ? _player1 : _player2;
            Console.WriteLine($"\nYour turn, Player {currentPlayer.Name}.");
            Console.Write("Enter a column number (1-7) and press Enter: ");

            while (true) // loop until valid input is entered
            {
                string input = Console.ReadLine().Trim();

                if (int.TryParse(input, out int column) && column >= 1 && column <= 7)
                {
                    return column;
                }

                Console.WriteLine("Invalid turn. Please enter a number between 1 and 7.");
            }
        }
    }
}
