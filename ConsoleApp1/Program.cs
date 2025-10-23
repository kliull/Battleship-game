using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Battleship_game
{
    class Program
    {
        const int gridSize = 10;
        const int totalShipCount = 10;
        const string alphabet = "ABCDEFGHIJ";
        static int[,] firstPlayerGrid = new int[gridSize, gridSize];
        static int[,] secondPlayerGrid = new int[gridSize, gridSize];

        static void ShipArrangement(int[,] grid, string alphabet)
        {
            Dictionary<int, int> shipTypes = new Dictionary<int, int>()
            {
                [4] = 1,
                [2] = 2,
                [3] = 3,
                [1] = 4,
            };
            Console.WriteLine(
                "Давайте расставим корабли на вашем игровом поле. Всего в игре участвуют 4 типа кораблей:"
            );
            Console.WriteLine(
                "1 «четырёхпалубный» линкор, 2 «трёхпалубных» крейсера, 3 «двухпалубные» субмарины, 4 «однопалубныx» торпедныx катера"
            );
            Console.WriteLine(
                "Напишите клетки, на котрых вы хотите расположить корабль(формат <А9>)."
            );
            string[] cellsInput = Console.ReadLine().Split();
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
                    if (PositionCheck(cellsInput, shipTypes, grid, alphabet))
                    {
                        foreach (string cell in cellsInput)
                        {
                            grid[int.Parse(cell[1..]) - 1, alphabet.IndexOf(cell[0])] = 1; // установили корабл.
                        }
                        shipCount += 1;
                        shipTypes[shipLen] -= 1;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Невозможно поставить корабль. Измените выбранные клетки."
                        );
                    }
                }
                else if (shipCount < totalShipCount)
                {
                    Console.WriteLine(
                        $"Все корабли c количеством палуб {shipLen} уже расставлены. Попробуйте поставить другой корабль."
                    );
                }
                else
                {
                    Console.WriteLine("Вы расставили все корабли на своем поле!");
                    break;
                }
                Console.WriteLine(
                    "Напишите клетки, на котрых вы хотите расположить корабль(формат <А9>)."
                );
                cellsInput = Console.ReadLine().Split();
            }
        }

        static bool PositionCheck(
            string[] cellsInput,
            Dictionary<int, int> shipTypes,
            int[,] grid,
            string alphabet
        )
        {
            int shipLen = cellsInput.Length;
            int rightCell = 0;
            foreach (string cell in cellsInput)
            {
                if (alphabet.Contains(char.ToUpper(cell[0])))
                {
                    int digit;
                    bool isDigit = int.TryParse(cell[1..], out digit);
                    if (isDigit && 1 <= digit && digit <= gridSize)
                    {
                        rightCell += 1;
                    }
                }
            }
            int countSurroundingsCheck = 0;
            if (rightCell == shipLen)
            {
                bool sameRow = true,
                    sameColumn = true;
                int baseRow = int.Parse(cellsInput[0][1..]) - 1,
                    baseColumn = alphabet.IndexOf(cellsInput[0][0]);
                foreach (string cell in cellsInput)
                {
                    int row = int.Parse(cell[1..]) - 1;
                    int place = alphabet.IndexOf(cell[0]);
                    if (SurroundingsCheck(row, place, grid))
                    {
                        countSurroundingsCheck += 1;
                    }
                    if (row != baseRow)
                        sameRow = false;
                    if (place != baseColumn)
                        sameColumn = false;
                }
                if (countSurroundingsCheck == shipLen)
                {
                    if ((sameRow || sameColumn) && isRowColumnCheck(cellsInput))
                        return true;
                }
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
            if (
                grid[row, place] == 1
                || grid[row - 1, place] == 1
                || grid[row - 1, place - 1] == 1
                || grid[row - 1, place + 1] == 1
                || grid[row + 1, place - 1] == 1
                || grid[row + 1, place + 1] == 1
                || grid[row, place - 1] == 1
                || grid[row + 1, place] == 1
                || grid[row, place + 1] == 1
            )
                return false;
            return true;
        }

        static void HitCheck()
        {
            // проверка попадания в корабль
        }

        static void PrintGrindForFirstPlayer(int[,] playerMap) { }

        static void Main()
        {
            bool isFinish = false;
            Console.WriteLine("Добро пожаловать в игру 'Морской бой'!");
            Console.WriteLine("Попробуйте победить, уничтожив все корабли соперника.\n");
            Console.WriteLine("Запускаю расстановку кораблей на поле 1 игрока:");
            ShipArrangement(firstPlayerGrid, alphabet);
            Console.WriteLine("Запускаю расстановку кораблей на поле 2 игрока:");
            ShipArrangement(secondPlayerGrid, alphabet);
            // сама играаа
            while (true)
            {
                Console.WriteLine("Ход за первым игроком. Введите клетку");

                if (isFinish)
                {
                    break;
                    // ...
                }
            }
        }
    }
}
