using System;
using System.Drawing;

namespace BoardLogic
{
    public sealed class Board
    {
        private ConsoleColor[] Colours = new ConsoleColor[4];
        public int[,] WholeBoard {get; private set;} = FillBoard(0);

        public record ReturnVal(int code, string text);
        public record ReturnWinningVal(int code, int[,] positions);

        private const int HEIGHT = 6;
        private const int WIDTH = 7;

        private static int[,] FillBoard (int fill_val)
        {
            int[,] tempArray = new int[HEIGHT,WIDTH];
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
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
            for (int i = 0; i < HEIGHT; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < WIDTH; j++)
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
            for(int i = 0; i < WIDTH; i++)
            {
                Console.Write($" {i+1} |");
            }
            Console.WriteLine("");
        }

        public void AnimateWinner(int player, int[,] positions)
        {
            for(int i = 0; i < 3; i++)
            {
                WholeBoard = FillBoard(3);
                for(int j = 0; j < 4; j++)
                {
                    this.WholeBoard[positions[j,0], positions[j,1]] = player;
                }
                DisplayBoard();
                Thread.Sleep(200);
                WholeBoard = FillBoard(0);
                DisplayBoard();
                Thread.Sleep(200);
            }
        }

        private bool BoardFilled()
        {
            for(int i = 0; i < HEIGHT; i++)
            {
                for(int j = 0; j < WIDTH; j++)
                {
                    if(WholeBoard[i,j] == 0) return false;
                }
            }

            return true;
        }

        public ReturnVal PlaceCounter(int pos, int player)
        {
            int placed_x = 0;
            pos--;
            for(int i = 0; i < HEIGHT; i++)
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
            if(BoardFilled())
            {
                WholeBoard = FillBoard(0);
                return new ReturnVal (406, "Board is filled!");
            } 
            ReturnWinningVal check_win = CheckWin(placed_x, pos, player);
            if(check_win.code != 200) return new ReturnVal(100, "Continue Game");
            else
            {
                AnimateWinner(player, check_win.positions);
                return new ReturnVal(200, $"{player}");
            } 
        }

        public ReturnWinningVal CheckWin(int x, int y, int player)
        {
            // As I am dumb, x is y, and y is x :)

            int count = 1;
            int[,] positions = new int[4,2]; 
            positions[0,0] = x;
            positions[0,1] = y;

            if(x < 3) // Checking down from placed
            {
                for(int i = 0; i < 3; i++)
                {
                    if(this.WholeBoard[x + i + 1, y] == player)
                    {
                        positions[i+1, 0] = x + i + 1;
                        positions[i+1, 1] = y;
                        count += 1;
                        if(count == 4) return new ReturnWinningVal(200, positions);
                    } else break;
                }
            }

            int current_y = y - 1;
            while(current_y > -1)
            {
                if(this.WholeBoard[x,current_y] == player) //check left from start
                {
                    positions[count,0] = x;
                    positions[count,1] = current_y;
                    count += 1;
                    current_y -= 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }
            current_y = y + 1;
            while(current_y < WIDTH) // check right from start
            {
                if(this.WholeBoard[x,current_y] == player)
                {
                    positions[count,0] = x; 
                    positions[count,1] = current_y;
                    count += 1;
                    current_y += 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }

            count = 1;
            int current_x = x+1;
            current_y = y-1;
            while(current_y > -1 && current_x < HEIGHT) // Diagonal, low to high
            {
                if(this.WholeBoard[current_x,current_y] == player) //check left from start
                {
                    positions[count,0] = current_x;
                    positions[count,1] = current_y;
                    count += 1;
                    current_y -= 1;
                    current_x += 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }
            current_y = y + 1;
            current_x = x-1;
            while(current_y < WIDTH && current_x > -1) // check right from start
            {
                if(this.WholeBoard[current_x,current_y] == player)
                {
                    positions[count,0] = current_x;
                    positions[count,1] = current_y;
                    count += 1;
                    current_y += 1;
                    current_x -= 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }

            count = 1;
            current_x = x-1;
            current_y = y-1;
            while(current_y > -1 && current_x > -1) // Diagonal, low to high
            {
                if(this.WholeBoard[current_x,current_y] == player) //check left from start
                {
                    positions[count,0] = current_x;
                    positions[count,1] = current_y;
                    count += 1;
                    current_y -= 1;
                    current_x -= 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }
            current_y = y + 1;
            current_x = x + 1;
            while(current_y < WIDTH && current_x < HEIGHT) // check right from start
            {
                if(this.WholeBoard[current_x,current_y] == player)
                {
                    positions[count,0] = current_x;
                    positions[count,1] = current_y;
                    count += 1;
                    current_y += 1;
                    current_x += 1;
                    if(count == 4) return new ReturnWinningVal(200, positions);
                } else break;
            }


            return new ReturnWinningVal(404, positions);
        }

        public Board(ConsoleColor player_1_colour, ConsoleColor player_2_colour, ConsoleColor board_colour = ConsoleColor.White, ConsoleColor win_colour = ConsoleColor.Green)
        {
            Colours[0] = board_colour;
            Colours[1] = player_1_colour;
            Colours[2] = player_2_colour;
            Colours[3] = win_colour;
        }
    }
}