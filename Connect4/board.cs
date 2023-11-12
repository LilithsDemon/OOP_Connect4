using System;
using System.Drawing;

namespace BoardLogic
{
    public sealed class Board
    {
        private ConsoleColor[] Colours = new ConsoleColor[3];
        public int[,] WholeBoard {get; private set;} = FillBoard(0);

        public static int[,] FillBoard (int fill_val)
        {
            int[,] tempArray = new int[6,7];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    tempArray[i, j] = fill_val;
                }
            }
            return tempArray;
        }

        public void DisplayBoard()
        {
            Console.Clear();
            int current_val = 0;
            for (int i = 0; i < 6; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 7; j++)
                {
                    current_val = this.WholeBoard[i, j];
                    Console.ForegroundColor = this.Colours[current_val];
                    string output = current_val != 0 ? "O" : " ";
                    Console.Write(output);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 29)));
            Console.Write("|");
            for(int i = 0; i < 7; i++)
            {
                Console.Write($" {i+1} |");
            }
            Console.WriteLine("");
        }

        public bool PlaceCounter(int pos, int player)
        {
            pos--;
            for(int i = 0; i < 6; i++)
            {
                if(this.WholeBoard[i, pos] == 0)
                {
                    this.WholeBoard[i, pos] = player;
                    if(i > 0)
                    {
                        this.WholeBoard[i - 1, pos] = 0;
                    }
                    this.DisplayBoard();
                    Thread.Sleep(200);
                }
                else
                {
                    if(i == 0) return false;
                    else return true;
                }
            }
            return true;
        }

        public Board(ConsoleColor player_1_colour, ConsoleColor player_2_colour, ConsoleColor board_colour = ConsoleColor.White)
        {
            Colours[0] = board_colour;
            Colours[1] = player_1_colour;
            Colours[2] = player_2_colour;
        }
    }
}