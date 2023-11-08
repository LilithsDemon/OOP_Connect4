using System;

namespace gui_connect4
{
    public class Board
    {

        public static int[,] FillBoard (int i, int j, int fill_val)
        {
            int[,] tempArray = new int[6,7];
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 7; j++)
                {
                    tempArray[i, j] = fill_val;
                }
            }
            return tempArray;
        }

        public static void DisplayBoard(int[,] whole_board, string colour1, string colour2)
        {
            int current_val = 0;
            Dictionary <int, ConsoleColor> colour_map = new Dictionary <int, ConsoleColor> ();
            colour_map.Add(0, ConsoleColor.White);
            switch(colour1)
            {
                case ("red"):
                    colour_map.Add(1, ConsoleColor.Red);
                    colour_map.Add(2, ConsoleColor.Blue);
                    break;
                default:
                    colour_map.Add(1, ConsoleColor.Blue);
                    colour_map.Add(2, ConsoleColor.Red);
                    break;
            }
            for (int i = 0; i < 6; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 7; j++)
                {
                    current_val = whole_board[i, j];
                    Console.ForegroundColor = colour_map[current_val];
                    string output = current_val != 0 ? "O" : " ";
                    Console.Write(output);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 29)));
        }
    }
}