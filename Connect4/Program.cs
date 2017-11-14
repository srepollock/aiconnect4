using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        public bool Win()
        {
            this.score++;
            Console.WriteLine(name + " won!");
            return true;
        }
    }
    public class AI : Player
    {
        public int depth { get; set; } = 0;
        public Board aiBoard { get; set; }
        public AI() : base()
        {

        }
        public AI(string name, int score, char c) : base(name, score, c)
        {

        }
        public override int Move()
        {
            return base.Move();
        }
        public int AIMove()
        {
            int finalMove = 0;
            int bestValue = -int.MaxValue;
            for (int c = 0; c < Global.colSize; c++)
            {
                int minimaxvalue = Minimax(this.aiBoard, this.depth, true);
                if (bestValue != Math.Max(bestValue, minimaxvalue))
                {
                    bestValue = minimaxvalue;
                    finalMove = c; // BUG: This always reuturns the last column
                }
            }
            return finalMove;
        }
        int Minimax(Board node, int depth, bool maximizingPlayer)
        {
            int bestValue = 0;
            if (depth == 0)
                return node.EvaluateBoard(this);
            if (maximizingPlayer)
            {
                bestValue = -int.MaxValue;
                foreach (Board child in node.GetChildren(this.character)) // This is when the computer wants to move
                {
                    int tempV = Minimax(child, depth - 1, false);
                    bestValue = Global.max(bestValue, tempV);
                }
                return bestValue;
            }
            else
            {
                char ch;
                if (this.character == Global.X) ch = Global.O;
                else ch = Global.X;
                bestValue = int.MaxValue;
                foreach (Board child in node.GetChildren(ch)) // This is when the player wants to move
                {
                    int tempV = Minimax(child, depth - 1, true);
                    bestValue = Global.min(bestValue, tempV);
                }
                return bestValue;
            }
        }
    }
    [Serializable]
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
    [Serializable]
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
        public bool CheckWin(Player p)
        {
            for (int r = 0; r < Global.rowSize - 3; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    char player = positions[r, c].value; // Currently checking
                    if (player == ' ') continue; // Empty
                    if (c + 3 < Global.colSize &&
                        player == positions[r, c + 1].value && // look right
                        player == positions[r, c + 2].value &&
                        player == positions[r, c + 3].value)
                    {
                        if (player == p.character) return true;
                        else return false;
                    }
                    if (r + 3 < Global.rowSize)
                    {
                        if (player == positions[r + 1, c].value && // look up
                            player == positions[r + 2, c].value &&
                            player == positions[r + 3, c].value)
                        {
                            if (player == p.character) return true;
                            else return false;
                        }
                        if (c + 3 < Global.colSize &&
                            player == positions[r + 1, c + 1].value && // look up & right
                            player == positions[r + 2, c + 2].value &&
                            player == positions[r + 3, c + 3].value)
                        {
                            if (player == p.character) return true;
                            else return false;
                        }
                        if (c - 3 >= 0 &&
                            player == positions[r + 1, c - 1].value && // look up & left
                            player == positions[r + 2, c - 2].value &&
                            player == positions[r + 3, c - 3].value)
                        {
                            if (player == p.character) return true;
                            else return false;
                        }
                    }
                }
            }
            return false;
        }
        int FindStreak(char ch, int streak)
        {
            int count = 0;
            for (int r = 0; r < Global.rowSize; r++)
            {
                for (int c = 0; c < Global.colSize; c++)
                {
                    if (positions[r, c].value == ch)
                    {
                        count += FindVerticalStreak(r, c, ch, streak);
                        count += FindHorizontalStreak(r, c, ch, streak);
                        count += FindDiagonalStreak(r, c, ch, streak);
                    }
                }
            }
            return count;
        }
        int FindVerticalStreak(int r, int c, char ch, int streak)
        {
            int consecutiveCount = 0;
            if (r + streak - 1 < Global.rowSize)
            {
                for (int i = 0; i < streak; i++)
                {
                    if (positions[r, c].value == positions[r + i, c].value)
                        consecutiveCount++;
                    else break;
                }
            }
            if (consecutiveCount == streak)
                return 1;
            else return 0;
        }
        int FindHorizontalStreak(int r, int c, char ch, int streak)
        {
            int consecutiveCount = 0;
            if (c + streak - 1 < Global.colSize)
            {
                for (int i = 0; i < streak; i++)
                {
                    if (positions[r, c].value == positions[r, c + i].value)
                        consecutiveCount++;
                    else break;
                }
            }
            if (consecutiveCount == streak)
                return 1;
            else return 0;
        }
        int FindDiagonalStreak(int r, int c, char ch, int streak)
        {
            int total = 0;
            int consecutiveCount = 0;
            if (r + streak - 1 < Global.rowSize && c + streak - 1 < Global.colSize)
            {
                for (int i = 0; i < streak; i++)
                {
                    if (positions[r, c].value == positions[r + i, c + i].value)
                        consecutiveCount++;
                    else break;
                }
            }
            if (consecutiveCount == streak)
                total++;
            consecutiveCount = 0;
            if (r - streak - 1 >= 0 && c + streak - 1 < Global.colSize)
            {
                for (int i = 0; i < streak; i++)
                {
                    if (positions[r, c].value == positions[r - i, c + i].value)
                        consecutiveCount++;
                    else break;
                }
            }
            if (consecutiveCount == streak)
                total++;
            return total;
        }
        public int EvaluateBoard(Player p)
        {
            char oppChar;
            if (p.character == Global.X) oppChar = Global.O;
            else oppChar = Global.X;
            int aiFours = FindStreak(p.character, 4);
            int aiThrees = FindStreak(p.character, 3);
            int aiTwos = FindStreak(p.character, 2);
            int oppFours = FindStreak(oppChar, 4);
            int oppThrees = FindStreak(oppChar, 3);
            int oppTwos = FindStreak(oppChar, 2);
            if (oppFours > 0)
                return int.MinValue; // Lost
            else
                return (aiFours * 100000 + aiThrees * 100 + aiTwos * 10) - (oppThrees * 100 + oppTwos * 10);
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
        public bool Play(char ch, int row, int col)
        {
            if (this.positions[row, col].value == ' ')
            {
                this.positions[row, col].value = ch;
                return true;
            }
            return false;
        }
        public ArrayList GetChildren(char ch)
        {
            ArrayList children = new ArrayList();
            Board tempBoard = null;
            for (int col = 0; col < Global.colSize; col++)
            {
                tempBoard = null;
                tempBoard = DeepClone<Board>(this);
                if (tempBoard.Play(ch, col)) children.Add(tempBoard);
            }
            return children;
        }
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
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
        public AI playerTwo { get; set; } = new AI();
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
                if (board.CheckWin(playerOne))
                {
                    win = true;
                    playerOne.Win();
                    break;
                }
                PlayerTwoMove();
                if (board.CheckWin(playerTwo))
                {
                    win = true;
                    playerTwo.Win();
                    break;
                }
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
            this.playerTwo.depth = Global.GetNumberInput(); // LET'S GO!!..THIS IS GROSS..
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
                // TODO: AI Movement
                // Set AI Board
                this.playerTwo.aiBoard = this.board;
                // Move
                this.board.Play(this.playerTwo.character, this.playerTwo.AIMove());
                // DEBUG: Broken and not working here
            }
            else
            {
                while (!this.board.Play(this.playerTwo.character, this.playerTwo.Move()))
                    Console.WriteLine("Please choose another column");
            }
            PrintBoard();
        }
        void PrintBoard()
        {
            Console.Write(this.board.ToString());
        }
    }
}