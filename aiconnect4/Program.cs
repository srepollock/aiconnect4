using System;

namespace aiconnect4
{
    public static class Global
    {
        public const int rowSize = 6;
        public const int colSize = 7;
        public const char X = 'X';
        public const char O = 'O';
        public const int DEFAULT_DEPTH = 4;
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
    public class Slot
    {
        public int row {get; set;}
        public int col {get; set;}        
        public char c {get; set;}
        public Slot(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.c = ' ';
        }
        public Slot(int row, int col, char c)
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
        public Slot[,] gameGrid {get; set;}
        public Grid()
        {
            gameGrid = ReturnResetGrid();
        }
        public Grid(Slot[,] grid)
        {
            this.gameGrid = grid;
        }
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
        void PlayerOneMove()
        {
            while(!this.gameGrid.Play(playerOne.Move(), this.playerFirst, 1))
                Console.WriteLine("Please choose another column");
            Console.Write(gameGrid.toString());
        }
        void PlayerTwoMove()
        {
            while(!this.gameGrid.Play(playerTwo.Move(), this.playerFirst, 2))
                Console.WriteLine("Please choose another column");
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
        Slot[,] aiGameGrid {get; set;}
        public AI() : base()
        {
            
        }
        public AI(string name, int score) : base(name, score)
        {

        }
        public override int Move()
        {
            /*
                if (this.char = X (first))
                    max()
                else // second
                    min()
            */
            
            return 0;
        }
        
    }
    public class MiniMax
    {
        public int maxDepth;
        public MiniMax()
        {
            this.maxDepth = Global.DEFAULT_DEPTH;
        }
        public MiniMax(int depth)
        {
            this.maxDepth = depth;
        }
        // a-b pruning
        /*
            a: 
                if the current max is greater than the successors min value, 
                dont explore that min tree anymore
        */
        /*
            b:
                if the current minimum is less than the successors maximum 
                value, dont look down that max tree anymore
        */
        public Move MaxMove(GamePosition game)
        {
            bool gameOver = false;
            int bestMove = 0;
            if (gameOver)
            {
                return EvaluateGameState(game);
            }
            else
            {
                Move[] moves = GenerateMoves(game); // list of cols?
                foreach (Move m in moves)
                {
                    m = MinMove(ApplyMove(game)); // test the move on the board and check
                    if (Value(m) > Value(bestMove))
                    {
                        bestMove = m;
                    }
                }
                return Move(bestMove);
            }
        }
        public Move MinMove(GamePosition game) // is this the board?
        {
            bool gameOver = false;
            int bestMove = 0;
            if (gameOver)
            {
                return EvaluateGameState(game);
            }
            else
            {
                Move[] moves = GenerateMoves(game); // list of cols?
                foreach (Move m in moves)
                {
                    m = MaxMove(ApplyMove(game)); // test the move on the board and check
                    if (Value(m) > Value(bestMove))
                    {
                        bestMove = m;
                    }
                }
                return bestMove;
            }
        }
        public void GenerateMoves() // based on what?
        {
            
        }
        public int EvaluateGameState(GamePosition game)
        {
            return 0;
        }
        int Value(int col)
        {
            // value of col?
            return 0;
        }
        GamePosition ApplyMove(GamePosition gp)
        {
            return new GamePosition();
        }
    }
    public class Move
    {
        int value;
        public Move(int value)
        {
            this.value = value;
        }
    }
    public class GamePosition
    {

    }
}