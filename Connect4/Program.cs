using System;
using System.Collections;
using System.Collections.Generic;

namespace Connect4
{
    public static class Global
    {
        public const int rowSize = 6;
        public const int colSize = 7;
        public const char X = 'X'; // X is always first
        public const char O = 'O';
        public const int winNum = 4;
        public static int max(int a, int b)
        {
            if (a > b) return a;
            return b;
        }
        public static bool maxBool(ref int a, int b)
        {
            if (a > b)
            {
                return true;
            }
            a = b;
            return false;
        }
        public static int min(int a, int b)
        {
            if (a < b) return a;
            return b;
        }
        public static int GetNumberInput()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num)) Console.WriteLine("Try again");
            return num;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            GameController gc = new GameController();
            bool playing = true;
            gc.SetupGame();
            while (playing) // Just checks if you want to run the game again yes||no
            {
                gc.Run();
                Console.Write("Would you like to play again? [1|yes, 2|no]: ");
                if (Global.GetNumberInput() == 2) playing = false;
                else gc.Reset();
            }
            Console.WriteLine("Player 1 score: " + gc.playerOne.score +
                " | Player 2 score: " + gc.playerTwo.score);
            Console.WriteLine("Exiting game...");
        }
    }
    public class Player
    {
        public string name { get; set; }
        public int score { get; set; }
        public char character { get; set; }
        public Player()
        {
            this.name = "";
            this.score = 0;
            this.character = '\0';
        }
        public Player(string name, int score, char c)
        {
            this.name = name;
            this.score = score;
            this.character = c;
        }
        public void SetName()
        {
            this.name = Console.ReadLine();
        }
        public virtual int Move()
        {
            Console.WriteLine(this.name + "'s Turn");
            Console.Write("Please select a column [0-6]: ");
            bool inRange = true;
            int col = -1;
            // REVIEW: Should have a break condition (-1 || q)
            while (inRange)
            {
                col = Global.GetNumberInput();
                if (col >= Global.colSize || col < 0) Console.WriteLine("Please choose a number in the range of [0-6]");
                else inRange = false;
            }
            return col;
        }
        public virtual bool Win()
        {
            this.score++;
            return true;
        }
    }
    public class AI : Player
    {
        public int depth { get; set; } = 0;
        public AI() : base()
        {

        }
        public AI(string name, int score, char c) : base(name, score, c)
        {

        }
        public override int Move()
        {
            // TODO: Minimax here
            return base.Move(); // Not like this
        }
        //int Minimax(Board node, int depth, bool maximizingPlayer)
        //{
        //    int bestValue = 0;
        //    if (depth == 0)
        //        return node.EvaluateBoard(this);
        //    if (maximizingPlayer)
        //    {
        //        bestValue = -int.MaxValue;
        //        foreach (Board child in node.GetChildren()) // TODO: Does AI need to know both?
        //        {
        //            int tempV = Minimax(child, depth - 1, false);
        //            bestValue = Global.max(bestValue, tempV);
        //        }
        //        return bestValue;
        //    }
        //    else
        //    {
        //        bestValue = int.MaxValue;
        //        foreach (Board child in node.GetChildren())
        //        {
        //            int tempV = Minimax(child, depth - 1, true);
        //            bestValue = Global.min(bestValue, tempV);
        //        }
        //        return bestValue;
        //    }
        //}
    }
    public class Position
    {
        public char value { get; set; } = ' ';
        public int row { get; set; } = 0;
        public int column { get; set; } = 0;
        public Position()
        {

        }
        public Position(int r, int c)
        {
            this.row = r;
            this.column = c;
        }
        public Position(char ch, int r, int c)
        {
            this.value = ch;
            this.row = r;
            this.column = c;
        }
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
    public class Board
    {
        public Position[,] positions { get; set; }
        public Board()
        {
            this.Reset(); // Set's to empty board
        }
        public Board(Board b)
        {
            this.positions = b.positions;
        }
        public Board(Position[,] positions)
        {
            this.positions = positions;
        }
        public bool CheckWin(Player p) // TODO: Refactor to check on this player's character for their win
        {
            for (int r = 0; r < Global.rowSize - 3; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    if (positions[r, c].value == ' ') continue; // Don't check blanks ????
                    if (positions[r, c].value == positions[r, c + 1].value &&
                        positions[r, c].value == positions[r, c + 2].value &&
                        positions[r, c].value == positions[r, c + 3].value)
                    {
                        if (positions[r, c].value == 'X') Console.WriteLine("Player 1 Wins!");
                        else Console.WriteLine("Player 2 Wins!");
                        Console.WriteLine("Horizontal");
                        return true;
                    }
                    // Vertical
                    if (positions[r, c].value == positions[r + 1, c].value &&
                        positions[r, c].value == positions[r + 2, c].value &&
                        positions[r, c].value == positions[r + 3, c].value)
                    {
                        if (positions[r, c].value == 'X') Console.WriteLine("Player 1 Wins!");
                        else Console.WriteLine("Player 2 Wins!");
                        Console.WriteLine("Vertical");
                        return true;
                    }
                    if (r - 1 < 0 || c - 1 < 0) break;
                    // Diagonal \
                    if (positions[r, c].value == positions[r - 1, c - 1].value &&
                        positions[r, c].value == positions[r - 2, c - 2].value &&
                        positions[r, c].value == positions[r - 3, c - 3].value)
                    {
                        if (positions[r, c].value == 'X') Console.WriteLine("Player 1 Wins!");
                        else Console.WriteLine("Player 2 Wins!");
                        Console.WriteLine("Diagonal\\");
                        return true;
                    }
                    // Diagonal /
                    if (positions[r, c].value == positions[r + 1, c + 1].value &&
                        positions[r, c].value == positions[r + 2, c + 2].value &&
                        positions[r, c].value == positions[r + 3, c + 3].value)
                    {
                        if (positions[r, c].value == 'X') Console.WriteLine("Player 1 Wins!");
                        else Console.WriteLine("Player 2 Wins!");
                        Console.WriteLine("Horizontal/");
                        return true;
                    }
                }
            }
            return false;
        }
        public int EvaluateBoard(Player p)
        {
            // TODO: Evaluate the board for the player with p.character
            return 0;
        }
        /// <summary>
        /// Plays int the current board the character at the column
        /// </summary>
        /// <param name="ch">Character to play</param>
        /// <param name="col">Column to try</param>
        /// <returns>[ValidMove|True] [InavlidMove|False]</returns>
        public bool Play(char ch, int col)
        {
            for (int row = Global.rowSize - 1; row >= 0; row--)
            {
                if (this.positions[row, col].value == ' ') // Can only enter empty position
                {
                    this.positions[row, col].value = ch;
                    return true; // Valid position
                }
            }
            return false; // Invalid (column full)
        }
        public ArrayList GetChildren(char ch)
        {
            ArrayList children = new ArrayList();
            for (int col = 0; col < Global.colSize; col++)
            {
                Board tempBoard = new Board(this);
                if (tempBoard.Play(ch, col)) children.Add(tempBoard);
            }
            return children;
        }
        public void Reset()
        {
            this.positions = new Position[Global.rowSize, Global.colSize];
            for (int r = 0; r < Global.rowSize; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    this.positions[r, c] = new Position(r, c);
                }
            }
        }
        public override string ToString()
        {
            string boardString = "|0|1|2|3|4|5|6|\n|_|_|_|_|_|_|_|\n";
            for (int r = 0; r < Global.rowSize; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    boardString += "|" + this.positions[r, c].ToString();
                }
                // gridString += "\n -  -  -  -  -  -  - \n";
                boardString += "|\n";
            }
            boardString += "\n";
            return boardString;
        }
    }
    public class GameController
    {
        public Board board { get; set; } = new Board();
        public Player playerOne { get; set; } = new Player();
        public Player playerTwo { get; set; } = new Player();
        public bool vsAi { get; set; } = false;
        public GameController()
        {

        }
        public void Run()
        {
            bool win = false;
            PrintBoard();
            if (playerOne.character != Global.X) PlayerTwoMove();
            while (!win)
            {
                PlayerOneMove();
                if (win = board.CheckWin(playerOne)) break;
                PlayerTwoMove();
                if (win = board.CheckWin(playerTwo)) break;
            }
        }
        public void Reset()
        {
            this.board.Reset();
        }
        #region Game Setup
        public bool SetupGame()
        {
            GetPlayerName(this.playerOne);
            SetSecondPlayer();
            WhoIsFirst();
            return true; // Success
        }
        void GetPlayerName(Player p)
        {
            Console.Write("Please enter your name: ");
            p.SetName();
        }
        void SetSecondPlayer()
        {
            Console.Write("Player vs Player (1) || Player vs AI (2): ");
            if (Global.GetNumberInput() == 1) GetPlayerName(this.playerTwo);
            else SetAI();
        }
        void SetAI()
        {
            this.vsAi = true;
            playerTwo.name = "AI";
            Console.Write("AI Depth: ");
            //this.playerTwo.depth = Global.GetNumberInput(); // TODO: Fix this
        }
        void WhoIsFirst()
        {
            Console.Write("Please say who goes first (1, 2): ");
            int choice = Global.GetNumberInput();
            if (choice == 1)
            {
                Console.WriteLine("PlayerOne going first");
                PlayerOneFirst();
            }
            else if (choice == 2)
            {
                Console.WriteLine("PlayerTwo going first");
                PlayerTwoFirst();
            }
            else
            {
                Console.WriteLine("You chose incorreclty. Defaulting to PlayerTwo");
                PlayerTwoFirst();
            }
        }
        void PlayerOneFirst()
        {
            this.playerOne.character = Global.X;
            this.playerTwo.character = Global.O;
        }
        void PlayerTwoFirst()
        {
            this.playerOne.character = Global.O;
            this.playerTwo.character = Global.X;
        }
        #endregion
        void PlayerOneMove()
        {
            while (!this.board.Play(this.playerOne.character, this.playerOne.Move()))
                Console.WriteLine("Please choose another column");
            PrintBoard();
        }
        void PlayerTwoMove()
        {
            if (vsAi)
            {
                // TODO: Ai movement
            }
            else
            {
                while (!this.board.Play(this.playerTwo.character, this.playerTwo.Move()))
                    Console.WriteLine("Please choose another column");
                PrintBoard();
            }
        }
        void PrintBoard()
        {
            Console.Write(this.board.ToString());
        }
    }
}