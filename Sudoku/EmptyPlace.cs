using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    /// <summary>
    /// This class represnts empty place in board. 
    /// </summary>
    public class EmptyPlace : IComparable<EmptyPlace>
    {
        private int row; //row in board
        private int column; // column in board
        private int box; // box index in board

        private BitArray options; // bit array, have length of valid range (1<=x<=square_side_length),contains 1 (true) if number(index in array) is valid,0 (false) if not.
        private int validOptions;// count of valid options (num of trues in optinos)

        private int onlyOption;//in case we have 1 valid option , he will be stored here.
        public EmptyPlace(int row,int col,int boxIndex)
        {
            this.row = row;
            this.column = col;
            this.box = boxIndex;
            this.onlyOption = -1;
        }
        public EmptyPlace(EmptyPlace place) // clone constructor 
        {
            this.row = place.row;
            this.column = place.column;
            this.box = place.box;
            this.options = place.options;
            this.validOptions = place.validOptions;
            this.onlyOption = place.onlyOption;
        }

        //// ----------------------------------------------- GETTERS --------------------------------------------


        /// <summary>
        /// Getter of row field.
        /// </summary>
        /// <returns>Row of empty place in board.</returns>
        public int GetRow()
        {
            return this.row;
        }


        /// <summary>
        /// Getter of col field.
        /// </summary>
        /// <returns>Col of empty place in board.</returns>
        public int GetCol()
        {
            return this.column;
        }



        /// <summary>
        /// Getter of box field.
        /// </summary>
        /// <returns>Box index of empty place in board.</returns>
        public int GetBoxIndex()
        {
            return this.box;
        }



        /// <summary>
        /// Getter of validOptions field.
        /// </summary>
        /// <returns>Number of valid Options.</returns>
        public int GetNumOfValidOptions()
        {
            return this.validOptions;
        }



        /// <summary>
        /// Getter of options field.
        /// </summary>
        /// <returns>BitArray of options to this specific empty place.</returns>
        public BitArray GetOptions()
        {
            return this.options;
        }



        /// <summary>
        /// Getter of only option field.
        /// </summary>
        /// <returns>The option in case of one valid option. Otherwise -1</returns>
        public int GetOnlyOption()
        {
            return this.onlyOption;
        }


        //// ------------------------------------------------------ SETTERS -----------------------------------------------------------------


        /// <summary>
        /// Setter of row field.
        /// </summary>
        /// <param name="row"></param>
        public void SetRow(int row)
        {
            this.row = row;
        }



        /// <summary>
        /// Setter of col field.
        /// </summary>
        /// <param name="col"></param>
        public void SetCol(int col)
        {
            this.column = col;
        }



        /// <summary>
        /// Setter of box field.
        /// </summary>
        /// <param name="boxIndex">new box index to empty place</param>
        public void SetBoxIndex(int boxIndex)
        {
            this.box = boxIndex;
        }


        //// ----------------------------------------------- UPDATE METHODS --------------------------------------------------



        /// <summary>
        /// Update the bit array of options that represent the valid option.
        /// </summary>
        /// <param name="rowOptions">Bit array that represnt which numbers are valid at that line.</param>
        /// <param name="colOptions">Bit array that represnt which numbers are valid at that col.</param>
        /// <param name="boxOptions">Bit array that represnt which numbers are valid at that box.</param>
        public void UpdateOptions(BitArray rowOptions,BitArray colOptions,BitArray boxOptions)
        {
            BitArray caughtedNumbers = rowOptions.Or(colOptions.Or(boxOptions)); // combined all trues(caughted numbers) to bit array

            // -------------- OR ----------------------
            //    | X1 | X2 | X1 OR X2  |
            //    |---------------------|
            //    | 0 |  0 |     0      |
            //    |---------------------|
            //    | 0 |  1 |     1      |
            //    |---------------------|
            //    | 1 |  0 |     1      |
            //    |---------------------|
            //    | 1 |  1 |     1      |
            //    |---------------------|

            //-------- Or operation example------------
            //  bitarray1 :   1 0 1 1 0 0 0 1
            //  bitarray2 :   0 0 1 0 0 1 0 1
            //  result :      1 0 1 1 0 1 0 1

            //i use this operation to get me the valid value for this empty area
            //i take the bit arrays of the row , column and box of this empty place and do combination of all 3 of them
            //this gets me all true values in those arrays, and false if any of them hasnt the number
            //this is how i get all caughtedNumbers

            this.options = caughtedNumbers.Not();//flip true to false,so now all are valid numbers(options) are true
            // for convenience i fliped true and false,so now every true valus is a option, so this how i get options array

            //counting valid options (trues)
            this.validOptions = 0; // count all options
            int onlyVal=-1; // temp only value (case of one option)
            for (int i = 0; i < this.options.Length; i++)
            {
                if (this.options[i]) //if true
                {
                    onlyVal = i + 1;
                    this.validOptions++;//increase counter
                }
            }
            if (this.validOptions == 1) { this.onlyOption = onlyVal; } // if there is only one option, we set validOptions counter of this.empty place
        }



        /// <summary>
        /// IComparable function , Compare Between this and other , compare by the value of validOptions.
        /// </summary>
        /// <param name="other">Other empty place to compare with b.</param>
        /// <returns>return 1 if this greater then other,0 if they are equal,-1 if this less then other</returns>
        public int CompareTo(EmptyPlace other)
        {
            return this.validOptions - other.validOptions;
        } 
    }
}


