using System;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using Microsoft.VisualBasic;

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
        private const int WAITTIME = 200;
        private const int CONTINUECODE = 100;
        private const int WINNINGCODE = 200;
        private const int BOARDERROR = 400;
        private const int NOWINFOUND = 404;

        /// <summary>
        /// Fills the board with the value given
        /// </summary>
        /// <param name="fill_val">Value to fill the board with</param>
        /// <returns></returns>
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

        /// <summary>
        /// Outputs the Board to the screen
        /// </summary>
        public void DisplayBoard()
        {
            Console.Clear();
            Console.ForegroundColor = this.Colours[0];
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
                    Console.ForegroundColor = this.Colours[0];
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
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Simulates the board flashing to show someone has won
        /// </summary>
        /// <param name="player">The player's value</param>
        /// <param name="positions">A 2D array that has the positions of the win to show in the player's colours</param>
        private void AnimateWinner(int player, int[,] positions)
        {
            for(int i = 0; i < 3; i++)
            {
                this.WholeBoard = FillBoard(3);
                for(int j = 0; j < 4; j++)
                {
                    this.WholeBoard[positions[j,0], positions[j,1]] = player;
                }
                DisplayBoard();
                Thread.Sleep(WAITTIME);
                this.WholeBoard = FillBoard(0);
                DisplayBoard();
                Thread.Sleep(WAITTIME);
            }
        }

        /// <summary>
        /// Checks to see if the Board is completely filled with counters or not
        /// </summary>
        /// <returns>bool value: false if not filled, true if filled</returns>
        private bool BoardFilled()
        {
            for(int i = 0; i < HEIGHT; i++)
            {
                for(int j = 0; j < WIDTH; j++)
                {
                    if(this.WholeBoard[i,j] == 0) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Places a counter into the row given
        /// </summary>
        /// <param name="pos">Row that counter should be placed into</param>
        /// <param name="player">The player's value</param>
        /// <returns>A ReturnVal record with the values of ReturnVal.Code (int) and ReturnVal.Text (string)</returns>
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
                    Thread.Sleep(WAITTIME);
                }
                else
                {
                    if(i == 0) return new ReturnVal(BOARDERROR, "This row is already filled!");
                }
            }
            if(BoardFilled())
            {
                this.WholeBoard = FillBoard(0);
                return new ReturnVal (BOARDERROR, "Board is filled!");
            } 
            ReturnWinningVal check_win = CheckWin(placed_x, pos, player);
            if(check_win.code != WINNINGCODE) return new ReturnVal(CONTINUECODE, "Continue Game");
            else
            {
                AnimateWinner(player, check_win.positions);
                return new ReturnVal(WINNINGCODE, $"{player}");
            } 
        }

        /// <summary>
        /// Checks to see if the new counter placed is part of a winning line
        /// </summary>
        /// <param name="x">X position of the counter placed</param>
        /// <param name="y">Y position of the counter placed</param>
        /// <param name="player">The player's value of who placed the counter</param>
        /// <returns>A ReturnWinningVal which consits of the code(int) - ReturnWinningVal.Code and a list of the winning positions(int[,]) - ReturnWinnignVal.Positions</returns>
        public ReturnWinningVal CheckWin(int x, int y, int player)
        {
            // As I am dumb, x is y, and y is x :)

            int[,] positions = new int[4,2]; 
            positions[0,0] = x;
            positions[0,1] = y;

            /// <smmary>
            /// Check relative position given by moving across the 2D array in comparision to delta x and delta y and sees if those are the players
            /// </summary>
            /// <param name="x">X position counter was placed in</param>
            /// <param name="y">Y position counter was placed in</param>
            /// <param name="player">The players value</param>
            /// <param name="idx">The movement of the x value to check</param>
            /// <param name="idy">The movement of the y value to check</param>
            /// <returns>A interger value that returns how many counters have been counted that belong to the player 4 = win</returns>
            int CheckLine(int x, int y, int player, int idx, int idy)
            {
                int count = 1;
                for(int i = 0; i < 2; i++)
                {
                    idx = 0 - idx;
                    idy = 0 - idy;
                    int current_x = x;
                    int current_y = y;
                    while(current_x + idx > -1 && current_x + idx < HEIGHT && current_y + idy > - 1 && current_y + idy < WIDTH)
                    {
                        current_x += idx;
                        current_y += idy;
                        if(this.WholeBoard[current_x, current_y] == player)
                        {
                            positions[count,0] = current_x;
                            positions[count,1] = current_y;
                            count += 1;
                            if(count == 4) return count;
                        } else break;
                    }
                }
                return count;
            }

            if(CheckLine(x, y, player, -1, 0) == 4) return new ReturnWinningVal(WINNINGCODE, positions);
            if(CheckLine(x, y, player, 0, -1) == 4) return new ReturnWinningVal(WINNINGCODE, positions);
            if(CheckLine(x,y, player, -1, -1) == 4) return new ReturnWinningVal(WINNINGCODE, positions);
            if(CheckLine(x,y, player, -1, 1) == 4) return new ReturnWinningVal(WINNINGCODE, positions);

            return new ReturnWinningVal(NOWINFOUND, positions);
        }

        /// <summary>
        /// Intialsies the class Board
        /// </summary>
        /// <param name="player_1_colour">ConsoleColour of player 1</param>
        /// <param name="player_2_colour">ConsoleColour of player 2</param>
        /// <param name="board_colour">Colour the board is wanted to be (Is preset does not need to set if not wanted)</param>
        /// <param name="win_colour"Colour the board flashes when someone wins (Is preset does not need to set if not wanted)></param>
        public Board(ConsoleColor player_1_colour, ConsoleColor player_2_colour, ConsoleColor board_colour = ConsoleColor.Yellow, ConsoleColor win_colour = ConsoleColor.Green)
        {
            this.Colours[0] = board_colour;
            this.Colours[1] = player_1_colour;
            this.Colours[2] = player_2_colour;
            this.Colours[3] = win_colour;
        }
    }
}