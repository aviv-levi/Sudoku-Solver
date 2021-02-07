using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


//// ░██████╗██╗░░░██╗██████╗░░█████╗░██╗░░██╗██╗░░░██╗  ░██████╗░█████╗░██╗░░░░░██╗░░░██╗███████╗██████╗░
//// ██╔════╝██║░░░██║██╔══██╗██╔══██╗██║░██╔╝██║░░░██║  ██╔════╝██╔══██╗██║░░░░░██║░░░██║██╔════╝██╔══██╗
//// ╚█████╗░██║░░░██║██║░░██║██║░░██║█████═╝░██║░░░██║  ╚█████╗░██║░░██║██║░░░░░╚██╗░██╔╝█████╗░░██████╔╝
//// ░╚═══██╗██║░░░██║██║░░██║██║░░██║██╔═██╗░██║░░░██║  ░╚═══██╗██║░░██║██║░░░░░░╚████╔╝░██╔══╝░░██╔══██╗
//// ██████╔╝╚██████╔╝██████╔╝╚█████╔╝██║░╚██╗╚██████╔╝  ██████╔╝╚█████╔╝███████╗░░╚██╔╝░░███████╗██║░░██║
//// ╚═════╝░░╚═════╝░╚═════╝░░╚════╝░╚═╝░░╚═╝░╚═════╝░  ╚═════╝░░╚════╝░╚══════╝░░░╚═╝░░░╚══════╝╚═╝░░╚═╝


//// --------------------------------------   🅱🆈 🅰🆅🅸🆅 🅻🅴🆅🅸   -------------------------------------


namespace Sudoku
{
    /// <summary>
    /// Main class of the program.
    /// </summary>
    class Program
    {
        enum EnterMethod { FromConsole,FromFilePath,FromFileDialog }; // enum that represnt how user enter the data
        [STAThread] //attribute which windows requires for using microsoft components (file dialog)
        static void Main(string[] args)
        {
            //// ------------------------------------ SET HOW MANY CONSOLE.READLINE CAN READ ------------------------------------

            int bufSize = 4096;
            Stream inStream = Console.OpenStandardInput(bufSize);
            Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));

            EnterMethod enterMethod;//last entered method
            string lastFilePath="-4"; // last inputted file path , by default equals "-4"

            Stopwatch timer = new Stopwatch(); // timer to measure solving time
            InputOutput.PrintTitle(); //colored title printing
            while (true)
            {
                //// -------------------------------- GET INPUT FROM USER -----------------------------------
                string boardinfo="";
                try
                {
                    switch (InputOutput.ShowOptionsAndGetUserChoice()) // get from user the selected option
                    {
                        case "1":
                            //user choose to enter the board manually in console board
                            boardinfo = InputOutput.ReadFromConsole();
                            enterMethod = EnterMethod.FromConsole;
                            break;
                        case "2":
                            //user choose to enter the board from text file selected in dialog box
                            boardinfo = InputOutput.ReadFromFile(out lastFilePath);
                            enterMethod = EnterMethod.FromFileDialog;
                            break;
                        case "3":
                            //user choose to enter the board from text file in filepath entered manually
                            boardinfo = InputOutput.ReadFromFilePath(out lastFilePath);
                            enterMethod = EnterMethod.FromFilePath;
                            break;
                        case "4":
                            //user choose to exit from program.
                            InputOutput.Print("\nGood bye!\n");
                            return;
                        default:
                            InputOutput.Print("\nNot in options,try again!\n");
                            continue;
                    }
                } // ------------------------- GET INPUT EXCEPTIONS ----------------------
                catch (IOException)
                {
                    InputOutput.Print("\nSomething went wrong , try again.\n", ConsoleColor.Red);
                    continue;
                }
                catch (ArgumentException)
                {
                    InputOutput.Print("\nSomething went wrong, check next time file_path is valid.\n", ConsoleColor.Red);
                    continue;
                }



                if (boardinfo == "-3")//'dialog-fail' code
                {
                    InputOutput.Print("\n Input file dialog has failed, try again.\n", ConsoleColor.Red);
                    continue;
                }
                if (boardinfo == "-4")//'filepath-fail' code
                {
                    InputOutput.Print("\n Invalid file path, try again.\n", ConsoleColor.Red);
                    continue;
                }

                if (!InputOutput.BoardIsValid(boardinfo))
                {
                    InputOutput.Print("\nThe board is not valid, check it and try again.\n");
                    continue;
                }
                //valid checkes went successfully
                Board board = new Board(boardinfo);
                board.Print();
                //activate the solve function
                timer.Start();
                string result = Solver.Solve(board);
                timer.Stop();

                if (result == "-1") //unsolvable-code
                {
                    InputOutput.Print("Board is unsolvable.", ConsoleColor.Red);
                }
                else
                {
                    //print the solved board
                    board.Print(ConsoleColor.Green);
                    switch (enterMethod) //checks how user entered the board, and return him the same way
                    {
                        //if entered from console,it return him the soultion in console
                        case EnterMethod.FromConsole:
                            InputOutput.Print(string.Format("\nBoard solved in: {0}\n", timer.Elapsed), ConsoleColor.Green);
                            InputOutput.Print(string.Format("Result as string: {0}\n", result), ConsoleColor.Green);
                            break;
                        //if entered from file,it return him the soultion in file
                        case EnterMethod.FromFilePath:
                        case EnterMethod.FromFileDialog:
                            string savedSolutionPath = lastFilePath.Replace(".txt", "_SOLVED.txt");//saved solution file at the same folder
                            try
                            {
                                bool succeed =  InputOutput.SaveFile(savedSolutionPath, result);
                                if (!succeed) // if file saved successfully
                                {
                                    //saving failed message
                                    InputOutput.Print(string.Format("\nBoard solved in: {0}\n", timer.Elapsed), ConsoleColor.Green);
                                    InputOutput.Print("\nYou already have soultion of this board in the same foldar.\n", ConsoleColor.Red);
                                    InputOutput.Print(string.Format("\nResult as string: {0}\n", result), ConsoleColor.Green);
                                }
                                else
                                {
                                    InputOutput.Print(string.Format("\nBoard solved in: {0}\n", timer.Elapsed), ConsoleColor.Green);
                                    //saving success message
                                    InputOutput.Print(string.Format("Solution file saved successfully at {0}.", savedSolutionPath), ConsoleColor.Green);
                                }
                            }
                            catch (Exception)
                            {
                                //unknown file saving file
                                InputOutput.Print("Saving went wrong somehow, try again.", ConsoleColor.Red);
                                InputOutput.Print(string.Format("Result as string: {0}\n", result), ConsoleColor.Green);
                                throw;
                            }
                            break;
                    }
                }
            }
        }
    }
}
