using System.Text;

namespace lab5
{
    public static class ChomskyNormalForm
    {
        private static string GetNextAvailableLetter(Grammar grammar)
        {
            // Start from G as specified in the problem statement
            char letter = 'G';
            while (grammar.VN.Contains(letter.ToString()))
            {
                letter++;
            }
            return letter.ToString();
        }

        private static void EliminateEmptyProductions(Grammar grammar)
        {
            // Find all nullable variables
            HashSet<string> nullableVariables = new HashSet<string>();
            
            // Find directly nullable variables first
            foreach (var pair in grammar.P)
            {
                if (pair.Value.Contains("ε"))
                {
                    nullableVariables.Add(pair.Key);
                }
            }
            
            // Find indirectly nullable variables
            bool changed;
            do
            {
                changed = false;
                
                foreach (var pair in grammar.P)
                {
                    if (!nullableVariables.Contains(pair.Key))
                    {
                        foreach (var rhs in pair.Value)
                        {
                            bool allNullable = true;
                            
                            foreach (char c in rhs)
                            {
                                string symbol = c.ToString();
                                
                                if (!nullableVariables.Contains(symbol) || grammar.VT.Contains(c))
                                {
                                    allNullable = false;
                                    break;
                                }
                            }
                            
                            if (allNullable && rhs.Length > 0)
                            {
                                nullableVariables.Add(pair.Key);
                                changed = true;
                                break;
                            }
                        }
                    }
                }
            } while (changed);
            
            // Generate all possible combinations of productions
            Dictionary<string, List<string>> newP = new Dictionary<string, List<string>>();
            
            foreach (var pair in grammar.P)
            {
                newP[pair.Key] = new List<string>();
                
                foreach (var rhs in pair.Value)
                {
                    if (rhs != "ε")
                    {
                        // Keep the original production
                        newP[pair.Key].Add(rhs);
                        
                        // Generate all combinations by removing nullable variables
                        List<int> nullablePositions = new List<int>();
                        for (int i = 0; i < rhs.Length; i++)
                        {
                            if (nullableVariables.Contains(rhs[i].ToString()))
                            {
                                nullablePositions.Add(i);
                            }
                        }
                        
                        // Generate all subsets of nullable positions
                        int subsets = 1 << nullablePositions.Count; // 2^n
                        for (int i = 1; i < subsets; i++) // Start from 1 to exclude empty subset
                        {
                            StringBuilder newRhs = new StringBuilder(rhs);
                            
                            // Process positions in reverse to avoid shifting issues
                            for (int j = nullablePositions.Count - 1; j >= 0; j--)
                            {
                                if ((i & (1 << j)) != 0)
                                {
                                    newRhs.Remove(nullablePositions[j], 1);
                                }
                            }
                            
                            if (newRhs.Length > 0 && !newP[pair.Key].Contains(newRhs.ToString()))
                            {
                                newP[pair.Key].Add(newRhs.ToString());
                            }
                        }
                    }
                }
            }
            
            grammar.P = newP;
        }

        private static void EliminateAnyUnitRules(Grammar grammar)
        {
            // Dictionary to store the set of non-terminals that can be derived from each non-terminal through unit productions
            Dictionary<string, HashSet<string>> unitDerivations = new Dictionary<string, HashSet<string>>();
            
            // Initialize unitDerivations for each non-terminal
            foreach (var nonTerminal in grammar.VN)
            {
                unitDerivations[nonTerminal] = new HashSet<string> { nonTerminal };
            }
            
            // Compute the transitive closure of unit productions
            bool changed;
            do
            {
                changed = false;
                
                foreach (var pair in grammar.P)
                {
                    string lhs = pair.Key;
                    
                    foreach (var rhs in pair.Value)
                    {
                        // If rhs is a single non-terminal (unit production)
                        if (rhs.Length == 1 && grammar.VN.Contains(rhs))
                        {
                            foreach (var derived in unitDerivations[rhs])
                            {
                                if (!unitDerivations[lhs].Contains(derived))
                                {
                                    unitDerivations[lhs].Add(derived);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            } while (changed);
            
            // Replace unit productions
            Dictionary<string, List<string>> newP = new Dictionary<string, List<string>>();
            
            foreach (var lhs in grammar.VN)
            {
                newP[lhs] = new List<string>();
                
                foreach (var derivedNT in unitDerivations[lhs])
                {
                    foreach (var rhs in grammar.P[derivedNT])
                    {
                        // If it's not a unit production
                        if (!(rhs.Length == 1 && grammar.VN.Contains(rhs)))
                        {
                            if (!newP[lhs].Contains(rhs))
                            {
                                newP[lhs].Add(rhs);
                            }
                        }
                    }
                }
            }
            
            grammar.P = newP;
        }

        private static void EliminateInaccessibleSymbols(Grammar grammar)
        {
            // Find all accessible symbols
            HashSet<string> accessibleSymbols = new HashSet<string> { grammar.S };
            
            bool changed;
            do
            {
                changed = false;
                
                foreach (var pair in grammar.P)
                {
                    string lhs = pair.Key;
                    
                    if (accessibleSymbols.Contains(lhs))
                    {
                        foreach (var rhs in pair.Value)
                        {
                            foreach (char c in rhs)
                            {
                                string symbol = c.ToString();
                                
                                if (grammar.VN.Contains(symbol) && !accessibleSymbols.Contains(symbol))
                                {
                                    accessibleSymbols.Add(symbol);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            } while (changed);
            
            // Remove inaccessible symbols and their productions
            HashSet<string> inaccessibleSymbols = new HashSet<string>(grammar.VN);
            inaccessibleSymbols.ExceptWith(accessibleSymbols);
            
            foreach (var symbol in inaccessibleSymbols)
            {
                grammar.VN.Remove(symbol);
                grammar.P.Remove(symbol);
            }
        }

        private static void EliminateNonProdcutiveSymbols(Grammar grammar)
        {
            
        }

        public static void Obtain(Grammar grammar)
        {
            
        }
    }
}