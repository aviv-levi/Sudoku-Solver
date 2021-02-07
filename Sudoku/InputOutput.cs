using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    /// <summary>
    /// The I/O class.
    /// Here responsibility is to handle the I/O from user.
    /// </summary>
    /// /// <remarks>
    /// <para>This class can read the board info from console window or from text file.</para>
    /// <para>Text file can be read by input his path to console , or by choose from dialog window.</para>
    /// </remarks>
    public static class InputOutput
    {
        //------------------------------------------ INPUT METHODS -----------------------------------



        /// <summary>
        /// Reads board info from console window.
        /// </summary>
        /// <returns>The inputted board info.</returns>
        public static string ReadFromConsole()
        {
            Print("Enter board info: ");
            string boardInfo = Console.ReadLine(); // reads user input to 'boardInfo'
            return boardInfo;
        }



        /// <summary>
        /// Reads board info from text file by his file path.
        /// </summary>
        /// <param name="filepath">filepath parameter, changes in the method</param>
        /// <exception cref="IOException">failed read file content.</exception>
        /// <exception cref="ArgumentException">inputted file path is not valid.</exception>
        /// <returns>The inputted board info.</returns>
        public static string ReadFromFilePath(out string filepath)
        {
            filepath = "-4"; //by default file doesn't exists, changes when all checks are good

            Print("Enter file path: ");
            string input_path = Console.ReadLine(); //reads file path from user
            if (!File.Exists(input_path))
            {
                return "-4";// if file dialog cant show up, return -3 as 'file-fail' code
            }
            filepath = input_path;
            string input = File.ReadAllText(input_path); //reads file content to 'input'
            return input;
        }




        /// <summary>
        ///  Reads board info from text file by choose from file dialog.
        /// </summary>
        /// <exception cref="IOException">failed to read file content.</exception>
        /// <returns>The inputted board info.</returns>
        public static string ReadFromFile(out string filepath)
        {
            filepath = "-4";//by default file doesn't exists, changes when all checks are good
            OpenFileDialog dialog = new OpenFileDialog 
            {
                Multiselect = false, // cant pick more then one text file
                Title = "Open Board Text", // dialog title
                Filter = "Text|*.txt|All|*.*", //can pick only text files
            };

            DialogResult result = dialog.ShowDialog(); // shows file dialog , get if the operation succeed
            if (result == DialogResult.OK) // if the operation succeed  
            {
                filepath = dialog.FileName; //gets file path from the dialog property called 'FileName'
                string input = File.ReadAllText(filepath); //reads file content to 'input'
                return input;
            }
            else //if the operation failed 
            {
                return "-3"; // if file dialog cant show up, return -3 as 'file-fail' code
            }
        }





        /// <summary>
        /// <para>Prints to user the options for reading board.</para>
        /// <para>After it prints , it gets user choich.</para>
        /// </summary>
        /// <returns>User input</returns>
        public static string ShowOptionsAndGetUserChoice()
        {
            // prints options
            Print("Which way you want to enter the board?\n");
            Print("1.Enter it in console");
            Print("2.Choose text file");
            Print("3.Enter file path");
            Print("4.Exit\n");
            //returns user input
            return Console.ReadLine();
        }


       

        //-------------------------------------------------OUTPUT METHODS----------------------------------------





        /// <summary>
        /// Prints text with color.
        /// </summary>
        /// <param name="text">Text to print.</param>
        /// <param name="color">Text color, white by default.</param>
        public static void Print(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color; // set text color
            Console.WriteLine(text); // print text
        }




        /// <summary>
        /// Prints colored title to console window center
        /// </summary>
        public static void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; // sets text color

            string openningTitle = "WELCOME TO MY SUDUKO SOLVER";
            Console.SetCursorPosition((Console.WindowWidth - openningTitle.Length) / 2, Console.CursorTop);// center the pointer
            Console.WriteLine(openningTitle + "\n"); // prints the title

            string madeby = "made by aviv levi";
            Console.SetCursorPosition((Console.WindowWidth - madeby.Length) / 2, Console.CursorTop);
            Console.WriteLine(madeby + "\n");
        }




        /// <summary>
        /// Saving text to text file.
        /// </summary>
        /// <param name="filePath">the file path where you want to save.</param>
        /// <param name="text">text to saved.</param>
        /// <exception cref="IOException">failed write file.</exception>
        /// <returns>true if saved successfully, false if not.</returns>
        public static bool SaveFile(string filePath, string text)
        {
            if (File.Exists(filePath)) // if the file exists in memory
            {
                return false;
            }
            File.WriteAllText(filePath, text); // saving text
            return true;
        }




        // --------------------------------------- CHECK INPUT VALIDATION ---------------------------------------------


        /// <summary>
        /// <para>Check if user input can suited to suduko board by his content.</para>
        /// </summary>
        /// <param name="input">user input</param>
        /// <returns>true if input content is valid. false if not.</returns>
        private static bool CheckInputContent(string input)
        {

            //we have to check 3 terms:
            //1.numbers dosent repeate in same line,column,box
            //2.all numbers in the valid range
            //3.board is not full

            bool emptyFlag = false; // flag if there is an empty place, if there isnt empty place,the board is full and the board is invalid
            int rowColLength = (int)Math.Sqrt(input.Length);// length of row/col equals to sqrt of input length,because the board is square 
            int[,] board = new int[rowColLength, rowColLength];//matrix to represnt the board
            int t = 0; // index to char in the input

            //the valid range by the rules of suduko is the count of numbers you can put one time in every row,col,box.
            //so the valid range is the length of row and col and box
            int validrange = rowColLength;

            for (int i = 0; i < rowColLength; i++) {
                for (int j = 0; j < rowColLength; j++){
                    board[i, j] = input[t++] - '0'; // enter the real value of the char from his ascii
                    if (board[i, j]<0 || board[i,j]>validrange) //if number isnt in valid range, board is invalid
                    {
                        return false;
                    }
                }
            }

            //check lines
            for (int i = 0; i < rowColLength; i++){
                int[] numbers_appear = new int[validrange]; //array that represent numbers in a single line, numbers are the index and every element is the appearances of this number 
                for (int j = 0; j < rowColLength; j++){ //run over the line
                    if (board[i,j] == 0) { // if empty place,turn-on emptyFlag
                        emptyFlag = true;
                    }
                    else
                    {
                        numbers_appear[board[i, j] - 1]++; //if place isnt empty, enter to the appearArray
                    }
                    
                }
                if (!numbers_appear.All<int>(num_apper => num_apper<=1)) // if some number apper more then once, board is invalid
                {
                    return false;
                }
            }

            //check columns
            for (int i = 0; i < rowColLength; i++)
            {
                int[] numbers_appear = new int[validrange];//array that represent numbers in a single column, numbers are the index and every element is the appearances of this number
                for (int j = 0; j < rowColLength; j++)
                {
                    if (board[j, i] == 0)// if empty place,turn-on emptyFlag
                    {
                        emptyFlag = true;
                    }
                    else
                    {
                        numbers_appear[board[j, i] - 1]++;//if place isnt empty, enter to the appearArray
                    }

                }
                if (!numbers_appear.All<int>(num_apper => num_apper <= 1))// if some number apper more then once, board is invalid
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
                for (int i = startY; i < startY+sideBoxLength; i++) {
                    int[] numbers_appear = new int[validrange];//array that represent numbers in a single box, numbers are the index and every element is the appearances of this number
                    for (int j = startX; j < startX+sideBoxLength; j++) {
                        if (board[i,j]==0)// if empty place,turn-on emptyFlag
                        {
                            emptyFlag = true;
                        }
                        else
                        {
                            numbers_appear[board[i, j] - 1]++;//if place isnt empty, enter to the appearArray
                        }
                    }
                    if (!numbers_appear.All<int>(num_apper => num_apper <= 1))// if some number apper more then once, board is invalid
                    {
                        return false;
                    }
                }
            }
            return emptyFlag;
        }           
        


        /// <summary>
        /// Check if user input can suited to suduko board by his length.
        /// </summary>
        /// <param name="boardinfo">user input</param>
        /// <returns>true if input length is valid, false if not.</returns>
        private static bool CheckInputLength(string boardinfo)
        {
            if (boardinfo.Length==0) // if input is empty, cant build board
            {
                return false;
            }
            //a valid suduko board is a square fill with little equal squares
            //so we can check if the 4th square root of the input length is integer
            double lenOfLittleSquare = Math.Sqrt(Math.Sqrt(boardinfo.Length));
            return lenOfLittleSquare % 1 == 0;
        }



        /// <summary>
        /// Input validation method.
        /// </summary>
        /// <param name="boardinfo">boardinfo that user entered.</param>
        /// <returns>true if board is valid, false if not.</returns>
        public static bool BoardIsValid(string boardinfo)
        {
            if (CheckInputLength(boardinfo))// check if user input can be valid suduko by his length
            {
                if (CheckInputContent(boardinfo))// check if user input can be valid suduko by his content
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        
    }
}
