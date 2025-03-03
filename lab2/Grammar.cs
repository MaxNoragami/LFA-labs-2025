using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab2
{
    public class Grammar
    {
        public HashSet<HashSet<string>> VN { get; private set; }
        public HashSet<char> VT { get; private set; }
        public Dictionary<HashSet<string>, HashSet<(char, HashSet<string>)>> P { get; private set; }
        public string S { get; private set; }
        public List<(string, HashSet<string>)> LHSandRHS { get; private set; }
        
        

        public Grammar(HashSet<HashSet<string>> vN, HashSet<char> vT, 
                       Dictionary<HashSet<string>, HashSet<(char, HashSet<string>)>> p, string s)
        {
            VN = vN;
            VT = vT;
            P = p;
            S = s;
            LHSandRHS = new List<(string, HashSet<string>)>();
        }

        public Grammar(HashSet<HashSet<string>> vN, HashSet<char> vT, string s, List<(string, HashSet<string>)> lhsANDrhs)
        {
            VN = vN;
            VT = vT;
            P = new Dictionary<HashSet<string>, HashSet<(char, HashSet<string>)>>();
            S = s;
            LHSandRHS = lhsANDrhs;
        }

        public string GenerateString()
        {
            StringBuilder current = new StringBuilder(S);
            Console.Write("\nWord Build: {0}", current);
            bool replaced = true;
            
            while (replaced)
            {
                replaced = false;
                for (int i = 0; i < current.Length; i++)
                {
                    string symbol = current[i].ToString();
                    // Create a candidate nonterminal set for comparison
                    var candidate = new HashSet<string> { symbol };
                    
                    if (VN.Any(nt => nt.SetEquals(candidate)))
                    {
                        replaced = true;
                        // Retrieve the matching nonterminal key from P
                        var key = VN.First(nt => nt.SetEquals(candidate));
                        
                        // Select a random production rule from the possible options
                        var productionOptions = P[key].ToList();
                        var random = new Random();
                        var selectedProduction = productionOptions[random.Next(productionOptions.Count)];
                        
                        // Build the replacement string: terminal + nonterminal (if any)
                        string replacement = selectedProduction.Item1.ToString() + 
                            (selectedProduction.Item2.Count > 0 ? selectedProduction.Item2.First() : "");
                            
                        current.Remove(i, 1);
                        current.Insert(i, replacement);
                        Console.Write(" ---> {0}", current);
                        break;
                    }
                }
            }
            return current.ToString();
        }

        public FiniteAutomaton ToFiniteAutomaton()
        {
            // Create final state (q_F) as a singleton set
            var qF = new HashSet<HashSet<string>>() { new HashSet<string> { "q_F" } };
            // Union VN and q_F to build the state set
            var q = new HashSet<HashSet<string>>(VN, new HashSetComparer());
            q.UnionWith(qF);
            
            HashSet<char> sigma = new HashSet<char>(VT);
            string q0 = S;
            var delta = new Dictionary<(HashSet<string>, char), HashSet<string>>(new TupleComparer());
            
            foreach (var rulePair in P)
            {
                HashSet<string> nonTerminal = rulePair.Key;
                
                foreach (var production in rulePair.Value)
                {
                    char terminal = production.Item1;
                    HashSet<string> nextNonTerminal = production.Item2;
                    
                    var key = (nonTerminal, terminal);
                    
                    // If the production has a nonterminal, use it; otherwise, transition to final state
                    if (nextNonTerminal.Count > 0)
                        delta[key] = nextNonTerminal;
                    else
                        delta[key] = qF.First();
                }
            }
            
            return new FiniteAutomaton(q, sigma, delta, q0, qF);
        }

        public int CheckType()
        {
            if (LHSandRHS.Count == 0)
                return 3; // Default return value if no rules exist

            List<int> potentialTypes = new List<int>();

            // Grammar-wide tracking of linearity for the single-character LHS branch.
            bool seenRightLinear = false;
            bool seenLeftLinear = false;

            foreach (var rule in LHSandRHS)
            {
                string lhs = rule.Item1;
                HashSet<string> rhsOptions = rule.Item2;

                Console.WriteLine("Checking rule: LHS = {0}, RHS = {1}", lhs, string.Join(" | ", rhsOptions));

                // Check if LHS contains at least one non-terminal.
                bool lhsHasNonTerminal = false;
                foreach (var nonTerminalSet in VN)
                {
                    foreach (var nonTerminal in nonTerminalSet)
                    {
                        if (lhs.Contains(nonTerminal))
                        {
                            lhsHasNonTerminal = true;
                            break;
                        }
                    }
                    if (lhsHasNonTerminal)
                        break;
                }

                // For rules with LHS longer than one symbol.
                if (lhs.Length > 1 && lhsHasNonTerminal)
                {
                    // If an epsilon alternative exists and either the LHS is not S or S appears on a RHS, force type 0.
                    if (rhsOptions.Contains("_") && (lhs != S || RhsContainsStart()))
                    {
                        potentialTypes.Add(0);
                        continue;
                    }

                    bool isType1 = true;
                    foreach (var rhs in rhsOptions)
                    {
                        // For Type 1, each RHS should be at least as long as the LHS.
                        if (rhs.Length < lhs.Length)
                        {
                            isType1 = false;
                            break;
                        }
                    }
                    potentialTypes.Add(isType1 ? 1 : 0);
                }
                // For rules with a single-character LHS.
                else if (lhs.Length == 1 && lhsHasNonTerminal)
                {
                    // If the rule consists solely of ε and lhs is not S, mark it as type 2.
                    if (rhsOptions.Count == 1 && rhsOptions.Contains("_") && lhs != S)
                    {
                        potentialTypes.Add(2);
                        continue;
                    }
                    // If the rule has multiple alternatives including ε (for a non-start nonterminal), force type 0.
                    if (rhsOptions.Count > 1 && rhsOptions.Contains("_") && lhs != S)
                    {
                        potentialTypes.Add(0);
                        continue;
                    }

                    bool canBeType3 = true;
                    foreach (var rhs in rhsOptions)
                    {
                        // For the start symbol S, allow an ε-production only if S never appears on any RHS.
                        if (rhs == "_" && lhs == S && !RhsContainsStart())
                            continue;

                        int nonTerminalCount = 0;
                        List<(string nonTerminal, int position)> nonTerminalsInRhs = new List<(string, int)>();

                        foreach (var nonTerminalSet in VN)
                        {
                            foreach (var nonTerminal in nonTerminalSet)
                            {
                                if (rhs.Contains(nonTerminal))
                                {
                                    nonTerminalCount++;
                                    nonTerminalsInRhs.Add((nonTerminal, rhs.IndexOf(nonTerminal)));
                                }
                            }
                        }

                        // If there are no non-terminals, the production must be exactly one terminal to be regular.
                        if (nonTerminalCount == 0)
                        {
                            if (rhs.Length > 1)
                            {
                                canBeType3 = false;
                            }
                            continue;
                        }

                        // More than one non-terminal forces a Type 2 classification.
                        if (nonTerminalCount > 1)
                        {
                            canBeType3 = false;
                            continue;
                        }

                        // With exactly one non-terminal, check its position.
                        var nonTerminalInfo = nonTerminalsInRhs[0];
                        string foundNonTerminal = nonTerminalInfo.nonTerminal;
                        int nonTerminalPosition = nonTerminalInfo.position;

                        // Determine if the production is right-linear or left-linear.
                        bool isRightLinearRule = nonTerminalPosition == rhs.Length - foundNonTerminal.Length;
                        bool isLeftLinearRule = nonTerminalPosition == 0;

                        if (!isRightLinearRule && !isLeftLinearRule)
                        {
                            canBeType3 = false;
                            continue;
                        }

                        if (isRightLinearRule)
                            seenRightLinear = true;
                        if (isLeftLinearRule)
                            seenLeftLinear = true;
                        if (seenRightLinear && seenLeftLinear)
                        {
                            canBeType3 = false;
                            break;
                        }
                    }
                    potentialTypes.Add(canBeType3 ? 3 : 2);
                }
                else
                {
                    // Invalid grammar.
                    potentialTypes.Add(-1);
                }
            }

            if (potentialTypes.Count == 0)
                return -1;

            // Return the most restrictive (minimum) type among all rules.
            return potentialTypes.Min();

        }

        public void PopulateProductionRules()
        {
            // Clear any existing production rules.
            P.Clear();

            // Process each production rule from LHSandRHS.
            foreach (var rule in LHSandRHS)
            {
                // rule.Item1 is the LHS as a string.
                // rule.Item2 is the set of alternatives (each alternative is a string).
                string lhsString = rule.Item1;

                // Find the matching nonterminal in VN (each nonterminal is a HashSet<string>).
                // It is assumed that VN contains sets with a single string.
                var key = VN.FirstOrDefault(nt => nt.Contains(lhsString));
                if (key == null)
                {
                    // If the nonterminal is not already present, add it.
                    key = new HashSet<string> { lhsString };
                    VN.Add(key);
                }

                // Ensure the key exists in the production rules dictionary.
                if (!P.ContainsKey(key))
                {
                    P[key] = new HashSet<(char, HashSet<string>)>();
                }

                // Process each alternative production.
                foreach (var alternative in rule.Item2)
                {
                    // Check if the alternative represents epsilon.
                    if (alternative == "_")
                    {
                        // Represent an epsilon production by using '_' (or any agreed-upon symbol)
                        // and an empty nonterminal set.
                        P[key].Add(('_', new HashSet<string>()));
                    }
                    else
                    {
                        // Otherwise, assume the alternative is of the form: terminal [nonterminal]
                        // where the first character is the terminal.
                        char terminal = alternative[0];
                        var nextNonTerminal = new HashSet<string>();

                        // If there is more than one character, the rest is the nonterminal.
                        if (alternative.Length > 1)
                        {
                            string nonTerminalSymbol = alternative.Substring(1);
                            nextNonTerminal.Add(nonTerminalSymbol);
                        }

                        P[key].Add((terminal, nextNonTerminal));
                    }
                }
            }
        }


        // Helper method to check if S appears on the right side of any rule
        private bool RhsContainsStart()
        {
            foreach (var rule in LHSandRHS)
            {
                foreach (var rhs in rule.Item2)
                {
                    if (rhs.Contains(S))
                        return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            string vNData = "V_N = {" + string.Join(", ", VN.Select(nt => "{" + string.Join(", ", nt) + "}")) + "}\n";
            string vTData = "V_T = {" + string.Join(", ", VT) + "}\n"; 
            
            StringBuilder pData = new StringBuilder("P = {\n");
            
            // Group productions by left-hand side
            foreach (var pair in P)
            {
                string lhs = "{" + string.Join(",", pair.Key) + "}";
                
                // Combine all productions for this non-terminal
                List<string> rhsStrings = new List<string>();
                foreach (var production in pair.Value)
                {
                    string rhs = production.Item1.ToString() + 
                        (production.Item2.Count > 0 ? " {" + string.Join(", ", production.Item2) + "}" : " ε");
                    rhsStrings.Add(rhs);
                }
                
                // Join with pipe symbol
                pData.Append("\t" + lhs + " ---> " + string.Join(" | ", rhsStrings) + "\n");
            }
            pData.Append("}\n");
            
            string sData = "S = " + S + "\n";
            return vNData + vTData + pData.ToString() + sData;
        }
    }

    // Helper comparer for tuple keys: (HashSet<string>, char)
    public class TupleComparer : IEqualityComparer<(HashSet<string>, char)>
    {
        public bool Equals((HashSet<string>, char) x, (HashSet<string>, char) y)
        {
            return x.Item2 == y.Item2 && x.Item1.SetEquals(y.Item1);
        }
        public int GetHashCode((HashSet<string>, char) obj)
        {
            int hash = 17;
            hash = hash * 23 + obj.Item2.GetHashCode();
            foreach (var s in obj.Item1.OrderBy(x => x))
                hash = hash * 23 + s.GetHashCode();
            return hash;
        }
    }
}