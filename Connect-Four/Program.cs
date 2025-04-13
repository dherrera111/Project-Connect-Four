/*
    Authors: Victor Leung and Dondon Herrera
    SODV1202: Introduction to OOP
    April 21, 2025
*/

using System;
using System.Threading;

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
            Console.WriteLine("======= Welcome to Connect Four Game by Herrera & Leung Co. ======= \n");
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
        private HumanPlayer _player1; // 'X'
        private HumanPlayer _player2; // 'O'
        private GameBoard _gameBoard;
        private bool _isPlayerOneTurn;

        /// <summary>
        /// Constructor: Initialize a new human vs. human game
        /// </summary>
        public HumanVsHumanHandler()
        {
            RegisterPlayers();

            _gameBoard = new GameBoard();
            _gameBoard.InitializeBoard();
            _gameBoard.SetupHumanVsHumanPlayers(_player1, _player2);
            _isPlayerOneTurn = true; // make sure that player one goes first
            _gameBoard.SetPlayerTurn(_isPlayerOneTurn);
        }

        private void RegisterPlayers()
        {
            // Ask for Player 1's name (assigned 'X').
            Console.Write("Enter your name Player 1: ");
            string playerName1 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName1))
            {
                playerName1 = "Connect 4 Player 1";
                Console.WriteLine($"The system automatically set player 1 name to: '{playerName1}' ");
            }
            _player1 = new HumanPlayer(playerName1, 'X');

            //Ask for Player 2's name (assigned 'O').
            Console.Write("Enter your name Player 2: ");
            string playeName2 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playeName2))
            {
                playeName2 = "Connect 4 Player 2";
                Console.WriteLine($"The system automatically set player 2 name to: '{playeName2}' ");
            }
            _player2 = new HumanPlayer(playeName2, 'O');
        }

        /// <summary>
        /// Start of the Game Loop - Human vs. Human Mode
        /// </summary>
        public void PlayTheGame()
        {
            bool isPlayOver = false;

            while (!isPlayOver)
            {
                Console.Clear();
                _gameBoard.DisplayGameBoard(); // show the current board

                int column = _gameBoard.GetPlayerMove(); // get player's move

                if (_gameBoard.IsValidMove(column))
                {
                    int row = _gameBoard.DropDisk(column); // get the row where it landed

                    // check if win
                    if (_gameBoard.IsLastMoveWin(row, column))
                    {
                        DisplayVictoryMessage();
                        isPlayOver = true;
                    }
                    // check if draw
                    else
                    {
                        // else, plays continue
                        _isPlayerOneTurn = !_isPlayerOneTurn; // switch to other player
                        _gameBoard.SetPlayerTurn(_isPlayerOneTurn);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move. Please try again.");
                    Thread.Sleep(1500); // Pause for 1.5 seconds before clearing
                }
            }
        }

        private void DisplayVictoryMessage()
        {
            Console.Clear();
            _gameBoard.DisplayGameBoard();

            Console.ForegroundColor = ConsoleColor.Yellow;

            // Display the congratulatory message with player info
            Console.WriteLine($"\nVictory!\n");
            Console.Write("Congratulations, ");

            // Set the winner's disk color
            if (_isPlayerOneTurn)
            {
                _gameBoard.SetDiskColor(_player1.Disk);
                Console.Write($"{_player1.Name} ({_player1.Disk})");
            }
            else
            {
                _gameBoard.SetDiskColor(_player2.Disk);
                Console.Write($"{_player2.Name} ({_player2.Disk})");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("! You’re the Connect Four Champion!!!\n");
            
            // reset color
            Console.ResetColor();
        }
    }

    //=========================================================
    // 3: PLAYER CLASSES
    //=========================================================

    /// <summary>
    /// The player details - parent class
    /// Defines the common properties and methods for all player types
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// The player name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The disk - O or X
        /// </summary>
        public char Disk { get; }

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
    public class AIPlayer : Player
    {
        public AIPlayer(string name, char disk) : base(name, disk)
        {
        }
    }

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
            board = new char[ROWS, COLUMNS]; // 2D array
            InitializeBoard(); // fill it with # spaces
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
            Console.WriteLine("======= Welcome to Connect Four Game by Herrera & Leung Co. =======");
            Console.WriteLine("\nConnect 4 Game Development Project: \n");

            // loop through each cell with the #
            for (int row = 0; row < ROWS; row++)
            {
                Console.Write("|");

                for (int col = 0; col < COLUMNS; col++)
                {
                    // set the disk color
                    SetDiskColor(board[row, col]);

                    Console.Write($" {board[row, col]} ");

                    // Reset color after each cell
                    Console.ResetColor();
                }

                Console.WriteLine("|");
            }

            // These are the column numbers at the bottom
            Console.WriteLine("  1  2  3  4  5  6  7  \n");
        }

        /// <summary>
        /// To set the color of the disk
        /// </summary>
        /// <param name="disk"></param>
        /// <returns></returns>
        public void SetDiskColor(char disk)
        {
            // Set color based on the cell value
            if (disk == 'X')
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (disk == 'O')
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ResetColor(); // Default color
            }
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
        public int GetPlayerMove()
        {
            Player currentPlayer = _isPlayerOneTurn ? _player1 : _player2;
            Console.Write($"\nYour turn, Player {currentPlayer.Name} (");
            SetDiskColor(currentPlayer.Disk);
            Console.Write(currentPlayer.Disk);
            Console.ResetColor();
            Console.WriteLine(").");
            Console.Write("Enter a column number (1-7) and press Enter: ");

            try
            {
                // convert to indexing system the subtract 1
                int column = Convert.ToInt32(Console.ReadLine()) - 1;
                return column;
            }
            catch
            {
                return -1; // Return invalid column
            }
        }

        /// <summary>
        /// Drop Disk: Place a player's disk in the selected column
        /// </summary>
        /// <param name="col"></param>
        /// <returns>The row where the disk landed</returns>
        public int DropDisk(int col)
        {
            int row = -1;

            // start from the bottom row and move up til find empty cell
            for (int i = ROWS - 1; i >= 0; i--)
            {
                if (board[i, col] == '#') // this is the empty cell
                {
                    // get the current player's disk, player 1 is X, player 2 is O
                    Player currentPlayer = _isPlayerOneTurn ? _player1 : _player2;

                    // place the player's symbol in the empty cell
                    board[i, col] = currentPlayer.Disk;
                    row = i; // row we placed the disk
                    break;
                }
            }

            return row;
        }

        /// <summary>
        /// To check if the move is valid
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsValidMove(int column)
        {
            // validte if the col within the board boundaries
            if (column < 0 || column >= COLUMNS)
            {
                return false;
            }

            // check the column is empty
            return board[0, column] == '#';
        }

        /// <summary>
        /// To check if the last move met the expectations
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsLastMoveWin(int row, int col)
        {
            char playerDisk = _isPlayerOneTurn ? _player1.Disk : _player2.Disk;

            // ==================
            // HORIZONTAL WINS (4 in a row, from left to right)
            // ==================

            int count = 0;

            // Loop in columns from col - 1 to col + 3 = making sure that index stays within board
            for(int i = Math.Max(0, col - 1); i <= Math.Min(col + 3, COLUMNS - 1); i++)
            {
                // check if the current cell at row contains the player disk
                if (board[row, i] == playerDisk)
                {
                    count++; // increment the count
                    if (count == 4) return true; // win
                }
                else
                {
                    // if the count series broken, then reset counter to 0
                    count = 0;
                }
            }

            // ==================
            // VERTICAL WINS (4 in a row, from top to bottom)
            // ==================
            count = 0;

            // Loop through from row - 3 to row + 3
            for (int j = Math.Max(0, row - 3); j <= Math.Min(row + 3, ROWS - 1); j++)
            {
                // check the current cell contains the player disk
                if (board[j, col] == playerDisk)
                {
                    count++; // increment
                    if (count == 4) return true; // win
                }
                else
                {
                    // if the count series broken, then reset counter to 0
                    count = 0;
                }
            }

            // ==================
            // DIAGONAL WINS (top left to bottom right)
            // ==================
            count = 0;
            int startRow = row - Math.Min(row, col); // starting row position
            int startCol = col - Math.Min(row, col); // starting col position

            // Loop diagonal from the starting position, moving down and to the right
            for (int i = 0; i < Math.Min(ROWS - startRow, COLUMNS - startCol); i++)
            {
                // check the current cell contains the player disk
                if (board[startRow + i, startCol + i] == playerDisk)
                {
                    count++; // increment
                    if (count == 4) return true; // win
                }
                else
                {
                    // if the count series broken, then reset counter to 0
                    count = 0;
                }
            }

            // ==================
            // DIAGONAL WINS (top right to bottom left)
            // ==================
            count = 0;
            startRow = row - Math.Min(row, COLUMNS - 1 - col); // starting row position
            startCol = col + Math.Min(row, COLUMNS - 1 - col); // starting col position

            // Loop diagonal from the starting position, moving down and to the left
            for (int i = 0; i < Math.Min(ROWS - startRow, startCol + 1); i++)
            {
                // check the current cell contains the player disk
                if (board[startRow + i, startCol - i] == playerDisk)
                {
                    count++; // increment
                    if (count == 4) return true; // win
                }
                else
                {
                    // if the count series broken, then reset counter to 0
                    count = 0;
                }
            }

            return false; // no win
        }
    }
}
