using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace lab4
{
    class Program
    {
        static void Main()
        {
            List<string> patterns = new List<string> { @"(a|b)(c|d)E+G?", @"P(Q|R|S)T(UV|W|X)*Z+", @"1(0|1)*2(3|4){5}36" };
            
            Console.WriteLine("REGEX GENERATOR:");
            Console.WriteLine("----------------");
            
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine($"Pattern {i+1}: {patterns[i]}");
                
                RegexParser regexParser = new RegexParser();
                RegexGenerator regexGenerator = new RegexGenerator(regexParser);

                // Generate example combinations
                List<string> validCombinations = regexGenerator.GenerateValidCombinations(patterns[i]);
                
                Console.WriteLine($"Generated valid combinations:");
                foreach (var combo in validCombinations)
                {
                    Console.WriteLine($" - {combo}");
                }
                
                // Verify generated combinations are valid
                Regex regex = new Regex($"^{patterns[i]}$", RegexOptions.Compiled);
                bool allValid = validCombinations.All(regex.IsMatch);
                Console.WriteLine($"All combinations valid: {allValid}");
                
                Console.WriteLine();
                
                // Show processing sequence
                Console.WriteLine($"Processing sequence for pattern {i+1}:");
                RegexNode rootNode = regexParser.ParseRegex(patterns[i]);
                RegexTreePrinter.Print(rootNode, 0);
                
                Console.WriteLine("\n");
            }
        }
    }
}