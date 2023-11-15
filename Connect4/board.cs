using System;
using System.Drawing;

namespace BoardLogic
{
    public sealed class Board
    {
        private ConsoleColor[] Colours = new ConsoleColor[3];
        public int[,] WholeBoard {get; private set;} = FillBoard(0);

        public record ReturnVal(int code, string text);

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



        public ReturnVal PlaceCounter(int pos, int player)
        {
            int placed_x = 0;
            pos--;
            for(int i = 0; i < 6; i++)
            {
                if(this.WholeBoard[i, pos] == 0)
                {
                    this.WholeBoard[i, pos] = player;
                    placed_x = i;
                    if(i > 0)
                    {
                        this.WholeBoard[i - 1, pos] = 0;
                    } 
                    this.DisplayBoard();
                    Thread.Sleep(200);
                }
                else
                {
                    if(i == 0) return new ReturnVal(400, "This row is already filled!");
                }
            }
            if(CheckWin(placed_x, pos, player).code != 200) return new ReturnVal(100, "Continue Game");
            else return new ReturnVal(200, $" Player: {player} won!!!");
        }

        public ReturnVal CheckWin(int x, int y, int player)
        {
            if(x < 3) // Checking down from placed
            {
                if(this.WholeBoard[x + 1, y] == player && this.WholeBoard[x+2, y] == player && this.WholeBoard[x+3, y] == player) return new ReturnVal(200, "Player won");
            }

            int count = 1;
            int current_y = y - 1;
            while(current_y > -1)
            {
                if(this.WholeBoard[x,current_y] == player) //check left from start
                {
                    count += 1;
                    current_y -= 1;
                    if(count == 4) return new ReturnVal(200, "Player won");
                } else break;
            }
            current_y = y + 1;
            while(current_y < 7) // check right from start
            {
                if(this.WholeBoard[x,current_y] == player)
                {
                    count += 1;
                    current_y += 1;
                    if(count == 4) return new ReturnVal(200, "Player won");
                } else break;
            }

            count = 1;   

            return new ReturnVal(404, "Not a winning place");
        }

        public Board(ConsoleColor player_1_colour, ConsoleColor player_2_colour, ConsoleColor board_colour = ConsoleColor.White)
        {
            Colours[0] = board_colour;
            Colours[1] = player_1_colour;
            Colours[2] = player_2_colour;
        }
    }
}