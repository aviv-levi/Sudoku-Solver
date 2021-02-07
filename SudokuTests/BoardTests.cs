using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Tests
{
    /// <summary>
    ///  UNIT TESTS OF MY SUDUKO SOLVER PROJECT.
    /// </summary>
    [TestClass()]
    public class BoardTests
    {

        // -------------------------------------------------------- SOLVABLE CHECKS -------------------------------------------------


        [TestMethod()]
        public void Test4X4Regular()
        {
            //Arrange
            Board board = new Board("1000001401200300");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test4X4Zeros()
        {
            //Arrange
            Board board = new Board("0000000000000000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test4X4Unsolvable()
        {
            //Arrange
            Board board = new Board("1200002000010002");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsFalse(resultIsValidSudukoSoultion);
        }



        [TestMethod()]
        public void Test9X9Regular()
        {
            //Arrange
            Board board = new Board("948600301005800469600540200307415690000063800010920004000390008020057000100204000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test9X9Zeros()
        {
            //Arrange
            Board board = new Board("000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test9X9Hard()
        {
            //Arrange
            Board board = new Board("800000000003600000070090200050007000000045700000100030001000068008500010090000400");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test9X9Unsolvable()
        {
            //Arrange
            Board board = new Board("837050000246173985951020000328597460674030100195060000509080073402010000703040009");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsFalse(resultIsValidSudukoSoultion);
        }



        [TestMethod()]
        public void Test16X16Regular()
        {
            //Arrange
            Board board = new Board("6007:1004>2?800=00@>0760<900004?0;=0008<07000069<00400>0036:072096001000>:0503008<0;9002?@00010004:0>005201;000<10000:00009<?2;0?020007=0109;008300008007=4000000071@<000;0050020@083510000064<00?05000000020=002180;0=70?0000>00060<000900=0000@04:?290;5300080");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test16X16Zeros()
        {
            //Arrange
            Board board = new Board("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }
        [TestMethod()]
        public void Test16X16Unsolvable()
        {
            //Arrange
            Board board = new Board("17062<;:3080=00?0000@703=01000<5050@0806<0004000:00;0000000700>0@1030000?>0800;0;8:4500000003>70000=;0400009008000701000004000=05>0070000:26000@00000:000004290100<?0000003160009=08<0000000000000174300:00?05600090005>;00000@400000?<020000=0020000@0000007000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsFalse(resultIsValidSudukoSoultion);
        }



        [TestMethod()]
        public void Test25X25Zeros()
        {
            //Arrange
            Board board = new Board("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //Act
            string result = Solver.Solve(board);
            bool resultIsValidSudukoSoultion = Board.CheckResult(result);
            //Assert
            Assert.IsTrue(resultIsValidSudukoSoultion);
        }


        // ----------------------------------------------------  VALIDATION CHECKS  ------------------------------------------------------


        [TestMethod()]
        public void TestNoValidLength()
        {
            //Arrange
            string boardinfo = "123400140120030043011";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }


        [TestMethod()]
        public void Test4X4NoValidValues()
        {
            //Arrange
            string boardinfo = "1234001401200300";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }
        [TestMethod()]
        public void Test4X4Full()
        {
            //Arrange
            string boardinfo = "1432321441232341";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }



        [TestMethod()]
        public void Test9X9NoValidValues()
        {
            //Arrange
            string boardinfo = "9486003010~~80046960054020030741569tz00063800010920004000390008020&&000100204000";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }
        [TestMethod()]
        public void Test9X9Full()
        {
            //Arrange
            string boardinfo="219764538734598162685132479926871345473256891851349726568427913342915687197683254";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }



        [TestMethod()]
        public void Test16X16NoValidValues()
        {
            //Arrange
            string boardinfo = "6~~~:1004>2?+*0=00@>0760<9qqq04?0;=0008<07000069<0040t>0036:072096001000>:05030z8<0;9002?@00010004:0>005201;000<10000:00009<?2;0?020007=0109;008300008007=4000000071@<000;0050020@083510000064<00?05000000020=002180;0=70?0000>00060<000900=0000@04:?290;5300080";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }
        [TestMethod()]
        public void Test16X16Full()
        {
            //Arrange
            string boardinfo = "1?9732@<=:5>;846;6285:974?1@=><3>:@<4=6;8923?517453=1>8?6<7;2:@9247?>9<=@8;5316:69812;:@34>?7=5<=;5@7?3691<:>4283<:>814527=69@?;9164;<?875:=@23>@7?5:3=2;>9<1684:8>3@5791642<;=?<=;264>1?3@8:975524;961><@?783:=?><6=82:5;31479@731:<@54>=896?;28@=9?7;3:2645<>1";
            //Act
            bool resultIsValidSuduko = InputOutput.BoardIsValid(boardinfo);
            //Assert
            Assert.IsFalse(resultIsValidSuduko);
        }


    }
}