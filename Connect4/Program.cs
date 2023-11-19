using System;
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
            public string Username {get; private set;}
            public ConsoleColor Colour {get; private set;}
            public int Wins {get; private set;} = 0;

            public void AddWin()
            {
                this.Wins++;
            }

            public User(string name, ConsoleColor given_colour)
            {
                this.Username = name;
                this.Colour = given_colour;
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
                Console.WriteLine($"Round: {Round}!");
                int pos = 0;
                Board.ReturnVal returnVal = new Board.ReturnVal(0, "");
                Thread.Sleep(200);
                while(Round < 4 || (Users[0].Wins == Users[1].Wins))
                {
                    while(true)
                    {
                        GameBoard.DisplayBoard();
                        bool found = false;
                        while(!found)
                        {
                            string output_text = PlayersTurn == false ? $"{Users[0].Username} Where would you Like to place your counter:" : $"{Users[1].Username} Where would you Like to place your counter:";
                            Console.WriteLine(output_text);
                            string user_input = Console.ReadLine() ?? "";
                            if(user_input == "")
                            { 
                                Console.WriteLine("Please enter a value");
                                continue;
                            }
                            found = int.TryParse(user_input, out pos);
                            if(found == false)
                            {
                                Console.WriteLine("Please enter a numerical value");
                                continue;
                            }
                            if(pos > 7 || pos < 1)
                            {
                                Console.WriteLine("Please enter a value bigger than 0 and smaller than 8");
                                found = false;
                                continue;
                            }

                            if(PlayersTurn == false) returnVal = GameBoard.PlaceCounter(pos, 1);
                            else returnVal = GameBoard.PlaceCounter(pos, 2);

                            if(returnVal.code == 400){
                                Console.WriteLine(returnVal.text);
                                Thread.Sleep(200);
                                found = false;
                            }
                        }

                        if(returnVal.code == 200)
                        {
                            GameBoard.AnimateWinner(int.Parse(returnVal.text));
                            Console.WriteLine($"{Users[int.Parse(returnVal.text) - 1].Username} Wins!!");
                            Users[int.Parse(returnVal.text) - 1].AddWin();
                            Round++;
                            Thread.Sleep(200);
                            break;
                        }
                        if(returnVal.code == 406)
                        {
                            Console.WriteLine("Board has been filled, there is no winner");
                            Round++;
                            break;
                        }
                        PlayersTurn = !PlayersTurn;
                    }
                }
                RevealWinner();
            }

            public void RevealWinner()
            {
                Console.Clear();
                int winner = (Users[0].Wins > Users[1].Wins) ? 0 : 1;
                Console.WriteLine($"{Users[winner].Username} Wins having won {Users[0].Wins} rounds");
            }

            public Game(User player_1, User player_2)
            {
                Users[0] = player_1;
                Users[1] = player_2;
                GameBoard = new Board(Users[0].Colour, Users[1].Colour);
                this.PlayGame();
            }
        }

        static void Main()
        {
            Game game = new Game(new User("Player 1", ConsoleColor.Blue), new User("Player 2", ConsoleColor.Red));
        }
    }
}