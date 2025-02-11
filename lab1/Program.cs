using System.Security.Cryptography;
using System.Text;

namespace lab1
{
    class Program
    {
        static void Main()
        {

            // Variant 1 - Alexei Maxim
            Console.WriteLine("Alexei Maxim,\nFAF-232,\nVariant 1\n");

            // My variant's grammar definition
            HashSet<string> vN = new HashSet<string>() {"S", "P", "Q"};
            HashSet<char> vT = new HashSet<char>() {'a', 'b', 'c', 'd', 'e', 'f'};
            Dictionary<string, List<string>> p = new Dictionary<string, List<string>>()
                {
                    {"S", new List<string>() {"aP", "bQ"}},
                    {"P", new List<string>() {"bP", "cP", "dQ", "e"}},
                    {"Q", new List<string>() {"eQ", "fQ", "a"}}
                };
            string s = "S";
            
            // Initializing the grammar & finiteAutomaton
            Grammar grammar = new Grammar(vN, vT, p, s);
            FiniteAutomaton finiteAutomaton = grammar.ToFiniteAutomaton();

            Console.WriteLine("My Variant's Grammar definition:");
            Console.WriteLine(grammar.ToString());

            // Generating the strings using the grammar definition
            Console.WriteLine("------- Strings from L(G) -------");
            for(int i = 0; i < 5; i++)
            {
                string generatedString = grammar.GenerateString();
                Console.WriteLine("\n#{0} Generated string: {1}\nAccepted by automaton: {2}", i + 1, generatedString, finiteAutomaton.StringBelongToLanguage(generatedString));
            }
            Console.WriteLine("\n---------------------------------\n");

            Console.WriteLine("My Variant's Finite Automaton definition:");
            Console.WriteLine(finiteAutomaton.ToString());


            // Checking some randomly generated strings if they can be obtained via the state transition from our Finite Automaton
            Console.WriteLine("------- Checking some random strings against FA -------");
            for(int i = 0; i < 5; i++)
            {
                string randomString = GenerateRandomString(vT);
                Console.WriteLine("\n#{0} Random string: {1}\nAccepted by automaton: {2}", i + 1, randomString, finiteAutomaton.StringBelongToLanguage(randomString));
            }
            Console.WriteLine("\n-------------------------------------------------------");

            
            
        }

        static string GenerateRandomString(HashSet<char> vT)
        {
            
            StringBuilder someString = new StringBuilder();
            for(int i = 0; i < RandomNumberGenerator.GetInt32(3, 10); i++)
            {
                someString.Append(vT.ElementAt(RandomNumberGenerator.GetInt32(0, vT.Count)));
            }
            return someString.ToString();
        } 
    }
}