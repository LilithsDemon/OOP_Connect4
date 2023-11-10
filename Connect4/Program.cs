using System;
using System.Dynamic;
using BoardLogic;

namespace MainConnect4
{
    class Program
    {

        public class User
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

        static void Main()
        {
            User[] users = new User[] {new User("Player 1", ConsoleColor.Red), new User("Player 2", ConsoleColor.Blue)};
            Board board = new Board(users[0].Colour, users[1].Colour);
            bool player = false; // If false, player 1, if True player 2
            int pos = 0;
            bool can_place = true;
            while(true)
            {
                board.DisplayBoard();
                bool found = false;
                while(!found)
                {
                    string output_text = player == false ? $"{users[0].Username} Where would you Like to place your counter:" : $"{users[1].Username} Where would you Like to place your counter:";
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

                    if(player == false) can_place = board.PlaceCounter(pos, 1);
                    else can_place = board.PlaceCounter(pos, 2);

                    if(!can_place)
                    {
                        Console.WriteLine("You cannot place that there as that row is full!");
                        found = false;
                    }
                }
                player = !player;
            }
        }
    }
}