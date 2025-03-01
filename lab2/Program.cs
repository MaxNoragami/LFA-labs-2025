namespace lab2
{
    class Program
    {
        static void Main()
        {
            // Variant 1
            HashSet<string> q = new HashSet<string>() {"q0", "q1", "q2", "q3"};
            HashSet<char> sigma = new HashSet<char>() {'a', 'b', 'c'};
            Dictionary<(string, char), HashSet<string>> delta = new Dictionary<(string, char), HashSet<string>> {
                { ("q0", 'a'), new HashSet<string>() {"q0", "q1"} },
                { ("q1", 'c'), new HashSet<string>() {"q1"} },
                { ("q1", 'b'), new HashSet<string>() {"q2"} },
                { ("q2", 'b'), new HashSet<string>() {"q3"} },
                { ("q3", 'a'), new HashSet<string>() {"q1"} },
            };
            HashSet<string> qF = new HashSet<string>() {"q2"};
            string q0 = "q0";

            // Initializing the FA
            FiniteAutomaton finiteAutomaton = new FiniteAutomaton(q, sigma, delta, q0, qF);
            
            Console.WriteLine(finiteAutomaton.ToString());

            // Creating a grammar instance from the FA
            Grammar grammar = finiteAutomaton.ToRegularGrammar();
            Console.WriteLine(grammar.ToString());

            Console.WriteLine(finiteAutomaton.IsDFA());
    
        }
    }
}