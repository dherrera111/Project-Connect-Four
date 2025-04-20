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
            GameMenu.ShowMenu(); // show the game menu
        }
    }

    /// <summary>
    /// The game menu: displays the main menu and handles user selection
    /// </summary>
    public static class GameMenu
    {
        public static void ShowMenu()
        {
            bool choosing = true;

            Console.SetWindowSize(100, 30); // set the console app windows size

            while (choosing)
            {
                Console.Clear();

                SetGameMessage(ConsoleColor.Yellow, "======= Welcome to Connect Four Game by Herrera & Leung Co. =======\n");

                SetGameMessage(ConsoleColor.Cyan, "Please select a game mode:\n");

                SetGameMessage(ConsoleColor.Green, "1. Human vs Human mode");
                SetGameMessage(ConsoleColor.Green, "2. Human vs AI mode");
                SetGameMessage(ConsoleColor.Green, "3. Exit the game\n");

                SetGameMessage(ConsoleColor.White, "Select an option (1-3) and hit Enter: ", false, false);

                string choice = Console.ReadLine();
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
                    SetGameMessage(ConsoleColor.Red, "\nOops! That’s not a valid option. Please choose 1, 2, or 3.\n", true, false);
                }
            }
        }

        /// <summary>
        /// To set the message, behavior and color in console
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
        /// <param name="isSleep"></param>
        /// <param name="newLine"></param>
        public static void SetGameMessage(ConsoleColor color, string message, bool isSleep = false, bool newLine = true)
        {
            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ResetColor();

            if (isSleep)
            {
                Thread.Sleep(1500);
            }
        }

        /// <summary>
        ///  Set disk color
        /// </summary>
        /// <param name="disk"></param>
        public static void DisplayMessageWithColor(char disk)
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

    }

    /// <summary>
    /// The game mode: handles the user selection
    /// </summary>
    public static class GameMode
    {
        /// <summary>
        /// Human vs. Human
        /// </summary>
        public static void HumanVsHuman()
        {
            GameMenu.SetGameMessage(ConsoleColor.DarkYellow, "\nYou chose Human vs Human mode — let the battle begin!\n");

            HumanVsHumanHandler game = new HumanVsHumanHandler();
            game.PlayTheGame();
        }

        /// <summary>
        /// Human vs. AI
        /// </summary>
        public static void HumanVsAI()
        {
            GameMenu.SetGameMessage(ConsoleColor.DarkYellow, "\nYou chose Human vs AI mode — let the battle begin!\n");

            HumanVsAIHandler game = new HumanVsAIHandler();
            game.PlayTheGame();
        }

        /// <summary>
        /// Exit the game
        /// </summary>
        public static void Exit()
        {
            Console.WriteLine("\nExiting game... Thank you for playing Connect Four!");
            return;
        }
    }

    //=========================================================
    // 2: GAME MODE
    //=========================================================

    /// <summary>
    /// Game Common controller for Game Mode
    /// </summary>
    public static class GameModeHelper
    {
        public static bool PlayAgain()
        {
            GameMenu.SetGameMessage(ConsoleColor.White, "\nDo you want to play again? (Y/N): ", false, false);

            string input = Console.ReadLine().Trim();

            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                GameMenu.SetGameMessage(ConsoleColor.DarkYellow, "\nThank you for playing Connect Four! Returning to the Game Menu... Please wait.", true, true);
                Console.Clear();
                GameMenu.ShowMenu();
            }
            return false;
        }

        public static void DisplayVictoryMessage(GameBoard gameBoard, bool isPlayerOneTurn, Player player1, Player player2)
        {
            Console.Clear();
            gameBoard.DisplayGameBoard();

            // Display the congratulatory message with player info
            Console.WriteLine($"\nVictory!\n");
            Console.Write("Congratulations, ");

            // Set the winner's disk color
            if (isPlayerOneTurn)
            {
                GameMenu.DisplayMessageWithColor(player1.Disk);
                Console.Write($"{player1.Name} ({player1.Disk})");
            }
            else
            {
                GameMenu.DisplayMessageWithColor(player2.Disk);
                Console.Write($"{player2.Name} ({player2.Disk})");
            }

            Console.ResetColor();

            Console.WriteLine("! You’re the Connect Four Champion!!!\n");

            Console.ResetColor();
        }

        public static void DisplayDrawMessage(GameBoard gameBoard)
        {
            Console.Clear();
            gameBoard.DisplayGameBoard();

            GameMenu.SetGameMessage(ConsoleColor.DarkYellow, "\nIt's a Draw!\n");
            GameMenu.SetGameMessage(ConsoleColor.Yellow, "Great game, and well played to both challengers!");
        }
    }

    /// <summary>
    /// The human vs. human handler: manages a human vs. human game
    /// </summary>
    public class HumanVsHumanHandler : IConnectFour
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
                        GameModeHelper.DisplayVictoryMessage(_gameBoard, _isPlayerOneTurn, _player1, _player2);
                        isPlayOver = true;
                    }
                    // check if the board is full then it's draw
                    else if (_gameBoard.IsDraw())
                    {
                        GameModeHelper.DisplayDrawMessage(_gameBoard);
                        isPlayOver = true;
                    }
                    // play continues
                    else
                    {
                        _isPlayerOneTurn = !_isPlayerOneTurn; // switch to other player
                        _gameBoard.SetPlayerTurn(_isPlayerOneTurn);
                    }
                }
                else
                {
                    GameMenu.SetGameMessage(ConsoleColor.Red, "\nOopppss.. Invalid move. Please try again.", true);
                }
            }

            // ask if players want to play again
            if (GameModeHelper.PlayAgain())
            {
                isPlayOver = false;
                _gameBoard.InitializeBoard();
                _gameBoard.SetPlayerTurn(true);
                PlayTheGame();
            }
        }
    }

    /// <summary>
    /// The human vs. AI handler: manages a human vs. AI game
    /// </summary>
    public class HumanVsAIHandler : IConnectFour
    {
        private HumanPlayer _player1; // 'X'
        private AIPlayer _player2; // 'O'
        private GameBoard _gameBoard;
        private bool _isPlayerOneTurn;
        

        /// <summary>
        /// Constructor: Initialize a new human vs. AI game
        /// </summary>
        public HumanVsAIHandler()
        {
            RegisterPlayers();

            _gameBoard = new GameBoard();
            _gameBoard.InitializeBoard();
            _gameBoard.SetupHumanVsAIPlayers(_player1, _player2);
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

            // Automatic registration for AI
            _player2 = new AIPlayer("AI", 'O');
        }

        /// <summary>
        /// Start of the Game Loop - Human vs. AI Mode
        /// </summary>
        public void PlayTheGame()
        {
            bool isPlayOver = false;

            while (!isPlayOver)
            {
                Console.Clear();// clear the board               
                _gameBoard.DisplayGameBoard();// show the current board


                if (_isPlayerOneTurn) // check if humans turn
                {
                    int column = _gameBoard.GetPlayerMove();// get player move
                    if (_gameBoard.IsValidMove(column))// is valid move
                    {
                        int row = _gameBoard.DropDisk(column);// dropdisk
                        if (_gameBoard.IsLastMoveWin(row, column)) // check if last move win
                        {
                            GameModeHelper.DisplayVictoryMessage(_gameBoard, _isPlayerOneTurn, _player1, _player2);//Display victory message
                            isPlayOver = true;
                        }
                        else if (_gameBoard.IsDraw())// check if draw
                        {
                            GameModeHelper.DisplayDrawMessage(_gameBoard);
                            isPlayOver = true;
                        }
                        else// plays must go on
                        {
                            _isPlayerOneTurn = !_isPlayerOneTurn;
                            _gameBoard.SetPlayerTurn(_isPlayerOneTurn);
                        }
                    }
                    else
                    {
                        GameMenu.SetGameMessage(ConsoleColor.Red, "\nOopppss.. Invalid move. Please try again.", true);
                    }
                }
                else// else AI turns
                {
                    GameMenu.SetGameMessage(ConsoleColor.Yellow, "AI is thinking its next move... hang tight!", true, true);

                    Console.Clear();
                    _gameBoard.DisplayGameBoard();// show the current board

                    int aiColumn = _player2.GetAIMove(_gameBoard);// get AI player move
                    int row = _gameBoard.DropDisk(aiColumn);// drop disk

                    if (_gameBoard.IsLastMoveWin(row, aiColumn))// check if last move win
                    {
                        GameModeHelper.DisplayVictoryMessage(_gameBoard, !_isPlayerOneTurn, _player1, _player2);//Display victory message
                        isPlayOver = true;
                    }
                    else if(_gameBoard.IsDraw())// check if draw
                    {
                        GameModeHelper.DisplayDrawMessage(_gameBoard);
                        isPlayOver = true;
                    }
                    else// plays must go on
                    {
                        _isPlayerOneTurn = !_isPlayerOneTurn;
                        _gameBoard.SetPlayerTurn(_isPlayerOneTurn);
                    }
                }                                                                                              
            }

            // ask if players want to play again
            if (GameModeHelper.PlayAgain())
            {
                isPlayOver = false;
                _gameBoard.InitializeBoard();
                _gameBoard.SetPlayerTurn(true);
                PlayTheGame();
            }
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
        ///  Initialize the parent constructor
        /// </summary>
        /// <param name="name">The human player name</param>
        /// <param name="disk">X or O</param>
        public HumanPlayer(string name, char disk) : base(name, disk)
        {
        }
    }

    /// <summary>
    /// The AI player 
    /// </summary>
    public class AIPlayer : Player
    {
        /// <summary>
        /// Initialize the parent constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="disk"></param>
        public AIPlayer(string name, char disk) : base(name, disk)
        {
        }

        // AI generate its own move - random number between 0 to 6
        public int GetAIMove(GameBoard board)
        {
            Random random = new Random();
            int column;

            do
            {
                column = random.Next(GameBoard.COLUMNS); // 0 to 6, COLUMNS = 7
            } while (!board.IsValidMove(column));

            return column;
        }

    }

    /// <summary>
    /// This is the contract to be implemented classes
    /// Defines what the game must be able to do ...
    /// </summary>
    interface IConnectFour
    {
        void PlayTheGame();
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

        private char[,] _board;
        private bool _isPlayerOneTurn; // sets whose turn it is
        private HumanPlayer _player1; // first player always human
        private Player _player2; // second player can be human or AI, use the parent or common class

        public GameBoard()
        {
            _board = new char[ROWS, COLUMNS]; // 2D array
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
                    _board[row, col] = '#'; // set # each cell
                }
            }
        }

        /// <summary>
        /// Displays the current game board.
        /// </summary>
        public void DisplayGameBoard()
        {
            GameMenu.SetGameMessage(ConsoleColor.Yellow, "======= Welcome to Connect Four Game by Herrera & Leung Co. =======\n");

            GameMenu.SetGameMessage(ConsoleColor.DarkYellow, "\nConnect 4 Game Development Project:\n");

            // loop through each cell with the #
            for (int row = 0; row < ROWS; row++)
            {
                Console.Write("|");

                for (int col = 0; col < COLUMNS; col++)
                {
                    // set the disk color
                    GameMenu.DisplayMessageWithColor(_board[row, col]);

                    Console.Write($" {_board[row, col]} ");

                    // Reset color after each cell
                    Console.ResetColor();
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
        public int GetPlayerMove()
        {
            Player currentPlayer = _isPlayerOneTurn ? _player1 : _player2;
            Console.Write($"\nYour turn, Player {currentPlayer.Name} (");
            GameMenu.DisplayMessageWithColor(currentPlayer.Disk);
            Console.Write(currentPlayer.Disk);
            Console.ResetColor();
            Console.WriteLine(").");

            GameMenu.SetGameMessage(ConsoleColor.Cyan, "Enter a column number (1-7) and press Enter: ", false, false);

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
                if (_board[i, col] == '#') // this is the empty cell
                {
                    // get the current player's disk, player 1 is X, player 2 is O
                    Player currentPlayer = _isPlayerOneTurn ? _player1 : _player2;

                    // place the player's symbol in the empty cell
                    _board[i, col] = currentPlayer.Disk;
                    row = i; // row we placed the disk
                    break;
                }
            }

            return row;
        }

        /// <summary>
        /// To check if the move is valid or within the board game
        /// </summary>
        /// <param name="column"></param>
        /// <returns>return true if within the board game otherwise false.</returns>
        public bool IsValidMove(int column)
        {
            // validte if the col within the board boundaries
            if (column < 0 || column >= COLUMNS)
            {
                return false;
            }

            // check the column is empty
            return _board[0, column] == '#';
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

            // ======================================================
            // HORIZONTAL WINS (4 in a row, from left to right)
            // ======================================================

            int count = 0;

            // Loop in columns from col - 1 to col + 3 = making sure that index stays within board
            for (int i = Math.Max(0, col - 3); i <= Math.Min(col + 3, COLUMNS - 1); i++)
            {
                // check if the current cell at row contains the player disk
                if (_board[row, i] == playerDisk)
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

            // ======================================================
            // VERTICAL WINS (4 in a row, from top to bottom)
            // ======================================================
            count = 0;

            // Loop through from row - 3 to row + 3
            for (int j = Math.Max(0, row - 3); j <= Math.Min(row + 3, ROWS - 1); j++)
            {
                // check the current cell contains the player disk
                if (_board[j, col] == playerDisk)
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

            // ======================================================
            // DIAGONAL WINS (top left to bottom right)
            // ======================================================
            count = 0;
            int startRow = row - Math.Min(row, col); // starting row position
            int startCol = col - Math.Min(row, col); // starting col position

            // Loop diagonal from the starting position, moving down and to the right
            for (int i = 0; i < Math.Min(ROWS - startRow, COLUMNS - startCol); i++)
            {
                // check the current cell contains the player disk
                if (_board[startRow + i, startCol + i] == playerDisk)
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

            // ======================================================
            // DIAGONAL WINS (top right to bottom left)
            // ======================================================
            count = 0;
            startRow = row - Math.Min(row, COLUMNS - 1 - col); // starting row position
            startCol = col + Math.Min(row, COLUMNS - 1 - col); // starting col position

            // Loop diagonal from the starting position, moving down and to the left
            for (int i = 0; i < Math.Min(ROWS - startRow, startCol + 1); i++)
            {
                // check the current cell contains the player disk
                if (_board[startRow + i, startCol - i] == playerDisk)
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

        /// <summary>
        /// To check if the game is draw
        /// </summary>
        /// <returns></returns>
        public bool IsDraw()
        {
            // If any column in top row is empty then the game is not draw
            for (int col = 0; col < COLUMNS; col++)
            {
                if (_board[0, col] == '#')
                {
                    return false;
                }
            }
            return true; // all columns are full
        }

        /// <summary>
        /// To set up the Player object to use the player's name and disk in this class.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public void SetupHumanVsAIPlayers(HumanPlayer player1, AIPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
        }
    }
}
