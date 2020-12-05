using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
               .Where(x => !string.IsNullOrWhiteSpace(x))
               .ToArray();

            var testSet = new Dictionary<string, BoardingPass>()
            {
                ["FBFBBFFRLR"] = new BoardingPass(44, 5, 357),
                ["BFFFBBFRRR"] = new BoardingPass(70, 7, 567),
                ["FFFBBBFRRR"] = new BoardingPass(14, 7, 119),
                ["BBFFBBFRLL"] = new BoardingPass(102, 4, 820),
            };

            foreach (var expected in testSet)
            {
                var actual = BoardingPass.Parse(expected.Key);

                Console.WriteLine($"Expected: {expected.Value.ToString()}");
                Console.WriteLine($"Actual: {actual.ToString()}");

            }

            Console.WriteLine("---------------");

            var passes = input
                .Select(line => BoardingPass.Parse(line))
                .ToList();

            int max = passes.Max(x => x.SeatId);

            Console.WriteLine($"Highest seat ID: {max}");

            var filledSeats = passes.Select(x => x.SeatId);
            var allSeats = AllPossibleBoardingPasses().Select(x => x.SeatId);
            var remainingSeats = allSeats.Except(filledSeats);
            
            var possibleSeats = new List<int>();

            for (int i = 0; i < passes.Count; i++)
            {
                for (int j = 0; j < passes.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (passes[i].SeatId - passes[j].SeatId == 2)
                        possibleSeats.Add(passes[i].SeatId + 1);
                }
            }

            var seats = possibleSeats.Intersect(remainingSeats);
            foreach (var item in seats)
            {
                if (item < max)
                    Console.WriteLine($"My seat ID: {item}");
            }
        }

        private static IEnumerable<BoardingPass> AllPossibleBoardingPasses()
        {
            var rows = CreateCombinations(7, 'F', 'B').ToArray();
            var cols = CreateCombinations(3, 'R', 'L').ToArray();
            var combis = new List<string>();

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < cols.Length; j++)
                {
                    combis.Add(rows[i] + cols[j]);
                }
            }

            return combis.Select(x => BoardingPass.Parse(x));
        }

        private static IEnumerable<string> CreateCombinations(int length, char zero, char one)
        {
            var numberOfCombinations = (int)Math.Pow(2, length);

            for (int i = 0; i < numberOfCombinations; i++)
            {
                char[] binary = Convert.ToString(i, 2).PadLeft(length, '0').ToCharArray();

                for (int j = 0; j < binary.Length; j++)
                {
                    if (binary[j].Equals('0'))
                        binary[j] = zero;
                    else
                        binary[j] = one;
                }

                yield return new string(binary);
            }
        }

        class BoardingPass
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int SeatId { get; set; }

            public BoardingPass(int row, int col, int seatId)
            {
                Row = row;
                Col = col;
                SeatId = seatId;
            }

            public static BoardingPass Parse(string input)
            {
                var row = input.Substring(0, 7);
                var col = input.Substring(7);

                int rowNumber = Parse(row, 0, 127, 'F', 'B');
                int colNumber = Parse(col, 0, 7, 'L', 'R');
                int seatId = (rowNumber * 8) + colNumber;

                return new BoardingPass(rowNumber, colNumber, seatId);
            }

            private static int Parse(string input, int lowerRange, int upperRange, char lowerHalf, char upperHalf)
            {
                var letter = input[0];
                bool last = input.Length == 1;
                int middle = (upperRange - lowerRange) / 2 + lowerRange;

                if (letter.Equals(lowerHalf))
                    return last ? lowerRange : Parse(input.Substring(1), lowerRange, middle, lowerHalf, upperHalf);
                else if (letter.Equals(upperHalf))
                    return last ? upperRange : Parse(input.Substring(1), middle + 1, upperRange, lowerHalf, upperHalf);
                else
                    throw new InvalidOperationException($"Invalid letter in input: {letter}. Allowed: {lowerHalf}|{upperHalf}");

            }

            public override string ToString()
            {
                return $"Row: {Row} - Col: {Col} - SeatID: {SeatId}";
            }
        }
    }
}
