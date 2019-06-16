using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laboratory1 {
    class Program {
        static void Main(string[] args) {
            string sudokuPattern = "457682193600000007100000004200000006584716239300000008800000002700000005926835471"; // sudoku w postaci stringa powinno zawierac 81 cyfr

            SudokuState startState = new SudokuState(sudokuPattern);
            SudokuSearch searcher = new SudokuSearch(startState);
            searcher.DoSearch();

            IState state = searcher.Solutions[0];

            List<SudokuState> solutionPath = new List<SudokuState>();

            while (state != null)
            {
                solutionPath.Add((SudokuState)state);
                state = state.Parent;
            }
            solutionPath.Reverse();

            foreach (SudokuState s in solutionPath)
            {
                s.Print();
                s.ComputeHeuristicGrade();
            }

            Console.ReadLine();
        }
    }
}
