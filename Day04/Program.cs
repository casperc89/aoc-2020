using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DayFour
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
               .ToArray();

            var passports = ParsePassports(input).ToArray();

            Console.WriteLine(passports.Count(p => p.IsValid()));
            Console.WriteLine(passports.Count(p => p.IsStrictlyValid()));
        }

        private static IEnumerable<Passport> ParsePassports(string[] input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return Passport.Parse(builder.ToString().Trim());
                    builder.Clear();
                    continue;
                }

                builder.Append(" " + line);
            }
        }

        class Passport
        {
            private static readonly string[] _acceptedFields = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            private IDictionary<string, string> _fields = new Dictionary<string, string>();

            public Passport(IDictionary<string, string> fields)
            {
                if (fields.Keys.Any(x => !_acceptedFields.Contains(x)))
                    throw new InvalidOperationException("Passport contains invalid fields");

                _fields = fields;
            }

            public bool IsValid()
            {
                if (_fields.Count == 8 || _fields.Count == 7 && !_fields.ContainsKey("cid"))
                    return true;
                else
                    return false;
            }

            public bool IsStrictlyValid()
            {
                var valid = IsValid();

                if (!valid)
                    return false;

                return
                    IsValidYear(_fields["byr"], 1920, 2002) &&
                    IsValidYear(_fields["iyr"], 2010, 2020) &&
                    IsValidYear(_fields["eyr"], 2020, 2030) &&
                    IsValidHeight(_fields["hgt"]) &&
                    IsValidHairColor(_fields["hcl"]) &&
                    IsValidEyeColor(_fields["ecl"]) &&
                    IsValidPassportId(_fields["pid"]);
            }

            private bool IsValidPassportId(string field)
            {
                var pattern = "^[0-9]{9}$";
                var regex = new Regex(pattern);
                var match = regex.IsMatch(field);

                return match;
            }

            private bool IsValidEyeColor(string field)
            {
                string[] valid = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                return valid.Contains(field);
            }

            private bool IsValidHairColor(string field)
            {
                var pattern = "^#[0-9a-f]{6}$";
                var regex = new Regex(pattern);
                var match = regex.IsMatch(field);

                return match;
            }

            private bool IsValidHeight(string field)
            {
                var pattern = "^(\\d{2,3})(cm|in)$";
                var regex = new Regex(pattern);

                var match = regex.Match(field);
                
                if(match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out int height);
                    var unit = match.Groups[2].Value;
                    switch (unit)
                    {
                        case "cm":
                            return height >= 150 && height <= 193;
                        case "in":
                            return height >= 59 && height <= 76;
                        default:
                            break;
                    }
                }

                return false;
            }

            private bool IsValidYear(string field, int min, int max)
            {
                return !string.IsNullOrEmpty(field) && int.TryParse(field, out int year) && year >= min && year <= max;
            }

            public static Passport Parse(string rawPassport)
            {
                var fields = rawPassport
                    .Split(" ")
                    .ToDictionary(x => x.Substring(0, x.IndexOf(":")), x => x.Substring(x.IndexOf(":") + 1));

                return new Passport(fields);
            }
        }
    }
}
