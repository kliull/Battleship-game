using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Asn1;
using System.Net.Http.Headers;

namespace Battleship_game
{
    class Program
    {
        static int drawTop = 2;
        static int drawLeft = 0;
        const int maxGridSize = 10;
        const int minGridSize = 1;
        const int totalShipCount = 10;
        const string alphabet = "ABCDEFGHIJ";
        static int[,] firstPlayerGrid = new int[maxGridSize, maxGridSize];
        static int[,] secondPlayerGrid = new int[maxGridSize, maxGridSize];

        static void ShipArrangement(int[,] grid, string alphabet)
        {
            Dictionary<int, int> shipTypes = new Dictionary<int, int>()
            {
                [4] = 1,
                [3] = 2,
                [2] = 3,
                [1] = 4,
            };
            Console.WriteLine(
                "Давайте расставим корабли на вашем игровом поле. Всего в игре участвуют 4 типа кораблей:"
            );
            Console.WriteLine(
                "1 «четырёхпалубный» линкор, 2 «трёхпалубных» крейсера, 3 «двухпалубные» субмарины, 4 «однопалубныx» торпедныx катера"
            );
            Console.WriteLine(
                "Напишите клетки, на которых вы хотите расположить корабль(формат <А9>)."
            );
            string cellInputIsValid;
            while (true)
            {
                cellInputIsValid = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(cellInputIsValid))
                {
                    Console.WriteLine("Пустой ввод. Пожалуйста, введите клетку/и.");
                    continue;
                }
                break;
            }
            string[] cellsInput = cellInputIsValid.ToUpper().Split();
            int shipCount = 0;
            while (true)
            {
                int shipLen = cellsInput.Length;
                bool isCorrectLen = shipTypes.ContainsKey(shipLen);
                if (!isCorrectLen)
                {
                    Console.WriteLine("Вы написали слишком много или слишком мало клеток");
                }
                else if (shipTypes[shipLen] != 0)
                {
                    if (PositionCheck(cellsInput, grid, alphabet))
                    {
                        foreach (string cell in cellsInput)
                        {
                            grid[int.Parse(cell[1..]) - 1, alphabet.IndexOf(cell[0])] = 1;
                        }
                        shipCount += 1;
                        shipTypes[shipLen] -= 1;
                        Console.WriteLine("Отлично, ваш корабль установлен.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Невозможно поставить корабль. Измените выбранные клетки."
                        );
                    }
                }
                else if (shipCount < totalShipCount)
                {
                    Console.WriteLine(
                        $"Все корабли c количеством палуб {shipLen} уже расставлены. Попробуйте поставить другой корабль.\n"
                    );
                }
                if (shipCount == 10)
                {
                    Console.WriteLine("Вы расставили все корабли на своем поле!\n");
                    break;
                }
                Console.WriteLine(
                    "Напишите клетки, на которых вы хотите расположить корабль(формат <А9>).\n"
                );
                while (true)
                {
                    cellInputIsValid = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(cellInputIsValid))
                    {
                        Console.WriteLine("Пустой ввод. Пожалуйста, введите клетку/и.");
                        continue;
                    }
                    break;
                }
                cellsInput = cellInputIsValid.ToUpper().Split();
            }
        }

        static bool PositionCheck(string[] cellsInput, int[,] grid, string alphabet)
        {
            if (CellsValidationCheck(cellsInput, alphabet))
            {
                bool sameRow = true,
                    sameColumn = true;
                int baseRow = int.Parse(cellsInput[0][1..]) - 1,
                    baseColumn = alphabet.IndexOf(cellsInput[0][0]);
                foreach (string cell in cellsInput)
                {
                    int row = int.Parse(cell[1..]) - 1;
                    int place = alphabet.IndexOf(cell[0]);
                    if (!SurroundingsCheck(row, place, grid))
                    {
                        return false;
                    }
                    if (row != baseRow)
                        sameRow = false;
                    if (place != baseColumn)
                        sameColumn = false;
                }
                if ((sameRow || sameColumn) && isRowColumnCheck(cellsInput))
                    return true;
                else
                    Console.WriteLine("Некорректное расположение корабля.");
                return false;
            }
            else
            {
                Console.WriteLine(
                    "Некорректный ввод клетки/ок. Напоминаем, поле состоит из рядов A-J и строк 1-10. Формат ввода <A9>"
                );
                return false;
            }
        }

        static bool CellsValidationCheck(string[] cellsInput, string alphabet)
        {
            int shipLen = cellsInput.Length;
            int rightCell = 0;
            foreach (string cell in cellsInput)
            {
                if (alphabet.Contains(char.ToUpper(cell[0])))
                {
                    int digit;
                    bool isDigit = int.TryParse(cell[1..], out digit);
                    if (isDigit && minGridSize <= digit && digit <= maxGridSize)
                    {
                        rightCell += 1;
                    }
                }
            }
            if (rightCell == shipLen)
            {
                return true;
            }
            return false;
        }

        static bool isRowColumnCheck(string[] cellsInput)
        {
            var colsr = new List<int>();
            var colsc = new List<int>();
            foreach (string cell in cellsInput)
            {
                int row = int.Parse(cell[1..]) - 1;
                int column = alphabet.IndexOf(cell[0]);
                colsr.Add(row);
                colsc.Add(column);
            }
            colsr.Sort();
            colsc.Sort();
            for (int i = 1; i < colsr.Count; i++)
            {
                if ((colsr[i] != colsr[i - 1] + 1) && (colsc[i] != colsc[i - 1] + 1))
                    return false;
            }
            return true;
        }

        static bool SurroundingsCheck(int row, int place, int[,] grid)
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int r = row + dr;
                    int c = place + dc;
                    if (r < 0 || r >= maxGridSize || c < 0 || c >= maxGridSize)
                        continue;
                    if (grid[r, c] == 1)
                        return false;
                }
            }
            return true;
        }

        static bool AllDead(int[,] grid)
        {
            for (int i = 0; i < maxGridSize; i++)
            {
                for (int j = 0; j < maxGridSize; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static string HitCheck(string cell, int[,] enemyGrid)
        {
            int row = int.Parse(cell[1..]) - 1;
            int column = alphabet.IndexOf(cell[0]);
            if (enemyGrid[row, column] == 1)
            {
                enemyGrid[row, column] = 3;
                return "YES";
            }
            else if (enemyGrid[row, column] == 2)
            {
                return "AGAIN";
            }
            enemyGrid[row, column] = 2;
            return "NO";
        }

        static void PrintGridForXPlayer(int[,] playerGrid)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ResetColor();
            int[,] enemyGrid = object.ReferenceEquals(playerGrid, firstPlayerGrid)
                ? secondPlayerGrid
                : firstPlayerGrid;

            Console.SetCursorPosition(drawLeft, drawTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("        Мой флот                  Флот соперника");
            Console.ResetColor();
            Console.SetCursorPosition(drawLeft, drawTop + 1);

            Console.Write("   ");
            for (int i = 0; i < maxGridSize; i++)
            {
                Console.Write(alphabet[i] + " ");
            }
            Console.Write("        ");
            for (int i = 0; i < maxGridSize; i++)
            {
                Console.Write(alphabet[i] + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < maxGridSize; i++)
            {
                Console.Write((i + 1).ToString().PadLeft(2) + " ");
                for (int j = 0; j < maxGridSize; j++)
                {
                    int val = playerGrid[i, j];
                    if (val == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O ");
                    }
                    else if (val == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(". ");
                    }
                    else if (val == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                    }
                    else
                        Console.Write("  ");
                    Console.ResetColor();
                }

                Console.Write("     ");
                Console.Write((i + 1).ToString().PadLeft(2) + " ");
                for (int j = 0; j < maxGridSize; j++)
                {
                    int val = enemyGrid[i, j];
                    if (val == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(". ");
                    }
                    else if (val == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                    }
                    else
                        Console.Write("  ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        static void ClearLine()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(curLeft, curTop);
        }

        static void Game(int[,] playerGrid)
        {
            while (true)
            {
                PrintGridForXPlayer(playerGrid);
                Console.SetCursorPosition(drawLeft, drawTop + 15);
                Console.WriteLine("Сделайте выстрел!");
                ClearLine();
                string cell;
                while (true)
                {
                    Console.SetCursorPosition(drawLeft, drawTop + 16);
                    cell = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(cell))
                    {
                        Console.SetCursorPosition(drawLeft, drawTop + 14);
                        ClearLine();
                        Console.WriteLine("Пустой ввод. Пожалуйста, введите клетку/и.");
                        continue;
                    }
                    break;
                }
                cell = cell.ToUpper();
                int[,] enemyGrid = ReferenceEquals(playerGrid, firstPlayerGrid)
                    ? secondPlayerGrid
                    : firstPlayerGrid;
                if (CellsValidationCheck(cell.Split(), alphabet))
                {
                    string didHit = HitCheck(cell, enemyGrid);
                    if (didHit == "YES")
                    {
                        Console.SetCursorPosition(drawLeft, drawTop + 14);
                        ClearLine();
                        Console.WriteLine("Вы попали! У вас дополнительный ход");
                    }
                    else if (didHit == "NO")
                    {
                        Console.SetCursorPosition(drawLeft, drawTop + 14);
                        ClearLine();
                        Console.WriteLine("Вы промахнулись:(. Сейчас ход другого игрока");
                        break;
                    }
                    else
                    {
                        Console.SetCursorPosition(drawLeft, drawTop + 14);
                        ClearLine();
                        Console.WriteLine("Вы уже били в эту клетку. Попробуйте снова");
                    }
                }
                else
                {
                    Console.SetCursorPosition(drawLeft, drawTop + 14);
                    ClearLine();
                    Console.WriteLine(
                        "Некорректный ввод клетки. Напоминаем, поле состоит из рядов A-J и строк 1-10. Формат ввода <A9>"
                    );
                }
            }
            return;
        }

        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в игру 'Морской бой'!");
            Console.WriteLine("Попробуйте победить, уничтожив все корабли соперника.\n");
            Console.WriteLine("Запускаю расстановку кораблей на поле 1 игрока:");

            ShipArrangement(firstPlayerGrid, alphabet);
            Console.WriteLine("Запускаю расстановку кораблей на поле 2 игрока:");

            ShipArrangement(secondPlayerGrid, alphabet);
            Console.Clear();

            while (true)
            {
                Console.SetCursorPosition(drawLeft, drawTop - 2);
                Console.WriteLine("Ход первого игрока.");
                Game(firstPlayerGrid);
                if (AllDead(secondPlayerGrid))
                {
                    Console.WriteLine("Игрок 1, Вы победили, поздравляем!!!");
                    break;
                }
                ClearLine();
                Console.SetCursorPosition(drawLeft, drawTop - 2);
                Console.WriteLine("Ход второго игрока.");
                Game(secondPlayerGrid);
                if (AllDead(firstPlayerGrid))
                {
                    Console.WriteLine("Игрок 2, Вы победили, поздравляем!!!");
                    break;
                }
            }
        }
    }
}
