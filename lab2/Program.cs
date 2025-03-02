namespace lab2
{
    class Program
    {
        static void Main()
        {
            // Variant 1
            HashSet<HashSet<string>> q = new HashSet<HashSet<string>>() {
                new HashSet<string>(){"q0"}, 
                new HashSet<string>(){"q1"}, 
                new HashSet<string>(){"q2"}, 
                new HashSet<string>(){"q3"}
            };
            HashSet<char> sigma = new HashSet<char>() {'a', 'b', 'c'};
            Dictionary<(HashSet<string>, char), HashSet<string>> delta = new Dictionary<(HashSet<string>, char), HashSet<string>> {
                { (new HashSet<string>(){"q0"}, 'a'), new HashSet<string>() {"q0", "q1"} },
                { (new HashSet<string>(){"q1"}, 'c'), new HashSet<string>() {"q1"} },
                { (new HashSet<string>(){"q1"}, 'b'), new HashSet<string>() {"q2"} },
                { (new HashSet<string>(){"q2"}, 'b'), new HashSet<string>() {"q3"} },
                { (new HashSet<string>(){"q3"}, 'a'), new HashSet<string>() {"q1"} },
            };
            HashSet<HashSet<string>> qF = new HashSet<HashSet<string>>() {new HashSet<string>(){"q2"}};
            string q0 = "q0";

            // Initializing the FA
            FiniteAutomaton finiteAutomaton = new FiniteAutomaton(q, sigma, delta, q0, qF);
            
            Console.WriteLine(finiteAutomaton.ToString());

            // Creating a grammar instance from the FA
            Grammar grammar = finiteAutomaton.ToRegularGrammar();
            Console.WriteLine(grammar.ToString());

            Console.WriteLine(finiteAutomaton.IsDFA());

            FiniteAutomaton dfa = finiteAutomaton.ToDFA();
            Console.WriteLine(dfa.ToString());
    
        }
    }
}