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
            
        }

        private static void EliminateInaccessibleSymbols(Grammar grammar)
        {
            
        }

        private static void EliminateNonProdcutiveSymbols(Grammar grammar)
        {
            
        }

        public static void Obtain(Grammar grammar)
        {
            
        }
    }
}