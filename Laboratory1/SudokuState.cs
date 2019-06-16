using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class SudokuState : State
    {
        private string id;
        private int xPosition;
        private int yPosition;
        string[] squareString = {"[00][01][02][10][11][12][20][21][22]", "[03][04][05][13][14][15][23][24][25]",
            "[06][07][08][16][17][18][26][27][28]", "[30][31][32][40][41][42][50][51][52]", "[33][34][35][43][44][45][53][54][55]",
            "[36][37][38][46][47][48][56][57][58]", "[60][61][62][70][71][72][80][81][82]", "[63][64][65][73][74][75][83][84][85]", "[66][67][68][76][77][78][86][87][88]" };
        
        public const int SMALL_GRID_SIZE = 3;        public const int GRID_SIZE = SMALL_GRID_SIZE * SMALL_GRID_SIZE;
        private int[,] table;        public int[,] Table
        {
            get { return this.table; }
            set { this.table = value; }
        }
        
        public SudokuState(string sudokuPattern) : base()
        {
            if (sudokuPattern.Length != GRID_SIZE * GRID_SIZE)
            {
                throw new ArgumentException("SudokuSring posiada niewlasciwa dlugosc.") ;
            }

            // utworzenie id
            this.id = sudokuPattern;
            // alokacja i wypelnienie tablicy przechowujacej stan sudoku
            this.table = new int[GRID_SIZE, GRID_SIZE];

            for (int i = 0; i < GRID_SIZE; ++i)
            {
                for (int j = 0; j < GRID_SIZE; ++j)
                {
                    this.table[i, j] = sudokuPattern[i * GRID_SIZE + j]
                   - 48;
                }
            }

            // obliczenie heurystyki
            this.h = ComputeHeuristicGrade();

        }

        public SudokuState(SudokuState parent, int newValue, int x, int y) :
       base(parent)
        {
            xPosition = x;
            yPosition = y;
            this.table = new int[GRID_SIZE, GRID_SIZE];
            // Skopiowanie stanu sudoku do nowej tabeli
            Array.Copy(parent.table, this.table, this.table.Length);
            // Ustawienie nowej wartosci w wybranym polu sudoku
            this.table[x, y] = newValue;

            // Utworzenie nowego id odpowiadajacemu aktualnemu stanowi planszy
            StringBuilder builder = new StringBuilder(parent.id);
            builder[x * GRID_SIZE + y] = (char)(newValue + 48);
            this.id = builder.ToString();

            this.h = ComputeHeuristicGrade();
        }



        public override string ID
        {
            get { return this.id; }
        }

        public override double ComputeHeuristicGrade()
        {
            var rowCounter = countZerosInRow();
            var columnCounter = countZerosInColumn();
            var squareCounter = countZerosInSquare();

           
            return rowCounter + columnCounter + squareCounter;
        }

        public double countZerosInRow()
        {
            int zerosCounter = 0;
            for(int i = 0; i < 9; i++)
            {
                if (xPosition == i) continue;
                if (table[i, yPosition] == 0)
                {
                    zerosCounter++;
                }
                else if (table[xPosition, yPosition] == table[i, yPosition])
                {
                    return double.PositiveInfinity;
                }
            }
            return zerosCounter;
        }

        public double countZerosInColumn()
        {
            int zerosCounter = 0;
            for (int i = 0; i < 9; i++)
            {
                if (yPosition == i) continue;
                if (table[xPosition, i] == 0)
                {
                    zerosCounter++;
                }
                else if (table[xPosition, yPosition] == table[xPosition, i])
                {
                    return double.PositiveInfinity;
                }
            }
            return zerosCounter;
        }

        public double countZerosInSquare()
        {
            //[00][01][02][10][11][12][20][21][22]
            //0-35
            //x to 1,5,9,13,17,21,25,29,33
            int squareNumber = findSquare();
            int x = 1;
            int y = 2;
            int zerosCounter = 0;
            for (int i = 0; i < 9; i++)
            {
                int xIndex = squareString[squareNumber][x] - 48;
                x += 4;
                int yIndex = squareString[squareNumber][y] - 48;
                y += 4;
                if (xIndex == xPosition && yIndex == yPosition) continue;
                if(table[xIndex, yIndex] == 0)
                {
                    zerosCounter++;
                }
                else if (table[xPosition, yPosition] == table[xIndex, yIndex])
                {
                    return double.PositiveInfinity;
                }
            }
            return zerosCounter;
        }

        public int findSquare()
        {
            string coordinate = "[" + xPosition.ToString() + yPosition.ToString() + "]";
            for(int i = 0; i < 9; i++)
            {
                if(squareString[i].Contains(coordinate))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Print()
        {
            int rowLength = table.GetLength(0);
            int colLength = table.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    
                    if(j == 2 || j == 5)
                    {
                        if(table[i, j] == 0)
                        {
                            Console.Write(" " + "|");
                        }
                        else
                        {
                            Console.Write(table[i, j] + "|");
                        }

                    }
                    else
                    {
                        if (table[i, j] == 0)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.Write(table[i, j]);
                        }
                    }
                }
                Console.Write(Environment.NewLine);
                if(i == 2 || i == 5)
                {
                    Console.Write(Environment.NewLine);
                }
            }
            Console.WriteLine();
        }

    }
}
