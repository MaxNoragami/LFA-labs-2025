using System.Text.RegularExpressions;

namespace lab4
{
    class Program
    {
        static void Main()
        {
            // Variant 1 -- Alexei Maxim -- FAF-232
            List<string> patterns = new List<string> { @"(a|b)(c|d)E+G?", @"P(Q|R|S)T(UV|W|X)*Z+", @"1(0|1)*2(3|4){5}36" };
            
            Console.WriteLine("REGEX GENERATOR:");
            Console.WriteLine("-------------------------");
            
            for (int i = 0; i < 1; i++)
            {
                int countOfGeneratedStrings = 0;
                Console.WriteLine($"Pattern {i+1}: {patterns[i]}");
                
                RegexParser regexParser = new RegexParser();
                RegexGenerator regexGenerator = new RegexGenerator(regexParser);

                // Generating example combinations
                HashSet<string> validCombinations = regexGenerator.GenerateValidCombinations(patterns[i]).ToHashSet();
                
                Console.WriteLine($"Generated valid combinations:");
                foreach (var combo in validCombinations)
                {
                    Console.WriteLine($" - {combo}");
                    countOfGeneratedStrings++;
                }
                
                // Verifying generated combinations are valid
                Regex regex = new Regex($"^{patterns[i]}$", RegexOptions.Compiled);
                bool allValid = validCombinations.All(regex.IsMatch);
                Console.WriteLine($"All combinations valid: {allValid}");
                Console.WriteLine($"Total amount of generated symbols: {countOfGeneratedStrings}");
                
                Console.WriteLine();
                
                // Showing parse sequence
                Console.WriteLine($"Processing sequence for pattern {i+1}:");
                RegexNode rootNode = regexParser.ParseRegex(patterns[i]);
                RegexTreePrinter.Print(rootNode, 0);
                
                Console.WriteLine("\n");
            }
        }
    }
}