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

        public Grammar(HashSet<HashSet<string>> vN, HashSet<char> vT, 
                       Dictionary<HashSet<string>, HashSet<(char, HashSet<string>)>> p, string s)
        {
            VN = vN;
            VT = vT;
            P = p;
            S = s;
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