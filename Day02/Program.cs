using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DayTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => PasswordValidator.Parse(x))
                .ToArray();

            Console.WriteLine($"Valid: {input.Where(x => x.IsValidA()).Count()}; Invalid: {input.Where(x => !x.IsValidA()).Count()}");
            Console.WriteLine($"Valid: {input.Where(x => x.IsValidB()).Count()}; Invalid: {input.Where(x => !x.IsValidB()).Count()}");
        }

        class PasswordValidator
        {
            private readonly int minLength;
            private readonly int maxLength;
            private readonly char letter;
            private readonly string password;

            public PasswordValidator(int minLength, int maxLength, char letter, string password)
            {
                this.minLength = minLength;
                this.maxLength = maxLength;
                this.letter = letter;
                this.password = password;
            }

            public static PasswordValidator Parse(string input)
            {
                var line = input.Split(" ");
                var lengthSplit = line[0].Split("-");

                return new PasswordValidator(int.Parse(lengthSplit[0]), int.Parse(lengthSplit[1]), line[1][0], line[2]);
            }

            public bool IsValidA()
            {
                var chars = new ConcurrentDictionary<char, int>();

                for (int i = 0; i < password.Length; i++)
                {
                    chars.AddOrUpdate(password[i], 1, (_, count) => count + 1);
                }

                int charCount = 0;
                if (chars.ContainsKey(letter))
                {
                    charCount = chars[letter];
                }

                return charCount >= minLength && charCount <= maxLength;
            }

            public bool IsValidB()
            {
                var posOneIdx = minLength - 1;
                var posTwoIdx = maxLength - 1;

                bool letterOnPosOne = password[posOneIdx].Equals(letter);
                bool letterOnPosTwo = password[posTwoIdx].Equals(letter);

                if ((letterOnPosOne && letterOnPosTwo) || (!letterOnPosOne && !letterOnPosTwo))
                    return false;

                return true;
            }

            public override string ToString()
            {
                var input = $"{minLength}-{maxLength} {letter}: {password}";

                return IsValidB() ? $"OK: {input}" : $"NOK: {input}";
            }
        }
    }
}
