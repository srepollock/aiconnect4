using System;

namespace aiconnect4
{
    public static class Global
    {
        public const int rowSize = 6;
        public const int colSize = 7;
        public const char X = 'X';
        public const char O = 'O';
        public static int GetInput()
        {
            int col;
            while (!int.TryParse(Console.ReadLine(), out col)) Console.WriteLine("Try again");
            return col;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            GameController gc = new GameController();
            bool playing = true;
            gc.SetupGame();
            while (playing) 
            {
                gc.Run();
                Console.Write("Would you like to play again? (1|yes, 2|no)");
                if (Global.GetInput() == 2) playing = false;
                else gc.ResetGame();
            }
            Console.WriteLine("Player 1 score: " + gc.playerOne.score + 
                " | Player 2 score: " + gc.playerTwo.score);
            Console.WriteLine("Exiting game...");
        }
    }
    public class Node
    {
        public int row {get; set;}
        public int col {get; set;}        
        public char c {get; set;}
        public Node(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.c = ' ';
        }
        public Node(int row, int col, char c)
        {
            this.row = row;
            this.col = col;
            this.c = c;
        }
        public override string ToString()
        {
            return "|" + this.c + "|";
        }
    }
    public class Grid
    {
        public Node[,] gameGrid {get; set;}
        public Grid()
        {
            gameGrid = ReturnResetGrid();
        }
        public Grid(Node[,] grid)
        {
            this.gameGrid = grid;
        }
        public Node[,] ReturnResetGrid()
        {
            Node[,] newGrid = new Node[Global.rowSize, Global.colSize];
            for (int r = 0; r < Global.rowSize; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    newGrid[r, c] = new Node(r, c);
                }
            }
            return newGrid;
        }
        public void ResetGrid()
        {
            this.gameGrid = ReturnResetGrid();
        }
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
        public string toString()
        {
            string gridString = "";
            for (int r = 0; r < this.gameGrid.GetLength(0); r++)
            {
                for (int c = 0; c < this.gameGrid.GetLength(1); c++)
                {
                    gridString += this.gameGrid[r, c].ToString();
                }
                // gridString += "\n -  -  -  -  -  -  - \n";
                gridString += "\n";
            }
            gridString += " 0  1  2  3  4  5  6\n";
            return gridString;
        }
        /// <summary>
        /// Play the column selected by the player or AI
        /// </summary>
        /// <param name="col">Column to play in</param>
        /// <param name="playerFirst">[true = playerOne first] | [false = playerTwo first]</param>
        /// <param name="playerNumber">[1 = playerOne] | [2 = playerTwo]</param>
        public void Play(int c, bool playerFirst, int playerNumber)
        {
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
        }
    }
    public class GameController
    {
        public Grid gameGrid {get; set;}
        public Player playerOne {get; set;}
        public Player playerTwo {get; set;}
        public bool playerFirst {get; set;} // [true = playerOne first] | [flase = playerTwo first]
        public bool vsAI {get; set;}
        public GameController()
        {
            this.gameGrid = new Grid();
            this.playerOne = new Player();
            this.playerTwo = new Player();
            this.playerFirst = false;
            this.vsAI = false;
        }
        public GameController(Grid grid, Player p1, Player p2, bool playerFirst, bool vsAI)
        {
            this.gameGrid = grid;
            this.playerOne = p1;
            this.playerTwo = p2;
            this.playerFirst = playerFirst;
            this.vsAI = vsAI;
        }
        public void SetupGame()
        {
            GetPlayerName(1);
            SetSecondPlayer();
            playerFirst = GetPlayerFirst();
        }
        void GetPlayerName(int playerNumber)
        {
            Console.Write("Please enter your name: ");
            if (playerNumber == 1) this.playerOne.name = Console.ReadLine();
            else this.playerTwo.name = Console.ReadLine();
        }
        void SetSecondPlayer()
        {
            int choice;
            Console.Write("Player vs Player (1) || Player vs AI (2):");
            while (!int.TryParse(Console.ReadLine(), out choice)) Console.WriteLine("Try again");
            if (choice == 1) { GetPlayerName(2); }
            else SetAI();
        }
        void SetAI()
        {
            this.playerTwo = new AI();
            this.playerTwo.name = "Some AI";
        }
        bool GetPlayerFirst()
        {
            int choice;
            Console.Write("Please say who goes first (1, 2)");
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
        public void ResetGame()
        {
            this.gameGrid.ResetGrid();
        }
        public void PlayerMoves()
        {
            if (playerFirst) { this.PlayerOneMove(); this.PlayerTwoMove(); }
            else { this.PlayerTwoMove(); this.PlayerOneMove(); }
        }
        void PlayerOneMove()
        {
            this.gameGrid.Play(playerOne.Move(), this.playerFirst, 1);
            Console.Write(gameGrid.toString());
        }
        void PlayerTwoMove()
        {
            this.gameGrid.Play(playerTwo.Move(), this.playerFirst, 2);
            Console.Write(gameGrid.toString());
        }
    }
    public class Player
    {
        public string name {get; set;}
        public int score {get; set;}
        public Player()
        {
            this.name = "";
            this.score = 0;
        }
        public Player(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
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
                col = Global.GetInput();
                if (col >= Global.colSize || col < 0) Console.WriteLine("Please choose a number in the range of [0-6]");
                else inRange = false;
            }
            return col;
        }
        public void Win()
        {
            this.score++;
        }
    }
    public class AI : Player
    {
        public AI() : base()
        {
            
        }
        public AI(string name, int score) : base(name, score)
        {

        }
        public override int Move()
        {
            int chioce = 0; // TODO: AI Always chooses 0
            return chioce;
        }
    }
    public class MiniMax
    {
        public int max {get; set;}
        public int min {get; set;}
        public MiniMax(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public int miniMax(Node node, int depth, Player maximizingPlayer)
        {
            return 0;
        }
    }
}