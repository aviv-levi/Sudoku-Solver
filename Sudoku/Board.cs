using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Sudoku
{
    /// <summary>
    /// This class represnts the suduko board.
    /// </summary>
    public class Board
    {

        private int[,] board; // board matrix

        public Board(string boardinfo)
        {
            this.BuildBoard(boardinfo); // build the board to matrix from user input
        }



        /// <summary>
        /// Build the board matrix from string format.
        /// </summary>
        /// <param name="boardinfo">board in string format</param>
        private void BuildBoard(string boardinfo)
        {
            //the board side length is sqrt of the length because board is square.
            int N = (int)Math.Sqrt(boardinfo.Length);
            this.board = new int[N, N];
            int p = 0; //index of char in boardinfo
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++,p++)
                {
                    board[i, j] = boardinfo[p] - '0'; //convert ascii format to the number he represnt (above nine its just his relative distance from zero)
                }
            }

        }

        ////------------------------------------------------------------- GETTERS --------------------------------------------------------

        /// <summary>
        /// Getter to the number of rows in the board.
        /// </summary>
        /// <returns>The number of rows in the board.</returns>
        public int GetRows()
        {
            return this.board.GetLength(0);
        }
        /// <summary>
        /// Getter to the number of column in the board.
        /// </summary>
        /// <returns>The number of column in the board.</returns>
        public int GetColumns()
        {
            return this.board.GetLength(1);
        }
        /// <summary>
        /// Getter to the number of boxes in the board.
        /// </summary>
        /// <returns>The number of boxes in the board.</returns>
        public int GetNumOfBoxes()
        {
            //the number of boxes equals to the number of row,columns
            return this.board.GetLength(0);
        }


        /// <summary>
        /// Get and set board elements.
        /// </summary>
        /// <param name="i">row of element.</param>
        /// <param name="j">column of element.</param>
        /// <returns>board element in a specific row , col</returns>
        public int this[int i,int j]
        {
            get
            {
                return this.board[i, j];
            }
            set
            {
                this.board[i, j]=value;
            }
        }




        /// <summary>
        /// Checking string format of board validation.
        /// </summary>
        /// <param name="boardinfo"></param>
        /// <returns>true if board info is a full valid suduko (result of solver is right)</returns>
        public static bool CheckResult(string boardinfo)
        {
            if (boardinfo=="-1")//unsolvble code
            {
                return false;
            }

            //we got soultion , we need to check its a valid suduko full board

            //we have to check 2 terms:
            //1.numbers dosent repeate in same line, column, box
            //2.board is full

            int rowColLength = (int)Math.Sqrt(boardinfo.Length);// length of row/col equals to sqrt of input length,because the board is square 
            Board board = new Board(boardinfo);//build board to check result
            //the valid range by the rules of suduko is the count of numbers you can put one time in every row, col, box.
            //so the valid range is the length of row and col and box
            int validrange = rowColLength;

            //check lines
            for (int i = 0; i < rowColLength; i++)
            {
                bool[] numbers_exists = new bool[validrange]; //array that represent numbers in a single line, numbers are the index and every element is if number is exists 
                for (int j = 0; j < rowColLength; j++)
                {
                    //run over the line
                    if (board[i, j] != 0)
                    {
                        numbers_exists[board[i, j] - 1] = true; //if place isnt empty, enter to the existArray
                    }
                    else
                    {
                        return false;
                    }

                }
                if (!numbers_exists.All<bool>(num_exists => num_exists = true)) // if some number not exist , board is not valid
                {
                    return false;
                }
            }
            //check columns
            for (int i = 0; i < rowColLength; i++)
            {
                bool[] numbers_exists = new bool[validrange];//array that represent numbers in a single column, numbers are the index and every is if number is exists 
                for (int j = 0; j < rowColLength; j++)
                {
                    if (board[j, i] != 0)
                    {
                        numbers_exists[board[j, i] - 1] = true;//if place isnt empty, enter to the existArray
                    }
                    else
                    {
                        return false;
                    }

                }
                if (!numbers_exists.All<bool>(num_exists => num_exists = true))// if some number not exist , board is not valid
                {
                    return false;
                }
            }
            //check boxes
            int sideBoxLength = (int)Math.Sqrt(rowColLength);
            for (int boxIndex = 0; boxIndex < rowColLength; boxIndex++)
            {
                int startX = boxIndex % sideBoxLength;
                int startY = boxIndex / sideBoxLength;
                for (int i = startY; i < startY + sideBoxLength; i++)
                {
                    bool[] numbers_exists = new bool[validrange];//array that represent numbers in a single box, numbers are the index and every element is if number is exists 
                    for (int j = startX; j < startX + sideBoxLength; j++)
                    {
                        if (board[i, j] != 0)
                        {
                            numbers_exists[board[i, j] - 1] = true;//if place isnt empty, enter to the existArray
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (!numbers_exists.All<bool>(num_exists => num_exists = true))// if some number not exist , board is not valid
                    {
                        return false;
                    }
                }
            }
            return true; // passed all checks, board is valid
        }



        /// <summary>
        /// Prints the board.
        /// </summary>
        /// <param name="color">optional text color, white by defult</param>
        public void Print(ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color; //sets text color
            int i = 0, j = 0,length = board.GetLength(0);
            Console.WriteLine();
            Console.Write("      ");
            for (j = 0; j < this.board.GetLength(0); j++)
            {
                if (j / 10 != 0)
                    Console.Write("  " + j + "  ");
                else
                    Console.Write("  " + j + "   ");

            }
            Console.Write("\n      ");
            for (j = 0; j < length; j++)
            {
                for (j = 1; j <= length; j++)
                    Console.Write("_____ ");

                for (i = 0; i < length; i++)
                {
                    Console.Write("\n     |");
                    for (j = 0; j < length; j++)
                    {
                        Console.Write("     |");
                    }
                    if (i / 10 != 0)
                        Console.Write("\n  " + i + " |");
                    else
                        Console.Write("\n  " + i + "  |");
                    for (j = 0; j < length; j++)
                    {
                        if (board[i, j] / 10 != 0)
                            Console.Write("  " + board[i, j] + " ");
                        else
                            Console.Write("  " + board[i, j] + "  ");
                        Console.Write("|");
                    }
                    Console.Write("\n     |");
                    for (j = 0; j < length; j++)
                    {
                        Console.Write("_____|");
                    }
                }
                Console.WriteLine();

            }
        }
        /// <summary>
        /// Convert board to string format. 
        /// </summary>
        /// <returns>string format of the board</returns>
        public override string ToString()
        {
            string board_text="";
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    board_text+=((char)(this.board[i, j] + '0')).ToString(); //add the ascii value of the element to 'board_text'
                }
            }
            return board_text;
        }
    }
}
