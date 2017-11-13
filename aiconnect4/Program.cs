using System;
using System.Collections;

namespace aiconnect4
{
    /// <summary>
    /// Global variables for the entire game. Easy to set
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Row size for the game
        /// </summary>
        public const int rowSize = 6;
        /// <summary>
        /// Column size for the game
        /// </summary>
        public const int colSize = 7;
        /// <summary>
        /// Player 1 character
        /// </summary>
        public const char X = 'X';
        /// <summary>
        /// Player 2 character
        /// </summary>
        public const char O = 'O';
        /// <summary>
        /// Global win
        /// </summary>
        public const int winNum = 4;
        /// <summary>
        /// Get input from user function
        /// </summary>
        /// <returns></returns>
        public static int GetNumberInput()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num)) Console.WriteLine("Try again");
            return num;
        }
    }
    /// <summary>
    /// Main Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">Arguments</param>
        static void Main(string[] args)
        {
            GameController gc = new GameController();
            bool playing = true;
            gc.SetupGame();
            while (playing) // Just checks if you want to run the game again yes||no
            {
                gc.Run();
                Console.Write("Would you like to play again? (1|yes, 2|no)");
                if (Global.GetNumberInput() == 2) playing = false;
                else gc.ResetGame();
            }
            Console.WriteLine("Player 1 score: " + gc.playerOne.score + 
                " | Player 2 score: " + gc.playerTwo.score);
            Console.WriteLine("Exiting game...");
        }
    }
    /// <summary>
    /// Slot class, used in the grid to store values
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// Slot row
        /// </summary>
        /// <returns>Row</returns>
        public int row {get; set;}
        /// <summary>
        /// Slot column
        /// </summary>
        /// <returns>Column</returns>
        public int col {get; set;}   
        /// <summary>
        /// Character the slot holds
        /// </summary>
        /// <returns>X||O</returns>     
        public char c {get; set;}
        /// <summary>
        /// Constructor: character default to ' '.
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        public Slot(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.c = ' ';
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="c">Character</param>
        public Slot(int row, int col, char c)
        {
            this.row = row;
            this.col = col;
            this.c = c;
        }
        /// <summary>
        /// Gets adjacent slots from the game grid.
        /// </summary>
        /// <param name="gameGrid">Current Game Grid</param>
        /// <returns>List of adjacent Slots</returns>
        public ArrayList GetAdjacentSlots(Grid curGrid)
        {
            ArrayList slots = new ArrayList();
            for (int r = this.row - 1; r < this.row + 1; r++)
            {
                if (r < 0 || r >= Global.rowSize) 
                {
                    continue;
                }
                for (int c = this.col - 1; c < this.col + 1; c++)
                {
                    if (!(c < 0 || c >= Global.colSize)) slots.Add(curGrid.gameGrid[r,c]);
                }
            }
            return slots;
        }
        /// <summary>
        /// Prints the slots character
        /// </summary>
        /// <returns>Character as string</returns>
        public override string ToString()
        {
            return this.c.ToString();
        }
    }
    /// <summary>
    /// The World grid object.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Current game grid
        /// </summary>
        /// <returns>2D Slot array</returns>
        public Slot[,] gameGrid {get; set;}
        /// <summary>
        /// Constructor
        /// </summary>
        public Grid()
        {
            gameGrid = ReturnResetGrid();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="grid">New world grid</param>
        /// <param name="row">Row to insert at</param>
        /// <param name="col">Col to insert at</param>
        /// <param name="c">Character to insert</param>
        public Grid(Slot[,] grid, int row, int col, char c)
        {
            this.gameGrid = grid;
            this.gameGrid[row,col].c = c;
        }
        /// <summary>
        /// Returns an empty grid. Called on reset
        /// </summary>
        /// <returns>Empty 2D Slot array</returns>
        public Slot[,] ReturnResetGrid()
        {
            Slot[,] newGrid = new Slot[Global.rowSize, Global.colSize];
            for (int r = 0; r < Global.rowSize; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    newGrid[r, c] = new Slot(r, c);
                }
            }
            return newGrid;
        }
        /// <summary>
        /// Reset the grid to empty.
        /// </summary>
        public void ResetGrid()
        {
            this.gameGrid = ReturnResetGrid();
        }
        /// <summary>
        /// Checks if there is currently a winner
        /// </summary>
        /// <returns>True or false respectively</returns>
        public bool CheckWin()
        {
            // TODO: Check if someone has won
            for (int r = 0; r < Global.rowSize - 3; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    // TODO: Bound checking
                    // if (r + 1 > Global.rowSize ||
                    //     r + 2 > Global.rowSize ||
                    //     r + 3 > Global.rowSize ||
                    //     c + 1 > Global.colSize ||
                    //     c + 2 > Global.colSize ||
                    //     c + 3 > Global.colSize ||
                    //     r - 3 < 0 ||
                    //     c - 3 < 0) break;
                    // Horizontal
                    if (gameGrid[r,c].c == ' ') continue; // Don't check blanks
                    if (gameGrid[r,c].c == gameGrid[r,c+1].c && 
                        gameGrid[r,c].c == gameGrid[r,c+2].c && 
                        gameGrid[r,c].c == gameGrid[r,c+3].c)
                        {
                            if (gameGrid[r,c].c == 'X') Console.WriteLine("Player 1 Wins!");
                            else Console.WriteLine("Player 2 Wins!");
                            Console.WriteLine("Horizontal");
                            return true;
                        }
                    // Vertical
                    if (gameGrid[r,c].c == gameGrid[r+1,c].c &&
                        gameGrid[r,c].c == gameGrid[r+2,c].c &&
                        gameGrid[r,c].c == gameGrid[r+3,c].c)
                        {
                            if (gameGrid[r,c].c == 'X') Console.WriteLine("Player 1 Wins!");
                            else Console.WriteLine("Player 2 Wins!");
                            Console.WriteLine("Vertical");
                            return true;
                        }
                    if (r-1 < 0 || c-1 < 0) break;
                    // Diagonal \
                    if (gameGrid[r,c].c == gameGrid[r-1,c-1].c &&
                        gameGrid[r,c].c == gameGrid[r-2,c-2].c &&
                        gameGrid[r,c].c == gameGrid[r-3,c-3].c)
                        {
                            if (gameGrid[r,c].c == 'X') Console.WriteLine("Player 1 Wins!");
                            else Console.WriteLine("Player 2 Wins!");
                            Console.WriteLine("Diagonal\\");
                            return true;
                        }
                    // Diagonal /
                    if (gameGrid[r,c].c == gameGrid[r+1,c+1].c &&
                        gameGrid[r,c].c == gameGrid[r+2,c+2].c &&
                        gameGrid[r,c].c == gameGrid[r+3,c+3].c)
                        {
                            if (gameGrid[r,c].c == 'X') Console.WriteLine("Player 1 Wins!");
                            else Console.WriteLine("Player 2 Wins!");
                            Console.WriteLine("Horizontal/");
                            return true;
                        }
                }
            }
            return false;
        }
        /// <summary>
        /// Evaluates an array of slots
        /// </summary>
        /// <param name="slots">4 slots</param>
        /// <param name="c">Character to evaluate in the slots</param>
        /// <returns>Value of the slots</returns>
        public int EvaluateSlot(Slot[] slots, char c)
        {
            int pos = 0;
            int neg = 0;
            for (int i = 0; i < Global.winNum; i++)
            {
                if (slots[i].c == c)
                {
                    pos++;
                }
                else
                {
                    if (!(slots[i].c == ' ')) neg++;
                }
            }
            return (pos * pos) - (neg * neg);
        }
        /// <summary>
        /// Prints the grid as a nice table.
        /// </summary>
        /// <returns>Table of the grid</returns>
        public string toString()
        {
            string gridString = "|0|1|2|3|4|5|6|\n|_|_|_|_|_|_|_|\n";
            for (int r = 0; r < this.gameGrid.GetLength(0); r++)
            {
                for (int c = 0; c < this.gameGrid.GetLength(1); c++)
                {
                    gridString += "|" + this.gameGrid[r, c].ToString();
                }
                // gridString += "\n -  -  -  -  -  -  - \n";
                gridString += "|\n";
            }
            gridString += "\n";
            return gridString;
        }
        /// <summary>
        /// Play the column selected by the player or AI
        /// </summary>
        /// <param name="col">Column to play in</param>
        /// <param name="playerFirst">[true = playerOne first] | [false = playerTwo first]</param>
        /// <param name="playerNumber">[1 = playerOne] | [2 = playerTwo]</param>
        public bool Play(int c, bool playerFirst, int playerNumber)
        {
            if (gameGrid[0, c].c != ' ') return false; // Column full
            for (int r = Global.rowSize-1; r >= 0; r--)
            {
                if (gameGrid[r, c].c == ' ')
                {
                    // Redundant. Player can just get X||O as a char to play
                    if (playerNumber == 1)
                    {
                        if (playerFirst) gameGrid[r, c].c = Global.X;
                        else gameGrid[r, c].c = Global.O;
                    } 
                    else 
                    {
                        if (playerFirst) gameGrid[r, c].c = Global.O; 
                        else gameGrid[r, c].c = Global.X;
                    }
                    break;
                }
            }
            return true;
        }
        /// <summary>
        /// This will evaluate the board for the possible winner
        /// </summary>
        /// <returns>[0|draw] [1|playerOne] [2|playerTwo]</returns>
        public int EvaluateBoard() // this is going to be fucking gross
        {

            return 0; // draw
        }
    }
    /// <summary>
    /// Game controller contians all the game data and runners.
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Games current grid
        /// </summary>
        /// <returns>Game grid</returns>
        public Grid gameGrid {get; set;}
        /// <summary>
        /// First player
        /// </summary>
        /// <returns>Player one</returns>
        public Player playerOne {get; set;}
        /// <summary>
        /// Second player
        /// </summary>
        /// <returns>Player two</returns>
        public Player playerTwo {get; set;}
        /// <summary>
        /// First or second player starts
        /// </summary>
        /// <returns>True or false respectively</returns>
        public bool playerFirst {get; set;} // [true = playerOne first] | [flase = playerTwo first]
        /// <summary>
        /// Vs AI
        /// </summary>
        /// <returns>True or false respectively</returns>
        public bool vsAI {get; set;}
        /// <summary>
        /// Constructor
        /// </summary>
        public GameController()
        {
            this.gameGrid = new Grid();
            this.playerOne = new Player();
            this.playerTwo = new Player();
            this.playerFirst = false;
            this.vsAI = false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <param name="p1">Player one</param>
        /// <param name="p2">Player two</param>
        /// <param name="playerFirst">Player first true||false</param>
        /// <param name="vsAI">VS AI true||false</param>
        public GameController(Grid grid, Player p1, Player p2, bool playerFirst, bool vsAI)
        {
            this.gameGrid = grid;
            this.playerOne = p1;
            this.playerTwo = p2;
            this.playerFirst = playerFirst;
            this.vsAI = vsAI;
        }
        /// <summary>
        /// Setup the game
        /// </summary>
        public void SetupGame()
        {
            GetPlayerName(1);
            SetSecondPlayer();
            playerFirst = GetPlayerFirst();
        }
        /// <summary>
        /// Get the players name and set it
        /// </summary>
        /// <param name="playerNumber">Player to set 1||2</param>
        void GetPlayerName(int playerNumber)
        {
            Console.Write("Please enter your name: ");
            if (playerNumber == 1) this.playerOne.name = Console.ReadLine();
            else this.playerTwo.name = Console.ReadLine();
        }
        /// <summary>
        /// Set second player data if playing vs human or AI
        /// </summary>
        void SetSecondPlayer()
        {
            int choice;
            Console.Write("Player vs Player (1) || Player vs AI (2): ");
            while (!int.TryParse(Console.ReadLine(), out choice)) Console.WriteLine("Try again");
            if (choice == 1) { GetPlayerName(2); }
            else SetAI();
        }
        /// <summary>
        /// Setup AI
        /// </summary>
        void SetAI()
        {
            this.playerTwo = new AI();
            this.playerTwo.name = "Some AI";
        }
        /// <summary>
        /// Get who is playing first from user
        /// </summary>
        /// <returns>True||false player going first</returns>
        bool GetPlayerFirst()
        {
            int choice;
            Console.Write("Please say who goes first (1, 2): ");
            while (!int.TryParse(Console.ReadLine(), out choice)) Console.WriteLine("Try again");
            if (choice == 1)
            {
                Console.WriteLine("PlayerOne going first");
                return true;
            }
            else if (choice == 2) Console.WriteLine("PlayerTwo going first");
            else Console.WriteLine("You chose incorreclty. Defaulting to PlayerTwo");
            return false;
        }
        /// <summary>
        /// Start the game and run
        /// </summary>
        public void Run()
        {
            bool win = false;
            Console.Write(gameGrid.toString());
            #region Game Loop
                while (!win)
                {
                    // PlayerMoves(); // Fix & should be separate
                    if (playerFirst) 
                    { 
                        this.PlayerOneMove(); 
                        if (win = gameGrid.CheckWin()) 
                        {
                            this.playerOne.Win();
                            break;
                        }
                        this.PlayerTwoMove(); 
                        if (win = gameGrid.CheckWin()) 
                        {
                            this.playerTwo.Win();
                            break;
                        }
                    }
                    else 
                    { 
                        this.PlayerTwoMove(); 
                        if (win = gameGrid.CheckWin()) 
                        {
                            this.playerTwo.Win();
                            break;
                        }
                        this.PlayerOneMove(); 
                        if (win = gameGrid.CheckWin()) 
                        {
                            this.playerOne.Win();
                            break;
                        }
                    }
                }
            #endregion
        }
        /// <summary>
        /// Reset the game
        /// </summary>
        public void ResetGame()
        {
            this.gameGrid.ResetGrid();
        }
        /// <summary>
        /// Player one's move
        /// </summary>
        void PlayerOneMove()
        {
            while(!this.gameGrid.Play(playerOne.Move(), this.playerFirst, 1))
                Console.WriteLine("Please choose another column");
            Console.Write(gameGrid.toString());
        }
        /// <summary>
        /// Player two's move
        /// </summary>
        void PlayerTwoMove()
        {
            while(!this.gameGrid.Play(playerTwo.Move(), this.playerFirst, 2))
                Console.WriteLine("Please choose another column");
            Console.Write(gameGrid.toString());
        }
    }
    /// <summary>
    /// Player class
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Player name
        /// </summary>
        /// <returns>Player name</returns>
        public string name {get; set;}
        /// <summary>
        /// Player score
        /// </summary>
        /// <returns>Player score</returns>
        public int score {get; set;}
        /// <summary>
        /// Constructor
        /// </summary>
        public Player()
        {
            this.name = "";
            this.score = 0;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Player name</param>
        /// <param name="score">Player score</param>
        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
        /// <summary>
        /// Player's move
        /// </summary>
        /// <returns>Column to play in</returns>
        public virtual int Move()
        {
            // TODO: get the player input and send it to the grid
            Console.WriteLine(this.name + "'s Turn");
            Console.Write("Please select a column [0-6]: ");
            bool inRange = true;
            int col = -1;
            // TODO: Should have a break condition (-1 || q)
            while (inRange)
            {
                col = Global.GetNumberInput();
                if (col >= Global.colSize || col < 0) Console.WriteLine("Please choose a number in the range of [0-6]");
                else inRange = false;
            }
            return col;
        }
        /// <summary>
        /// Add to the player score
        /// </summary>
        public void Win()
        {
            this.score++;
        }
    }
    public class AI : Player
    {
        public Grid aiGameGrid {get; set;}
        public AI() : base()
        {
            this.aiGameGrid = new Grid();
        }
        public AI(string name, int score, Grid grid) : base(name, score)
        {
            this.aiGameGrid = grid;
        }
        public override int Move()
        {
            // return MiniMax();
            return 0;
        }
        public int MiniMax(Slot n, int depth, bool maximizingPlayer)
        {
            return 0;
        }
    }
}