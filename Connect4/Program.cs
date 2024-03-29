﻿using System;
using System.Dynamic;
using System.Reflection.Metadata;
using BoardLogic;

namespace MainConnect4
{
    class Program
    {

        public record ReturnVal(int code, string text);

        public sealed class User
        {
            public string Username { get; private set; }
            public ConsoleColor Colour { get; private set; }
            public int Wins { get; private set; } = 0;

            /// <summary>
            /// Simply adds 1 to the numbers of win for the player
            /// </summary>
            public void AddWin()
            {
                this.Wins++;
            }

            /// <summary>
            /// Instantiates a new user
            /// </summary>
            /// <param name="name">Name of the user (string)</param>
            /// <param name="given_colour">ConsoleColor of the user for the board</param>
            public User(string name, ConsoleColor given_colour)
            {
                this.Username = name;
                this.Colour = given_colour;
            }

            public User(string name, int player)
            {
                this.Username = name;
                if(player == 1) this.Colour = ConsoleColor.Red;
                else this.Colour = ConsoleColor.Yellow;
            }
        }

        public sealed class Game
        {
            private int Round = 1;
            private User[] Users = new User[2];
            private bool PlayersTurn = false;
            private Board GameBoard;

            public void PlayGame()
            {
                int pos = 0;
                Board.ReturnVal returnVal = new Board.ReturnVal(0, "");
                while (this.Round < 4 || (this.Users[0].Wins == this.Users[1].Wins)) // Runs till 3 rounds have been played, or they are tied.
                {
                    Console.Clear();
                    Console.WriteLine($"Round: {this.Round}!");
                    Thread.Sleep(3000);
                    while (true)
                    {
                        this.GameBoard.DisplayBoard();
                        bool found = false;
                        while (!found)
                        {
                            string output_text = this.PlayersTurn == false ? $"{this.Users[0].Username} Where would you Like to place your counter:" : $"{this.Users[1].Username} Where would you Like to place your counter:";
                            Console.WriteLine(output_text);
                            string user_input = Console.ReadLine() ?? "";
                            if (user_input == "")
                            {
                                Console.WriteLine("Please enter a value");
                                continue;
                            }
                            found = int.TryParse(user_input, out pos);
                            if (found == false)
                            {
                                Console.WriteLine("Please enter a numerical value");
                                continue;
                            }
                            if (pos > 7 || pos < 1)
                            {
                                Console.WriteLine("Please enter a value bigger than 0 and smaller than 8");
                                found = false;
                                continue;
                            }

                            if (this.PlayersTurn == false) returnVal = this.GameBoard.PlaceCounter(pos, 1);
                            else returnVal = this.GameBoard.PlaceCounter(pos, 2);

                            if (returnVal.code == 400)
                            {
                                Console.WriteLine(returnVal.text);
                                Thread.Sleep(200);
                                found = false;
                            }
                        }

                        if (returnVal.code == 200)
                        {
                            Console.WriteLine($"{this.Users[int.Parse(returnVal.text) - 1].Username} Wins!!");
                            this.Users[int.Parse(returnVal.text) - 1].AddWin();
                            Round++;
                            Thread.Sleep(200);
                            break;
                        }
                        if (returnVal.code == 406)
                        {
                            Console.WriteLine("Board has been filled, there is no winner");
                            Round++;
                            Thread.Sleep(200);
                            break;
                        }
                        this.PlayersTurn = !this.PlayersTurn;
                    }
                }
                RevealWinner();
            }

            /// <summary>
            /// Used to show who has won the most number of games during the game
            /// </summary>
            public void RevealWinner()
            {
                Console.Clear();
                int winner = (this.Users[0].Wins > this.Users[1].Wins) ? 0 : 1;
                Console.WriteLine($"{this.Users[winner].Username} Wins having won {this.Users[0].Wins} rounds");
            }

            public Game(User player_1, User player_2)
            {
                this.Users[0] = player_1;
                this.Users[1] = player_2;
                this.GameBoard = new Board(this.Users[0].Colour, this.Users[1].Colour);
                this.PlayGame();
            }
        }

        /// <summary>
        /// Starts by allowing users to be able to pick their names and colours before starting the game off with their chosen values
        /// </summary>
        static void StartGame()
        {
            string[] usernames = new string[2];
            List<ConsoleColor> all_console_colours = ((ConsoleColor[]) ConsoleColor.GetValues(typeof(ConsoleColor))).ToList();
            ConsoleColor[] colours_picked = new ConsoleColor[2];

            Console.WriteLine("Welcome to Connect 4");
            for (int i = 0; i < 2; i++)
            {
                while (true)
                {
                    Console.WriteLine($"Player {i + 1}: What do you want your username to be: ");
                    usernames[i] = Console.ReadLine() ?? "";
                    if (usernames[i] != "") break;
                    else Console.WriteLine("Please enter an actual username! ");
                }
                while (true)
                {
                    Console.WriteLine($"Welcome {usernames[i]}: What colour would you like to be: Options: ");
                    for (int j = 0; j < all_console_colours.Count(); j++)
                    {
                        Console.WriteLine($"{j + 1}: {all_console_colours[j]}");
                    }
                    int chosen_colour = 0;
                    bool num = int.TryParse(Console.ReadLine() ?? "", out chosen_colour);
                    if (num && chosen_colour > 0 && chosen_colour - 1 < all_console_colours.Count())
                    {
                        colours_picked[i] = all_console_colours[chosen_colour - 1];
                        all_console_colours.RemoveAt(chosen_colour - 1);
                        break;
                    }
                }
            }

            Game game = new Game(new User(usernames[0], colours_picked[0]), new User(usernames[1], colours_picked[1]));
        }

        /// <summary>
        /// Uses a paramater to be able to start a fast game where it does not need to ask for names or colours
        /// </summary>
        static void FastStart()
        {
            Console.WriteLine("Welcome to Connect 4");
            Game game = new Game(new User("Player 1", 1), new User("Player 2", 2));
        }

        static void Main(string[] args)
        {
            try
            {
                if(args[0] == "fast") FastStart();
                else StartGame();
            }
            catch
            {
                StartGame();
            }
        }
    }
}