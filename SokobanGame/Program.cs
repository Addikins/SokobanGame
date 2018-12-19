using System;
using System.IO;

namespace SokobanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var Level = File.ReadAllLines("SokobanLevel1.txt");
            PrintLevel(Level);
            while (true)
            {
                var playerPosition = FindPlayer(Level);
                Console.WriteLine(playerPosition);
                Point target;
                Point boxTarget;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Console.WriteLine("Up");
                        target = new Point(playerPosition.X, playerPosition.Y - 1);
                        boxTarget = new Point(playerPosition.X, playerPosition.Y - 2);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Console.WriteLine("Right");
                        target = new Point(playerPosition.X + 1, playerPosition.Y);
                        boxTarget = new Point(playerPosition.X + 2, playerPosition.Y);

                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Console.WriteLine("Down");
                        target = new Point(playerPosition.X, playerPosition.Y + 1);
                        boxTarget = new Point(playerPosition.X, playerPosition.Y + 2);

                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Console.WriteLine("Left");
                        target = new Point(playerPosition.X - 1, playerPosition.Y);
                        boxTarget = new Point(playerPosition.X - 2, playerPosition.Y);
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
                PrintLevel(Level);
                Console.WriteLine(target);
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

        private static void PrintLevel(string[] fileContent)
        {
            Console.Clear();
            for (int i = 0; i < fileContent.Length; i++)
            {
                Console.WriteLine(fileContent[i]);
            }
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
    }
}
