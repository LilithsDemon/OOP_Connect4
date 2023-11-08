using System;
using System.Dynamic;
using gui_connect4;

namespace main_connect4
{
    class Program
    {

        public class User
        {
            public string username {get; private set;}
            public string colour {get; private set;}
            public int wins {get; private set;} = 0;

            public void AddWin()
            {
                wins++;
            }

            public User(string name, string given_colour)
            {
                username = name;
                colour = given_colour;
            }
        }

        static void Main()
        {
            int[ , ] rows = Board.FillBoard(6, 7, 0);
            User[] users = new User[] {new User("Test1", "red"), new User("Test2", "blue")};
            rows[5,0] = 1;
            rows[5,1] = 2;
            rows[4,0] = 1;
            rows[3,0] = 2;
            Board.DisplayBoard(rows, "red", "blue");
        }
    }
}