using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KCKGame
{
    class Program
    {
        static readonly int left = 0;
        static readonly int right = 1;
        static readonly int up = 2;
        static readonly int down = 3;
        
        static int firstPlayerScore = 0;
        static int firstPlayerDirection = right;
        static int firstPlayerColumn = 0;
        static int firstPlayerRow = 0;

        static int secondPlayerScore = 0;
        static int secondPlayerDirection = left;
        static int secondPlayerColumn = 0;
        static int secondPlayerRow = 0;

        static bool[,] isUsed;

        static void Main(string[] args)
        {
            SetGameField();

            int option = Menu();

            while (option != 3)
            {
                switch (option)
                {
                    case 0:
                        {
                            MenuStart();
                            break;
                        }
                    case 1:
                        {
                            MenuControl();
                            break;
                        }
                    case 2:
                        {
                            MenuAuthors();
                            break;
                        }
                }

                option = Menu();
            }
        }

        static void SetGameField()
        {
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;

            Console.WindowWidth = 100;
            Console.BufferWidth = 100;

            firstPlayerColumn = 0;
            firstPlayerRow = Console.WindowHeight / 2;

            secondPlayerColumn = Console.WindowWidth - 1;
            secondPlayerRow = Console.WindowHeight / 2;
        }

        static void Heading()
        {
            string heading = "██╗  ██╗ ██████╗██╗  ██╗     ██████╗  █████╗ ███╗   ███╗███████╗";
            int cursorLeft = Console.BufferWidth / 2 - heading.Length / 2;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorTop = 1;
            Console.CursorLeft = cursorLeft;
            Console.WriteLine(heading);
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("██║ ██╔╝██╔════╝██║ ██╔╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("█████╔╝ ██║     █████╔╝     ██║  ███╗███████║██╔████╔██║█████╗  ");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("██╔═██╗ ██║     ██╔═██╗     ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  ");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("██║  ██╗╚██████╗██║  ██╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝");
        }

        static int Menu()
        {
            Console.CursorVisible = false;
            ConsoleKeyInfo key;
            int currentSelection = 0;
            string[] options = { "start", "sterowanie", "o autorach", "koniec" }; 

            do
            {
                Heading();

                Console.CursorTop = 10;
                Console.ResetColor();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.CursorLeft = Console.BufferWidth / 2 - options[i].Length / 2;

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine(options[i].ToUpper());
                    Console.ResetColor();
                }

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (currentSelection < options.Length-1)
                                currentSelection++;
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (currentSelection > 0)
                                currentSelection--;
                            break;
                        }
                }

            } while (key.Key != ConsoleKey.Enter);

            return currentSelection;
        }

        static void MenuStart()
        {
            Console.Clear();

            isUsed = new bool[Console.WindowWidth, Console.WindowHeight];

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangePlayerDirection(key);
                }

                MovePlayers();

                bool firstPlayerLoses = DoesPlayerLose(firstPlayerRow, firstPlayerColumn);
                bool secondPlayerLoses = DoesPlayerLose(secondPlayerRow, secondPlayerColumn);

                if (firstPlayerLoses && secondPlayerLoses)
                {
                    firstPlayerScore++;
                    secondPlayerScore++;
                    Console.WriteLine();
                    Console.WriteLine("Game over");
                    Console.WriteLine("Draw game!!!");
                    Console.WriteLine("Current score: {0} - {1}", firstPlayerScore, secondPlayerScore);
                    ResetGame();
                }
                if (firstPlayerLoses)
                {
                    secondPlayerScore++;
                    Console.WriteLine();
                    Console.WriteLine("Game over");
                    Console.WriteLine("Second player wins!!!");
                    Console.WriteLine("Current score: {0} - {1}", firstPlayerScore, secondPlayerScore);
                    ResetGame();
                }
                if (secondPlayerLoses)
                {
                    firstPlayerScore++;
                    Console.WriteLine();
                    Console.WriteLine("Game over");
                    Console.WriteLine("First player wins!!!");
                    Console.WriteLine("Current score: {0} - {1}", firstPlayerScore, secondPlayerScore);
                    ResetGame();
                }

                isUsed[firstPlayerColumn, firstPlayerRow] = true;
                isUsed[secondPlayerColumn, secondPlayerRow] = true;

                WriteOnPosition(firstPlayerColumn, firstPlayerRow, '*', ConsoleColor.Yellow);
                WriteOnPosition(secondPlayerColumn, secondPlayerRow, '*', ConsoleColor.Red);

                Thread.Sleep(100);
            }
        }

        static void MenuReturn()
        {
            string retstring = "Naciśnij dowolny klawisz aby wrocić do menu...";

            Console.CursorLeft = Console.BufferWidth / 2 - retstring.Length / 2;
            Console.CursorTop = Console.BufferHeight - 5;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(retstring);

            Console.ReadKey();
            Console.Clear();
        }

        static void MenuControl()
        {
            Console.Clear();
            Heading();

            int x = 15;

            int cursorLeft = x;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorTop = 10;
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Gracz 1 - poruszanie się:\n");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("W - góra");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("S - dół");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("A - lewo");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("D - prawo");

            string longestString = "Gracz 2 - poruszanie się:";
            cursorLeft = Console.BufferWidth - longestString.Length - x;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorTop = 10;
            Console.CursorLeft = cursorLeft;
            Console.WriteLine(longestString+"\n");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Up Arrow - góra");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Down Arrow - dół");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Left Arrow - lewo");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Right Arrow - prawo");

            MenuReturn();
        }
        
        static void MenuAuthors()
        {
            Console.Clear();
            Heading();

            string text = "Projekt wykonali:";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorTop = 10;
            int cursorLeft = Console.BufferWidth / 2 - text.Length / 2;

            Console.CursorLeft = cursorLeft;
            Console.WriteLine(text+"\n");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Janeq Gierasimiuk");
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("Kamil Dołęgiewicz");

            MenuReturn();
        }

        static void ChangePlayerDirection(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.W && firstPlayerDirection != down)
            {
                firstPlayerDirection = up;
            }
            if (key.Key == ConsoleKey.A && firstPlayerDirection != right)
            {
                firstPlayerDirection = left;
            }
            if (key.Key == ConsoleKey.D && firstPlayerDirection != left)
            {
                firstPlayerDirection = right;
            }
            if (key.Key == ConsoleKey.S && firstPlayerDirection != up)
            {
                firstPlayerDirection = down;
            }

            if (key.Key == ConsoleKey.UpArrow && secondPlayerDirection != down)
            {
                secondPlayerDirection = up;
            }
            if (key.Key == ConsoleKey.LeftArrow && secondPlayerDirection != right)
            {
                secondPlayerDirection = left;
            }
            if (key.Key == ConsoleKey.RightArrow && secondPlayerDirection != left)
            {
                secondPlayerDirection = right;
            }
            if (key.Key == ConsoleKey.DownArrow && secondPlayerDirection != up)
            {
                secondPlayerDirection = down;
            }
        }

        static void MovePlayers()
        {
            if (firstPlayerDirection == right)
            {
                firstPlayerColumn++;
            }
            if (firstPlayerDirection == left)
            {
                firstPlayerColumn--;
            }
            if (firstPlayerDirection == up)
            {
                firstPlayerRow--;
            }
            if (firstPlayerDirection == down)
            {
                firstPlayerRow++;
            }

            if (secondPlayerDirection == right)
            {
                secondPlayerColumn++;
            }
            if (secondPlayerDirection == left)
            {
                secondPlayerColumn--;
            }
            if (secondPlayerDirection == up)
            {
                secondPlayerRow--;
            }
            if (secondPlayerDirection == down)
            {
                secondPlayerRow++;
            }
        }

        static bool DoesPlayerLose(int row, int col)
        {
            if (row < 0)
            {
                return true;
            }
            if (col < 0)
            {
                return true;
            }
            if (row >= Console.WindowHeight)
            {
                return true;
            }
            if (col >= Console.WindowWidth)
            {
                return true;
            }

            if (isUsed[col, row])
            {
                return true;
            }

            return false;
        }

        static void ResetGame()
        {
            isUsed = new bool[Console.WindowWidth, Console.WindowHeight];
            SetGameField();
            firstPlayerDirection = right;
            secondPlayerDirection = left;
            Console.WriteLine("Press any key to start again...");
            Console.ReadKey();
            Console.Clear();
            MovePlayers();
        }

        static void WriteOnPosition(int x, int y, char ch, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(ch);
        }
    }
}
