using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KCKGame
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
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
            firstPlayerScore = 0;
            secondPlayerScore = 0;

            string startText = "Naciśnij dowolny klawisz, żeby zacząć rozgrywkę.";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorTop = Console.WindowHeight / 2;
            Console.CursorLeft = Console.WindowWidth / 2 - startText.Length / 2;
            Console.WriteLine(startText);

            Console.ReadKey();
            Console.Clear();

            int totalRoundNumber = 10;
            int roundNumber = 1;


            MakeObstacles(roundNumber);


            string[] text = { "Koniec rundy x", "", "Wynik: x - x"};

            while (roundNumber <= totalRoundNumber)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangePlayerDirection(key);
                }

                MovePlayers();

                bool firstPlayerLoses = DoesPlayerLose(firstPlayerRow, firstPlayerColumn);
                bool secondPlayerLoses = DoesPlayerLose(secondPlayerRow, secondPlayerColumn);

                Console.ForegroundColor = ConsoleColor.Yellow;

                if (firstPlayerLoses && secondPlayerLoses)
                {
                    firstPlayerScore++;
                    secondPlayerScore++;

                    text[1] = "Remis!!!";
                    Console.CursorTop = 1;
                    Console.CursorLeft = Console.WindowWidth / 2 - text[0].Length / 2;
                    Console.WriteLine("Koniec rundy {0}", roundNumber);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[1].Length / 2;
                    Console.WriteLine(text[1]);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[2].Length / 2;
                    Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);

                    ResetGame(ref roundNumber);
                }
                else if (firstPlayerLoses)
                {
                    secondPlayerScore++;

                    text[1] = "Wygrywa gracz po prawej!";
                    Console.CursorTop = 1;
                    Console.CursorLeft = Console.WindowWidth / 2 - text[0].Length / 2;
                    Console.WriteLine("Koniec rundy {0}", roundNumber);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[1].Length / 2;
                    Console.WriteLine(text[1]);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[2].Length / 2;
                    Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);

                    ResetGame(ref roundNumber);
                }
                else if (secondPlayerLoses)
                {
                    firstPlayerScore++;

                    text[1] = "Wygrywa gracz po lewej!";
                    Console.CursorTop = 1;
                    Console.CursorLeft = Console.WindowWidth / 2 - text[0].Length / 2;
                    Console.WriteLine("Koniec rundy {0}", roundNumber);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[1].Length / 2;
                    Console.WriteLine(text[1]);
                    Console.CursorLeft = Console.WindowWidth / 2 - text[2].Length / 2;
                    Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);

                    ResetGame(ref roundNumber);
                }

                

                isUsed[firstPlayerColumn, firstPlayerRow] = true;
                isUsed[secondPlayerColumn, secondPlayerRow] = true;

                WriteSnakeHead();
                

                WriteOnPosition(firstPlayerColumn, firstPlayerRow, '°', ConsoleColor.DarkCyan);
                WriteOnPosition(secondPlayerColumn, secondPlayerRow, '°', ConsoleColor.Green);

                Thread.Sleep(100);
            }

            string[] endText = { "Koniec gry", "", "Wynik: x - x", "Naciśnij dowolny klawisz, aby wrócić do menu..." };

            if (firstPlayerScore == secondPlayerScore)
            {
                endText[1] = "Remis!!!";
                Console.CursorTop = 1;
                Console.CursorLeft = Console.WindowWidth / 2 - endText[0].Length / 2;
                Console.WriteLine(endText[0]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[1].Length / 2;
                Console.WriteLine(endText[1]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[2].Length / 2;
                Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);
            }
            else if (firstPlayerScore > secondPlayerScore)
            {
                endText[1] = "Wygrał gracz po lewej!";
                Console.CursorTop = 1;
                Console.CursorLeft = Console.WindowWidth / 2 - endText[0].Length / 2;
                Console.WriteLine(endText[0]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[1].Length / 2;
                Console.WriteLine(endText[1]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[2].Length / 2;
                Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);
            }
            else if (firstPlayerScore < secondPlayerScore)
            {
                endText[1] = "Wygrał gracz po prawej!";
                Console.CursorTop = 1;
                Console.CursorLeft = Console.WindowWidth / 2 - endText[0].Length / 2;
                Console.WriteLine(endText[0]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[1].Length / 2;
                Console.WriteLine(endText[1]);
                Console.CursorLeft = Console.WindowWidth / 2 - endText[2].Length / 2;
                Console.WriteLine("Wynik: {0} - {1}\n", firstPlayerScore, secondPlayerScore);
            }

            Console.CursorLeft = Console.WindowWidth / 2 - endText[3].Length / 2;
            Console.WriteLine(endText[3]);

            Console.ReadKey();
            Thread.Sleep(500);
            Console.Clear();
        }

        static void MenuReturn()
        {
            string retstring = "Naciśnij dowolny klawisz, aby wrocić do menu...";

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

        static void MakeObstacles(int roundNumber)
        {
            Random random = new Random();

            for (int i = 1; i <= roundNumber+10; i++)
            {
                int randomRow = random.Next(3, Console.WindowHeight - 4);
                int randomColumn = random.Next(10, Console.WindowWidth - 13);

                isUsed[randomColumn, randomRow] = true;
                isUsed[randomColumn+1, randomRow] = true;
                isUsed[randomColumn, randomRow+1] = true;
                isUsed[randomColumn+1, randomRow+1] = true;
                WriteOnPosition(randomColumn, randomRow, '▓', ConsoleColor.Red);
                WriteOnPosition(randomColumn+1, randomRow, '▓', ConsoleColor.Red);
                WriteOnPosition(randomColumn, randomRow+1, '▓', ConsoleColor.Red);
                WriteOnPosition(randomColumn+1, randomRow+1, '▓', ConsoleColor.Red);
            }
        }

        static void ResetGame(ref int roundNumber)
        {
            isUsed = new bool[Console.WindowWidth, Console.WindowHeight];
            SetGameField();
            firstPlayerDirection = right;
            secondPlayerDirection = left;

            roundNumber++;

            string text = "Naciśnij dowolny klawisz, żeby zacząć od nowa...";
            Console.CursorLeft = Console.WindowWidth / 2 - text.Length / 2;
            Console.WriteLine(text);

            Console.ReadKey();
            Thread.Sleep(250);
            Console.Clear();

            MakeObstacles(roundNumber);
            MovePlayers();
        }

        static void WriteOnPosition(int x, int y, char ch, ConsoleColor color)
        {

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(ch);

            
        }

        static void WriteSnakeHead()
        {
            Position[] directions = new Position[]
                {
                    new Position(0, 1), // right
                    new Position(0, -1), // left
                    new Position(1, 0), // down
                    new Position(-1, 0), // up
                };

            Position firstSnakeHead = new Position(firstPlayerRow, firstPlayerColumn);
            Position firstPlayerNextDirection = directions[firstPlayerDirection];

            Position firstSnakeNewHead = new Position(firstSnakeHead.row + firstPlayerNextDirection.row,
                firstSnakeHead.col + firstPlayerNextDirection.col);

            Console.SetCursorPosition(firstSnakeNewHead.col, firstSnakeNewHead.row);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write('°');

            Position secondSnakeHead = new Position(secondPlayerRow, secondPlayerColumn);
            Position secondPlayerNextDirection = directions[secondPlayerDirection];

            Position secondSnakeNewHead = new Position(secondSnakeHead.row + secondPlayerNextDirection.row,
                secondSnakeHead.col + secondPlayerNextDirection.col);

            Console.SetCursorPosition(secondSnakeNewHead.col, secondSnakeNewHead.row);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write('°');
        }
        
    }
}
