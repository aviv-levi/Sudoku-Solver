using Accord.Collections;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Sudoku
{
    /// <summary>
    /// The Suduko solver class.
    /// </summary>
    public static class Solver
    {

        private static Board board; // the suduko board

        private static BitArray[] rowsOptions; // bitArray array , every bitArray represent specific row, every bitArray contains which number is already caught(true)
        private static BitArray[] columnsOptions;//bitArray array , every bitArray represent specific column , every bitArray contains which number is already caught(true)
        private static BitArray[] boxesOptions;//bitArray array , every bitArray represent specific box , every bitArray contains which number is already caught(true)

        private static List<EmptyPlace> emptyPlaces; //empty places list , stay sorted everytime 



        ////------------------------------------ SOLVING METHODS -------------------------------------


        /// <summary>
        /// The suduko board solving algorithm.
        /// </summary>
        /// <param name="board">suduko board</param>
        /// <returns>string format of the solved board.</returns>
        public static string Solve(Board board)
        {
            Solver.board = board; //stored board in a static global var
            BuildOptions();// build bitArray arrays
            BuildEmptyPlaces();// build the empty places list
            return SolveBoard() ? board.ToString() : "-1"; //if board is solveable return his string format,else return -1 as 'unsolveable code'
        }



        /// <summary>
        /// The recorsive solve board algorithm.
        /// </summary>
        /// <returns>return true if board is solvable, retun false if it unsolvable.</returns>
        private static bool SolveBoard()
        {
            if (emptyPlaces.Count == 0) // the board is solved when there aren't any empty places in board.
            {
                return true;
            }

            //list is sorted by validOptions counter, so the first element is the min_option empty place
            EmptyPlace min = emptyPlaces[0];

            //i stored in empty place a value of an only option in case of 1 valid option, so i can directly use it instead of go for loop
            while (min.GetNumOfValidOptions() == 1 && emptyPlaces.Count != 0)
            {
                //insert option to board
                emptyPlaces.Remove(min);//remove from empty place because we fill it now
                board[min.GetRow(), min.GetCol()] = min.GetOnlyOption(); // insert to board
                SetBitOptions(min.GetRow(), min.GetCol(), min.GetBoxIndex(), min.GetOnlyOption() - 1, true);// set the bitarrays
                UpdateEmptyPlaces(min.GetRow(), min.GetCol(), min.GetBoxIndex());//update the empty places elements
                if (SolveBoard())//if option is good
                {
                    return true;
                }
                //if option isnt good, we undo the place to be empty again
                SetBitOptions(min.GetRow(), min.GetCol(), min.GetBoxIndex(), min.GetOnlyOption() - 1, false);//set the bitarrays to remove the option 
                board[min.GetRow(), min.GetCol()] = 0;//insert 0 which represnts its empty place
                emptyPlaces.Add(min);//return place to emptyPlaces list
                UpdateEmptyPlaces(min.GetRow(), min.GetCol(), min.GetBoxIndex());//update empty places elements
                return false;
            }

            if (emptyPlaces.Count == 0) // if there are no empty places left
            {
                return true;
            }

            for (int i = 0; i < min.GetOptions().Count; i++)
            {
                if (min.GetOptions()[i]) // if bit in current index is true,this option is valid
                {
                    //insert option to board
                    emptyPlaces.Remove(min);//remove from empty place because we fill it now
                    board[min.GetRow(), min.GetCol()] = i + 1; // insert to board
                    SetBitOptions(min.GetRow(), min.GetCol(), min.GetBoxIndex(), i, true);// set the bitarrays
                    UpdateEmptyPlaces(min.GetRow(), min.GetCol(), min.GetBoxIndex());//update the empty places elements
                    if (SolveBoard())//if option is good
                    {
                        return true;
                    }
                    //if option isnt good, we undo the place to be empty again
                    SetBitOptions(min.GetRow(), min.GetCol(), min.GetBoxIndex(), i, false);//set the bitarrays to remove the option 
                    board[min.GetRow(), min.GetCol()] = 0;//insert 0 which represnts its empty place
                    emptyPlaces.Add(min);//return place to emptyPlaces list
                    UpdateEmptyPlaces(min.GetRow(), min.GetCol(), min.GetBoxIndex());//update empty places elements
                }
            }
            // if every option isnt good,go back to the function who called you(recursion)
            return false;

        }


        ////----------------------------------------------- PRE SOLVING METHODS --------------------------------------------------



        /// <summary>
        /// Build bitArray arrays.
        /// </summary>
        private static void BuildOptions()
        {
            rowsOptions = new BitArray[board.GetRows()];
            columnsOptions = new BitArray[board.GetColumns()];
            boxesOptions = new BitArray[board.GetNumOfBoxes()];
            int length = board.GetRows(); // because we know its squre, the rows,cols,boxes length is same, so i picked one and stored it in local variable
            // create every bit array
            for (int i = 0; i < length; i++)
            {
                //every bitarray need a length of the range values number, whice is the length of the row , which we stored in a local var
                rowsOptions[i] = new BitArray(length);
                rowsOptions[i].SetAll(false);
                columnsOptions[i] = new BitArray(length);
                columnsOptions[i].SetAll(false);
                boxesOptions[i] = new BitArray(length);
                boxesOptions[i].SetAll(false);
            }
            //run over the board and set the options  
            for (int i = 0; i < board.GetRows(); i++)
            {
                for (int j = 0; j < board.GetColumns(); j++)
                {
                    if (board[i, j] != 0) //for every element,update his row options,col options,box options , that his number is caught
                    {
                        //set bit in index of number as true
                        rowsOptions[i].Set(board[i, j] - 1, true);
                        columnsOptions[j].Set(board[i, j] - 1, true);
                        boxesOptions[GetBoxIndex(i, j)].Set(board[i, j] - 1, true);
                    }
                }
            }
           
        }


        /// <summary>
        /// Build the empty places list and insert the empty places as objects
        /// </summary>
        private static void BuildEmptyPlaces()
        {
            emptyPlaces = new List<EmptyPlace>();

            //run all over the board and build for every empty place a EmptyPlace object and add it to emptyPlaces list
            for (int i = 0; i < board.GetRows(); i++)
            {
                for (int j = 0; j < board.GetColumns(); j++)
                {
                    //if place is empty
                    if (board[i, j] == 0)
                    {
                        //build object
                        EmptyPlace currentEmptyPlace = new EmptyPlace(i, j, GetBoxIndex(i, j));
                        //set his options
                        currentEmptyPlace.UpdateOptions(GetRowCopy(currentEmptyPlace.GetRow()), GetColCopy(currentEmptyPlace.GetCol()), GetBoxCopy(currentEmptyPlace.GetBoxIndex()));
                        //add to emptyPlaces list
                        emptyPlaces.Add(currentEmptyPlace);
                    }
                }
            }
            //keep list sorted
            emptyPlaces.Sort();
        }



        ////----------------------------------------------UPDATE METHODS----------------------------------------------


        /// <summary>
        /// Update empty places list options.
        /// </summary>
        /// <param name="row">empty place row</param>
        /// <param name="col">empty place column</param>
        /// <param name="box">empty place box</param>
        private static void UpdateEmptyPlaces(int row, int col, int box)
        {
            //run over the empty places list
            for (int i = 0; i < emptyPlaces.Count; i++)
            {
                //if current element is in the entered row, col or box index, set your update options
                if (emptyPlaces[i].GetRow() == row || emptyPlaces[i].GetCol() == col || emptyPlaces[i].GetBoxIndex() == box)
                {
                    EmptyPlace emptyPlace = emptyPlaces[i];

                    BitArray rowCopy = (BitArray)rowsOptions[emptyPlace.GetRow()].Clone();
                    BitArray colCopy = (BitArray)columnsOptions[emptyPlace.GetCol()].Clone();
                    BitArray boxCopy = (BitArray)boxesOptions[emptyPlace.GetBoxIndex()].Clone();
                    emptyPlace.UpdateOptions(rowCopy, colCopy, boxCopy);
                }


            }
            //at the end of changes sort the updated list
            emptyPlaces.Sort();
        }

        /// <summary>
        /// Turn on/off the bit at a specified index in a specified bitarrays of place.
        /// </summary>
        /// <para> Update valid options of places in the same row/col/box.</para>
        /// <param name="row">place's row in board</param>
        /// <param name="column">place's column in board</param>
        /// <param name="box">place's box in board</param>
        /// <param name="validOptionIndex">index of number in bitarray</param>
        /// <param name="turnon">set the bit to true/false</param>
        private static void SetBitOptions(int row, int column, int box, int validOptionIndex, bool turnon)
        {
            rowsOptions[row].Set(validOptionIndex, turnon);
            columnsOptions[column].Set(validOptionIndex, turnon);
            boxesOptions[box].Set(validOptionIndex, turnon);
        }


        ////------------------------------------------ HELPER METHODS ----------------------------------------


        /// <summary>
        /// Getter to bit array of a specified row.
        /// </summary>
        /// <param name="row">row in suduko board</param>
        /// <returns>copy of specified row bit array.</returns>
        public static BitArray GetRowCopy(int row)
        { 
            return (BitArray)rowsOptions[row].Clone();
        }


        /// <summary>
        /// Getter to bit array of a specified column.
        /// </summary>
        /// <param name="column">column in suduko board</param>
        /// <returns>copy of specified column bit array.</returns>
        /// 

        public static BitArray GetColCopy(int column)
        {
            return (BitArray)columnsOptions[column].Clone();
        }


        /// <summary>
        /// Getter to bit array of a specified box index.
        /// </summary>
        /// <param name="box">box index in suduko board</param>
        /// <returns>copy of specified box index bit array.</returns>
        public static BitArray GetBoxCopy(int box)
        {
            return (BitArray)boxesOptions[box].Clone();
        }


        /// <summary>
        /// Compute to box index
        /// </summary>
        /// <param name="row">row in suduko board</param>
        /// <param name="column">column in suduko board</param>
        /// <returns>the box index in that row and column.</returns>
        private static int GetBoxIndex(int row, int column)
        {
            //number of element in box is same as in row , therefore sqrt(rows) equals to the side of the boxs
            int boxSideLength = (int)Math.Sqrt(board.GetRows());

            int boxstartX = column - (column % boxSideLength); //the box starts at column minus the relative index(column) of place
            int boxstartY = row - (row % boxSideLength);//the box starts at row minus the relative index(row) of place

            //box index calculation
            int placesAboveMeCount = boxstartY * boxSideLength; // how many rows above me * number of places in each row 
            int placesCounter = placesAboveMeCount + boxstartX; // add places in my row and get all places till that place
            int boxesCounter = placesCounter / boxSideLength;// all places divide by number of places in every box
            return boxesCounter; // get the box index
        }
    }
}
