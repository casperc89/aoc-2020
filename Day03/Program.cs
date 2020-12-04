using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DayThree
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            long treeCount = 1;
            treeCount *= TraverseMap(input, 1, 1);
            treeCount *= TraverseMap(input, 3, 1);
            treeCount *= TraverseMap(input, 5, 1);
            treeCount *= TraverseMap(input, 7, 1);
            treeCount *= TraverseMap(input, 1, 2);

            Console.WriteLine("----------------");
            Console.WriteLine(treeCount);

        }

        private static int TraverseMap(string[] input, int right, int down)
        {
            int treeCount = 0;
            var map = ParseTreeMap(input);
            int mapRepeats = map.Count * right;

            int curRight = right;
            int curDown = down;

            // First repeat the map to make sure we can traverse to bottom of the map
            foreach (var item in map)
            {
                var arr = item.Value.ToArray();

                for (int i = 0; i < mapRepeats; i++)
                {
                    item.Value.AddRange(arr);
                }
            }

            // Then traverse map
            for (int row = 0; row < map.Count; row++)
            {
                if(row < curDown)
                {
                    continue;
                }

                var item = map[row];
                var coord = item[curRight];
                if (coord.Position == MapPosition.Tree)
                    treeCount++;

                curRight += right;
                curDown += down;
            }

            Console.WriteLine(treeCount);
            return treeCount;
        }

        static IDictionary<int, List<MapCoord>> ParseTreeMap(string[] map)
        {
            var tree = new Dictionary<int, List<MapCoord>>();

            for (int row = 0; row < map.Length; row++)
            {

                var line = map[row];
                var coords = new MapCoord[line.Length];

                for (int col = 0; col < line.Length; col++)
                {
                    var pos = (line[col]) switch
                    {
                        '.' => MapPosition.Square,
                        '#' => MapPosition.Tree,
                        _ => MapPosition.Unknown,
                    };

                    coords[col] = new MapCoord() { RowIndex = row, ColIndex = col, Position = pos };
                }

                tree.Add(row, coords.ToList());
            }

            return tree;
        }

        class MapCoord
        {
            public int RowIndex { get; set; }

            public int ColIndex { get; set; }

            public MapPosition Position { get; set; }
        }

        enum MapPosition
        {
            Unknown,
            Square,
            Tree
        }
    }
}
