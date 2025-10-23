using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Battleship_game
{
    class Program
    {
        const int gridSize = 10;
        const string alphabet = "ABCDEFGHIJ";
        static int[,] firstPlayerGrid = new int[gridSize, gridSize];
        static int[,] secondPlayerGrid = new int[gridSize, gridSize];

        static void ship_arrangement(int[,] grid, string alphabet)
        {
            Dictionary<int, int> shipTypes = new Dictionary<int, int>()
            {
                [4] = 1,
                [2] = 2,
                [3] = 3,
                [1] = 4,
            };
            Console.WriteLine("Давайте расставим корабли на вашем игровом поле. Всего в игре участвуют 4 типа кораблей:");
            Console.WriteLine("1 «четырёхпалубный» линкор, 2 «трёхпалубных» крейсера, 3 «двухпалубные» субмарины, 4 «однопалубныx» торпедныx катера");
            Console.WriteLine("Напишите клетки, на котрых вы хотите расположить корабль(формат <А9>).");
            string[] cellsInput = Console.ReadLine().Split();
            int shipCount = 0;
            while (true)
            {
                int shipLen = cellsInput.Length;
                if (shipTypes[shipLen] != 0)
                {
                    if (position_check(cellsInput, shipTypes))
                    {
                        foreach (string cell in cellsInput)
                        {
                            grid[int.Parse(cell[1..]) - 1, alphabet.IndexOf(cell[0])] = 1; // установили корабл.
                        }
                        shipCount += 1;
                    }
                    else
                    {
                        Console.WriteLine($"Невозможно поставить корабль. Измените выбранные клетки.");
                    }
                }
                else if (shipCount < 10)
                {
                    Console.WriteLine($"Все корабли c количеством палуб {shipLen} уже расставлены. Попробуйте поставить другой корабль.");

                }
                else
                {
                    Console.WriteLine("Вы расставили все корабли на своем поле!");
                    break;
                }
                Console.WriteLine("Напишите клетки, на котрых вы хотите расположить корабль(форрмат <А9>).");
                cellsInput = Console.ReadLine().Split();
            }
        }
        static bool position_check(string[] cellsInput, Dictionary<int, int> shipTypes)
        {
            int shipLen = cellsInput.Length;
            int rightCell = 0;
            foreach (string cell in cellsInput)
            {
                if (alphabet.Contains(cell[0]))
                {
                    int digit;
                    bool isDigit = int.TryParse(cell[1..], out digit);
                    if (isDigit && 1 <= digit && digit <= 10)
                    {
                        rightCell += 1;
                    }
                }
            }
            if (rightCell == shipLen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void hit_check()
        {
            // проверка попадания в корабль
        }
        static void printMap(int[,] playerMap)
        {
        }
        static void Main()
        {

            Console.WriteLine("Добро пожаловать в игру 'Морской бой'!");
            Console.WriteLine("Попробуйте победить, уничтожив все корабли соперника.\n");
            Console.WriteLine("Запускаю расстановку кораблей на поле 1 игрока:");
            ship_arrangement(firstPlayerGrid, alphabet);
            Console.WriteLine("Запускаю расстановку кораблей на поле 2 игрока:");
            ship_arrangement(secondPlayerGrid, alphabet);
            // Игровой цикл


        }
    }
}
