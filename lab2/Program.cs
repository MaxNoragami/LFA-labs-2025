using System.Net.Mail;

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
            HashSet<char> sigma = new HashSet<char>() {'0', '1'};
            Dictionary<(HashSet<string>, char), HashSet<string>> delta = new Dictionary<(HashSet<string>, char), HashSet<string>> {
                { (q.ElementAt(0), '0'), new HashSet<string>() {"q0", "q2"} },
                { (q.ElementAt(0), '1'), new HashSet<string>() {"q0", "q1"} },
                { (q.ElementAt(1), '1'), new HashSet<string>() {"q3"} },
                { (q.ElementAt(2), '0'), new HashSet<string>() {"q3"} },
                { (q.ElementAt(3), '0'), new HashSet<string>() {"q3"} },
                { (q.ElementAt(3), '1'), new HashSet<string>() {"q3"} },
            };
            HashSet<HashSet<string>> qF = new HashSet<HashSet<string>>() {new HashSet<string>(){"q3"}};
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

            Grammar dfaGrammar = dfa.ToRegularGrammar();
            Console.WriteLine(dfaGrammar.ToString());

            Console.WriteLine(dfa.IsDFA());
            
            Console.WriteLine(CheckGrammars());

        }

        static int CheckGrammars()
        {
            string start = string.Empty;
            HashSet<HashSet<string>> nonTerminals = new HashSet<HashSet<string>>();
            HashSet<char> terminals = new HashSet<char>();
            List<(string, HashSet<string>)> rules = new List<(string, HashSet<string>)>();

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

            Console.Write("\nStart: ");      
            string? startInput = Console.ReadLine();
            if(startInput == null)
            {
                Console.WriteLine("Null assignment of the 'start' variable.");
                return -1;
            }
            start = startInput.ToUpper();
            if(!nonTerminals.Any(v => v.Contains(start))) nonTerminals.Add(new HashSet<string>(){start});

            int ruleCounter = 0;
            Console.Write("INFO:\n\t- '|' to show more possiblities for the RHS\n\t- 'cancel' to restart the addition of rules\n");
            while(true)
            {
                ruleCounter += 1;

                Console.WriteLine("\n--- Enter the Prodution Rule #{0} ---", ruleCounter);
                Console.Write("LHS: ");
                string? lhsRawInput = Console.ReadLine();
                if(lhsRawInput == null) continue;
                lhsRawInput = lhsRawInput.Replace(" ", "");
                if(lhsRawInput == "cancel") continue;

                Console.Write("\nRHS: ");
                string? rhsRawInput = Console.ReadLine();
                if(rhsRawInput == null) continue;
                rhsRawInput = rhsRawInput.Replace(" ", "");
                if(rhsRawInput == "cancel") continue;
                string[] rhsInput = rhsRawInput.Split('|');
                HashSet<string> rhsRule = rhsInput.ToHashSet();
                // foreach(string rhsRule in rhsInput) rules.Add((lhsRawInput, rhsRule));
                rules.Add((lhsRawInput, rhsRule));
                Console.WriteLine("\nAre you adding more rules? [Anything/no]?");
                Console.Write("Answer: ");
                string? moreRulesToBeAdded = Console.ReadLine();
                if(moreRulesToBeAdded == null || moreRulesToBeAdded.ToLower() == "n" || moreRulesToBeAdded.ToLower() == "no") break;
            }
        
            Grammar createdGrammar = new Grammar(nonTerminals, terminals, start, rules);

            return createdGrammar.CheckType();
        }
    }
    
}