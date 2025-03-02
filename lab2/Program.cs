using System.Net.Mail;

namespace lab2
{
    class Program
    {
        static void Main()
        {
            // Variant 1
            // Alexei Maxim, FAF-232
            
            // Task 2.
            // Possible States
            HashSet<HashSet<string>> q = new HashSet<HashSet<string>>() {
                new HashSet<string>(){"q0"}, 
                new HashSet<string>(){"q1"}, 
                new HashSet<string>(){"q2"}, 
                new HashSet<string>(){"q3"}
            };

            // Possible transition functions
            HashSet<char> sigma = new HashSet<char>() {'a', 'b', 'c'};

            // The available transitions
            Dictionary<(HashSet<string>, char), HashSet<string>> delta = new Dictionary<(HashSet<string>, char), HashSet<string>> {
                { (q.ElementAt(0), 'a'), new HashSet<string>() {"q0", "q1"} },
                { (q.ElementAt(1), 'c'), new HashSet<string>() {"q1"} },
                { (q.ElementAt(1), 'b'), new HashSet<string>() {"q2"} },
                { (q.ElementAt(2), 'b'), new HashSet<string>() {"q3"} },
                { (q.ElementAt(3), 'a'), new HashSet<string>() {"q1"} },
            };

            // Final states
            HashSet<HashSet<string>> qF = new HashSet<HashSet<string>>() {new HashSet<string>(){"q2"}};
            // Initial state
            string q0 = "q0";

            // Initializing the FA
            FiniteAutomaton finiteAutomaton = new FiniteAutomaton(q, sigma, delta, q0, qF);

            // Vizualizing the FA
            Console.WriteLine(finiteAutomaton.ToString());

            // Creating a grammar instance from the FA
            Grammar grammar = finiteAutomaton.ToRegularGrammar();
            Console.WriteLine(grammar.ToString());

            // Checking if the FA is deterministic
            Console.WriteLine(finiteAutomaton.IsDFA());

            // We convert the NFA to DFA
            FiniteAutomaton dfa = finiteAutomaton.ToDFA();
            Console.WriteLine(dfa.ToString());

            // Generating the Graph
            dfa.GenerateGraph();
            
            // We create the grammar of the DFA that was created out of the NFA
            Grammar dfaGrammar = dfa.ToRegularGrammar();
            Console.WriteLine(dfaGrammar.ToString());


            Console.WriteLine(dfa.IsDFA());
            
            // Task 1. Checking different grammar types, via input from keyboard
            CheckGrammars();

        }

        // Helper function for Task 1, dealing with keyboard input
        static int CheckGrammars()
        {
            // Setting the variables in order to create a grammar instance
            string start = string.Empty;
            HashSet<HashSet<string>> nonTerminals = new HashSet<HashSet<string>>();
            HashSet<char> terminals = new HashSet<char>();
            List<(string, HashSet<string>)> rules = new List<(string, HashSet<string>)>();

            // Entering the Non-Terminal values
            Console.WriteLine("\n--- Instantiate Your Grammar for Type Checking---");
            Console.Write("\nNon-Terminals: ");
            string? nonTerminalsInput = Console.ReadLine();
            if(nonTerminalsInput == null)
            {
                Console.WriteLine("Null assignment of the 'non-terminal' variables.");
                return -1;
            }
            nonTerminalsInput = nonTerminalsInput.Replace(" ", "");
            string[] nonTerminalsSplit = nonTerminalsInput.Split(',');
            foreach(var variable in nonTerminalsSplit) nonTerminals.Add(new HashSet<string>(){variable});
            
            // Entering the Terminal values
            Console.Write("\nTerminals: ");
            string? terminalsInput = Console.ReadLine();
            if(terminalsInput == null)
            {
                Console.WriteLine("Null assignment of the 'terminal' variables.");
                return -1;
            }
            terminalsInput = terminalsInput.ToLower();
            terminalsInput = terminalsInput.Replace(" ", "");
            terminalsInput = terminalsInput.Replace(",", "");
            char[] terminalsSplit = terminalsInput.ToCharArray();
            foreach(var variable in terminalsSplit) terminals.Add(variable);
            
            // Entering the Start value
            Console.Write("\nStart: ");      
            string? startInput = Console.ReadLine();
            if(startInput == null)
            {
                Console.WriteLine("Null assignment of the 'start' variable.");
                return -1;
            }
            start = startInput.ToUpper();
            if(!nonTerminals.Any(v => v.Contains(start))) nonTerminals.Add(new HashSet<string>(){start});

            // Entering prouduction rules
            int ruleCounter = 0;
            Console.Write("INFO:\n\t- '|' to show more possiblities for the RHS\n\t- 'cancel' to restart the addition of rules\n");
            while(true)
            {
                ruleCounter += 1;

                // Entering the Left Hand Side of the rule
                Console.WriteLine("\n--- Enter the Prodution Rule #{0} ---", ruleCounter);
                Console.Write("LHS: ");
                string? lhsRawInput = Console.ReadLine();
                if(lhsRawInput == null) continue;
                lhsRawInput = lhsRawInput.Replace(" ", "");
                if(lhsRawInput == "cancel") continue;

                // Entering the Right Hand Side of the rule
                Console.Write("\nRHS: ");
                string? rhsRawInput = Console.ReadLine();
                if(rhsRawInput == null) continue;
                rhsRawInput = rhsRawInput.Replace(" ", "");
                if(rhsRawInput == "cancel") continue;
                string[] rhsInput = rhsRawInput.Split('|');
                HashSet<string> rhsRule = rhsInput.ToHashSet();

                // Adding those to the set of rules
                rules.Add((lhsRawInput, rhsRule));
                Console.WriteLine("\nAre you adding more rules? [Anything/no]?");
                Console.Write("Answer: ");
                string? moreRulesToBeAdded = Console.ReadLine();
                if(moreRulesToBeAdded == null || moreRulesToBeAdded.ToLower() == "n" || moreRulesToBeAdded.ToLower() == "no") break;
            }

            // A little bit of automation, if we have a type 3 grammar 
            // then we construct a Finite Automaton that coresponds to it
            Grammar createdGrammar = new Grammar(nonTerminals, terminals, start, rules);
            int type = createdGrammar.CheckType();
            if(type == 3)
            {
                createdGrammar.PopulateProductionRules();
                Console.WriteLine(createdGrammar.ToString());
                FiniteAutomaton finiteAutomaton = createdGrammar.ToFiniteAutomaton();
                Console.WriteLine(finiteAutomaton.ToString());
            }

            // Returning the Type
            return type;
        }
    }
    
}