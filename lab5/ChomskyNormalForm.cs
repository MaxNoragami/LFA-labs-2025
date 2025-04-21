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
            // Find all productive symbols (non-terminals that can derive a string of terminals)
            HashSet<string> productiveSymbols = new HashSet<string>();
            
            // Find directly productive symbols (non-terminals that derive terminals)
            foreach (var pair in grammar.P)
            {
                foreach (var rhs in pair.Value)
                {
                    bool allTerminals = true;
                    
                    foreach (char c in rhs)
                    {
                        if (!grammar.VT.Contains(c))
                        {
                            allTerminals = false;
                            break;
                        }
                    }
                    
                    if (allTerminals)
                    {
                        productiveSymbols.Add(pair.Key);
                        break;
                    }
                }
            }
            
            // Find indirectly productive symbols
            bool changed;
            do
            {
                changed = false;
                
                foreach (var pair in grammar.P)
                {
                    string lhs = pair.Key;
                    
                    if (!productiveSymbols.Contains(lhs))
                    {
                        foreach (var rhs in pair.Value)
                        {
                            bool allProductive = true;
                            
                            foreach (char c in rhs)
                            {
                                string symbol = c.ToString();
                                
                                if (grammar.VN.Contains(symbol) && !productiveSymbols.Contains(symbol))
                                {
                                    allProductive = false;
                                    break;
                                }
                            }
                            
                            if (allProductive)
                            {
                                productiveSymbols.Add(lhs);
                                changed = true;
                                break;
                            }
                        }
                    }
                }
            } while (changed);
            
            // Remove non-productive symbols
            HashSet<string> nonProductiveSymbols = new HashSet<string>(grammar.VN);
            nonProductiveSymbols.ExceptWith(productiveSymbols);
            
            foreach (var symbol in nonProductiveSymbols)
            {
                grammar.VN.Remove(symbol);
                grammar.P.Remove(symbol);
            }
            
            // Remove productions containing non-productive symbols
            Dictionary<string, List<string>> newP = new Dictionary<string, List<string>>();
            
            foreach (var pair in grammar.P)
            {
                newP[pair.Key] = new List<string>();
                
                foreach (var rhs in pair.Value)
                {
                    bool containsNonProductive = false;
                    
                    foreach (char c in rhs)
                    {
                        string symbol = c.ToString();
                        
                        if (grammar.VN.Contains(symbol) && nonProductiveSymbols.Contains(symbol))
                        {
                            containsNonProductive = true;
                            break;
                        }
                    }
                    
                    if (!containsNonProductive)
                    {
                        newP[pair.Key].Add(rhs);
                    }
                }
            }
            
            grammar.P = newP;
        }

        public static void Obtain(Grammar grammar)
        {
            // If the start symbol S occurs on some RHS, create a new start symbol S* and add a new production S* -> S
            bool needNewStartSymbol = false;
            
            foreach (var pair in grammar.P)
            {
                foreach (var rhs in pair.Value)
                {
                    if (rhs.Contains(grammar.S))
                    {
                        needNewStartSymbol = true;
                        break;
                    }
                }
                
                if (needNewStartSymbol)
                    break;
            }
            
            if (needNewStartSymbol)
            {
                string newStartSymbol = GetNextAvailableLetter(grammar);
                grammar.VN.Add(newStartSymbol);
                grammar.P[newStartSymbol] = new List<string> { grammar.S };
                grammar.S = newStartSymbol;
            }
            
            Console.WriteLine("\nGrammar after adding a new start symbol:");
            Console.WriteLine(grammar.ToString());

            // Apply the grammar cleanup steps
            EliminateEmptyProductions(grammar);
            Console.WriteLine("\nGrammar after eliminating empty prods:");
            Console.WriteLine(grammar.ToString());

            EliminateAnyUnitRules(grammar);
            Console.WriteLine("\nGrammar after eliminating unit rules:");
            Console.WriteLine(grammar.ToString());

            EliminateInaccessibleSymbols(grammar);
            Console.WriteLine("\nGrammar after eliminating inaccessible symbols:");
            Console.WriteLine(grammar.ToString());

            EliminateNonProdcutiveSymbols(grammar);
            Console.WriteLine("\nGrammar after eliminating non-productive symbols:");
            Console.WriteLine(grammar.ToString());

            // Step 1: Replace each terminal in the RHS with a new non-terminal
            Dictionary<char, string> terminalToNonTerminal = new Dictionary<char, string>();
            Dictionary<string, List<string>> newP = new Dictionary<string, List<string>>();
            
            foreach (var pair in grammar.P)
            {
                string lhs = pair.Key;
                newP[lhs] = new List<string>();
                
                foreach (var rhs in pair.Value)
                {
                    string newRhs = "";
                    
                    // Process each character in the RHS
                    for (int i = 0; i < rhs.Length; i++)
                    {
                        char c = rhs[i];
                        
                        // If it's a terminal and not the only symbol in a production
                        if (grammar.VT.Contains(c) && (rhs.Length > 1))
                        {
                            // Create a new non-terminal for the terminal if it doesn't exist
                            if (!terminalToNonTerminal.ContainsKey(c))
                            {
                                string newNonTerminal = GetNextAvailableLetter(grammar);
                                grammar.VN.Add(newNonTerminal);
                                
                                if (!newP.ContainsKey(newNonTerminal))
                                {
                                    newP[newNonTerminal] = new List<string>();
                                }
                                
                                newP[newNonTerminal].Add(c.ToString());
                                terminalToNonTerminal[c] = newNonTerminal;
                            }
                            
                            newRhs += terminalToNonTerminal[c];
                        }
                        else
                        {
                            // Keep non-terminals or single terminals as they are
                            newRhs += c;
                        }
                    }
                    
                    newP[lhs].Add(newRhs);
                }
            }
            
            grammar.P = newP;
            
            // Dictionary to map production RHS to existing non-terminals
            Dictionary<string, string> rhsToNonTerminal = new Dictionary<string, string>();
            
            // Step 2: Replace productions with more than 2 symbols on the RHS with binary productions
            newP = new Dictionary<string, List<string>>();
            
            foreach (var pair in grammar.P)
            {
                string lhs = pair.Key;
                newP[lhs] = new List<string>();
                
                foreach (var rhs in pair.Value)
                {
                    if (rhs.Length > 2)
                    {
                        // Create a chain of non-terminals for the RHS
                        string currentLhs = lhs;
                        string remainingRhs = rhs;
                        
                        while (remainingRhs.Length > 2)
                        {
                            string firstSymbol = remainingRhs[0].ToString();
                            string restOfRhs = remainingRhs.Substring(1);
                            
                            // Check if we already have a non-terminal for the rest of RHS
                            string newNonTerminal;
                            if (rhsToNonTerminal.ContainsKey(restOfRhs))
                            {
                                newNonTerminal = rhsToNonTerminal[restOfRhs];
                            }
                            else
                            {
                                newNonTerminal = GetNextAvailableLetter(grammar);
                                grammar.VN.Add(newNonTerminal);
                                rhsToNonTerminal[restOfRhs] = newNonTerminal;
                                
                                if (!newP.ContainsKey(newNonTerminal))
                                {
                                    newP[newNonTerminal] = new List<string>();
                                }
                            }
                            
                            // Add A -> B1C production
                            newP[currentLhs].Add(firstSymbol + newNonTerminal);
                            
                            // Prepare for next iteration
                            currentLhs = newNonTerminal;
                            remainingRhs = remainingRhs.Substring(1);
                            
                            if (!newP.ContainsKey(currentLhs) && !grammar.P.ContainsKey(currentLhs))
                            {
                                newP[currentLhs] = new List<string>();
                            }
                        }
                        
                        // Add the final production with the last two symbols
                        if (!newP[currentLhs].Contains(remainingRhs))
                        {
                            newP[currentLhs].Add(remainingRhs);
                        }
                    }
                    else
                    {
                        // If the RHS has 2 or fewer symbols, keep it as it is
                        newP[lhs].Add(rhs);
                    }
                }
            }
            
            // Add any existing productions for non-terminals we're reusing
            foreach (var nonTerminal in rhsToNonTerminal.Values)
            {
                if (grammar.P.ContainsKey(nonTerminal) && !newP.ContainsKey(nonTerminal))
                {
                    newP[nonTerminal] = new List<string>(grammar.P[nonTerminal]);
                }
            }
            
            grammar.P = newP;
            
            // Step 3: Final optimization to merge duplicate right-hand sides
            Dictionary<string, List<string>> finalP = new Dictionary<string, List<string>>();
            Dictionary<string, string> productionToNonTerminal = new Dictionary<string, string>();
            
            // Build a mapping of unique productions to non-terminals
            foreach (var pair in grammar.P)
            {
                string lhs = pair.Key;
                
                if (!finalP.ContainsKey(lhs))
                {
                    finalP[lhs] = new List<string>();
                }
                
                foreach (var rhs in pair.Value)
                {
                    // For two-symbol productions, check if we can reuse an existing non-terminal
                    if (rhs.Length == 2)
                    {
                        if (productionToNonTerminal.ContainsKey(rhs))
                        {
                            if (!finalP[lhs].Contains(rhs))
                            {
                                finalP[lhs].Add(rhs);
                            }
                        }
                        else
                        {
                            productionToNonTerminal[rhs] = lhs;
                            if (!finalP[lhs].Contains(rhs))
                            {
                                finalP[lhs].Add(rhs);
                            }
                        }
                    }
                    else
                    {
                        // For single symbol productions, we just add them
                        if (!finalP[lhs].Contains(rhs))
                        {
                            finalP[lhs].Add(rhs);
                        }
                    }
                }
            }
            
            grammar.P = finalP;
        }
    }
}