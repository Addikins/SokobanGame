using System;
using System.IO;

namespace SokobanGame
{
    public class LevelClass
    {
        public string Level { get; set; }

    }
    public class Movement
    {
        //can indicator/player move
        //move methods

    }
    class Program
    {
        // Flavor text needs better placement, continues to get cleared early
        // Need an option to restart level not only the option to exit to main menu
        static void Main(string[] args)
        {
            bool playAgain = true;
            while (playAgain == true)
            {
                Console.WriteLine("Level Selection");
                var Level = File.ReadAllLines($"LevelSelection.txt");
                PrintLevel(Level);
                string levelSelection = LevelSelection(Level);

                Level = File.ReadAllLines($"SokobanLevel{levelSelection}.txt");
                PrintLevel(Level);
                GameLoop(Level);
                Console.ReadKey();
                playAgain = GetYesNo(Level);
            }
        }
        private static string MenuSelection(string[] Level)
        {
            bool choiceMade = false;
            while (choiceMade == false)
            {
                var indicatorPosition = FindIndicator(Level);
                Point target = null;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        target = new Point(indicatorPosition.X, indicatorPosition.Y - 1);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        target = new Point(indicatorPosition.X, indicatorPosition.Y + 1);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        string LevelLine = Level[indicatorPosition.Y];
                        return $"{LevelLine}";
                    default:
                        target = null;
                        break;
                }
                if (target != null)
                {
                    MoveIndicator(Level, indicatorPosition, target);
                }
                PrintLevel(Level);
            }
            return "Error";
        }
        private static void GameLoop(string[] Level)
        {
            bool win = false;
            while (win == false)
            {
                var playerPosition = FindPlayer(Level);
                Point target;
                Point boxTarget;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        // Console.WriteLine("Up");
                        target = new Point(playerPosition.X, playerPosition.Y - 1);
                        boxTarget = new Point(playerPosition.X, playerPosition.Y - 2);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        // Console.WriteLine("Right");
                        target = new Point(playerPosition.X + 1, playerPosition.Y);
                        boxTarget = new Point(playerPosition.X + 2, playerPosition.Y);

                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        // Console.WriteLine("Down");
                        target = new Point(playerPosition.X, playerPosition.Y + 1);
                        boxTarget = new Point(playerPosition.X, playerPosition.Y + 2);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        // Console.WriteLine("Left");
                        target = new Point(playerPosition.X - 1, playerPosition.Y);
                        boxTarget = new Point(playerPosition.X - 2, playerPosition.Y);
                        break;
                    case ConsoleKey.Escape:
                        // Exits game
                        Console.WriteLine("Leaving Game");
                        target = null;
                        boxTarget = null;
                        return;
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        //Restarts level
                        // Level = File.ReadAllLines($"SokobanLevel{levelSelection}.txt");
                        Console.WriteLine("Restarting level");
                        target = null;
                        boxTarget = null;
                        break;
                    default:
                        target = null;
                        boxTarget = null;
                        break;
                }
                if (target != null)
                {
                    MovePlayer(Level, playerPosition, target, boxTarget);
                }
                /*  if (win)
                  {
                      flavorText = "\nYou've won!\n";
                  }
                  else
                  {
                      flavorText = "/nPush the boxes into the goals!\n";
                  } */
                PrintLevel(Level);
                win = FindBoxes(Level);
            }
        }
        private static void MoveIndicator(string[] Level, Point indicatorPosition, Point target)
        {
            char targetCharacter = Level[target.Y][target.X];
            if (CanIndicatorMove(targetCharacter, Level))
            {
                Level[target.Y] = ReplaceTile(Level, target, ">");
                Level[indicatorPosition.Y] = ReplaceTile(Level, indicatorPosition, " ");
            }
        }
        private static void MovePlayer(string[] Level, Point playerPosition, Point target, Point boxTarget)
        {
            char targetCharacter = Level[target.Y][target.X];
            if (CanPlayerMove(targetCharacter, boxTarget, Level))
            {
                if (targetCharacter == ' ')
                {
                    Level[target.Y] = ReplaceTile(Level, target, "@");
                }
                else if (targetCharacter == '.')
                {
                    Level[target.Y] = ReplaceTile(Level, target, "+");
                }
                else if (targetCharacter == '$')
                {
                    Level[target.Y] = ReplaceTile(Level, target, "@");
                    if (Level[boxTarget.Y][boxTarget.X] == '.')
                    {
                        Level[boxTarget.Y] = ReplaceTile(Level, boxTarget, "*");
                    }
                    else
                    {
                        Level[boxTarget.Y] = ReplaceTile(Level, boxTarget, "$");
                    }
                }
                else if (targetCharacter == '*')
                {
                    Level[target.Y] = ReplaceTile(Level, target, "+");
                    if (Level[boxTarget.Y][boxTarget.X] == '.')
                    {
                        Level[boxTarget.Y] = ReplaceTile(Level, boxTarget, "*");
                    }
                    else
                    {
                        Level[boxTarget.Y] = ReplaceTile(Level, boxTarget, "$");
                    }
                }


                if (Level[playerPosition.Y][playerPosition.X] == '+')
                {
                    Level[playerPosition.Y] = ReplaceTile(Level, playerPosition, ".");
                }
                else
                {
                    Level[playerPosition.Y] = ReplaceTile(Level, playerPosition, " ");
                }
            }
        }
        private static bool CanIndicatorMove(char targetCharacter, string[] Level)
        {
            if (targetCharacter != ' ')
            {
                return false;
            }
            return true;
        }
        private static bool CanPlayerMove(char targetCharacter, Point boxTarget, string[] Level)
        {
            if (targetCharacter == '$' || targetCharacter == '*')
            {
                char boxTargetCharacter = Level[boxTarget.Y][boxTarget.X];
                return boxTargetCharacter == ' ' || boxTargetCharacter == '.';
            }
            return targetCharacter != '#';
        }

        private static string ReplaceTile(string[] Level, Point target, string tileCharacter)
        {
            return Level[target.Y].Remove(target.X, 1).Insert(target.X, tileCharacter);
        }

        private static void PrintLevel(string[] fileContent /*,string flavorText*/)
        {
            Console.Clear();
            Console.WriteLine("Press Esc to exit\n");
            //  Console.WriteLine(flavorText);
            for (int i = 0; i < fileContent.Length; i++)
            {
                Console.WriteLine(fileContent[i]);
            }
        }

        private static bool FindBoxes(string[] fileContent)
        {
            for (int y = 0; y < fileContent.Length; y++)
            {
                for (int x = 0; x < fileContent[y].Length; x++)
                {
                    Point point = new Point(x, y);
                    var character = fileContent[y][x];
                    if (character == '$')
                    {
                        return false;
                    }
                }
            }
            Console.WriteLine("\nYou've won!\n");
            return true;
        }
        private static Point FindIndicator(string[] fileContent)
        {
            for (int y = 0; y < fileContent.Length; y++)
            {
                for (int x = 0; x < fileContent[y].Length; x++)
                {
                    Point point = new Point(x, y);
                    var character = fileContent[y][x];
                    if (character == '>')
                    {
                        return point;
                    }
                }
            }
            return null;
        }
        private static Point FindPlayer(string[] fileContent)
        {
            for (int y = 0; y < fileContent.Length; y++)
            {
                for (int x = 0; x < fileContent[y].Length; x++)
                {
                    Point point = new Point(x, y);
                    var character = fileContent[y][x];
                    if (character == '@' || character == '+')
                    {
                        return point;
                    }
                }
            }
            return null;
        }
        private static string LevelSelection(string[] Level)
        {
            string answer = MenuSelection(Level);
            if (answer.Contains("1"))
            {
                return "1";
            }
            else if (answer.Contains("2"))
            {
                return "2";
            }
            else if (answer.Contains("3"))
            {
                return "3";
            }
            else if (answer.Contains("4"))
            {
                return "4";
            }
           /* switch (answer)
            {
                case "1":
                    return "1";
                case "2":
                    return "2";
                case "3":
                    return "3";
                case "4":
                    return "4";
                default:
                    Console.WriteLine($"ERROR");
                    answer = '0';
                    break;
            }*/
            return "Error"; 
        }
        private static bool GetYesNo(string[] Level)
        {
            while (true)
            {
                Level = File.ReadAllLines("YesNo.txt");
                PrintLevel(Level);
                Console.WriteLine("Would you like to start a new game?\n");
                string answer = MenuSelection(Level);
                if (answer.ToLower().Contains('y'))
                {
                    return true;
                }
                else if (answer.ToLower().Contains('n'))
                {
                    return false;
                }
               /* switch (answerChoice)
                {
                    case 'y':
                        return true;
                    case 'n':
                        return false;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                } */
            }
        }
    }
}
