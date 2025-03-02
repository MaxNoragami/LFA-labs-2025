using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class FiniteAutomaton
    {
        public HashSet<HashSet<string>> Q { get; set; } // Here goes V_n
        public HashSet<char> Sigma { get; set; } // Here goes V_t
        public Dictionary<(HashSet<string>, char), HashSet<string>> Delta { get; set; } // Here goes P , we have a Dictionary with a Tuple and a hashset        
        public string Q0 {get; set; } // Here goes S
        public HashSet<HashSet<string>> QF { get; set; } // Here goes q_f

        public FiniteAutomaton(HashSet<HashSet<string>> q, HashSet<char> sigma, Dictionary<(HashSet<string>, char), HashSet<string>> delta, string q0, HashSet<HashSet<string>> qF)
        {
            Q = q;
            Sigma = sigma;
            Delta = delta;
            Q0 = q0;
            QF = qF;
        }

        public bool StringBelongToLanguage(string inputString)
        {
            // Wrap Q0 into a singleton set so that currentStates is a HashSet of states.
            var currentStates = new HashSet<HashSet<string>> { new HashSet<string> { Q0 } };
            Console.Write("\nFA Check: ({0}) ", string.Join(" | ", currentStates.Select(s => "{" + string.Join(",", s) + "}")));
            
            for (int i = 0; i < inputString.Length; i++)
            {
                var nextStates = new HashSet<HashSet<string>>();
                char letter = inputString[i];
                foreach (var state in currentStates)
                {
                    if (Delta.TryGetValue((state, letter), out var nextState))
                    {
                        nextStates.Add(nextState);
                    }
                }
                if (nextStates.Count == 0)
                {
                    Console.Write("-----> Invalid for letter {0}", letter);
                    return false;
                }
                Console.Write("--{0}--> ({1}) ", letter, 
                    string.Join(" | ", nextStates.Select(s => "{" + string.Join(",", s) + "}")));
                currentStates = nextStates;
            }
            // Accept if any reached state is among QF.
            return currentStates.Any(state => QF.Contains(state));
        }

        public Grammar ToRegularGrammar()
        {
            // Nonterminals for the grammar are taken from the FA's states.
            var vN = Q;  // (HashSet<HashSet<string>>)
            var vT = Sigma;
            string s = Q0;
            
            // Build production rules.
            // Production type: A -> aB (or A -> a if next state is final)
            var p = new Dictionary<HashSet<string>, (char, HashSet<string>)>(new HashSetComparer());
            foreach (var transition in Delta)
            {
                var lhs = transition.Key.Item1; // left-hand side is a set of strings
                char terminal = transition.Key.Item2;
                var rhs = transition.Value;       // right-hand side state

                // If the destination state is final, treat as terminal production.
                if (QF.Contains(rhs))
                    p[lhs] = (terminal, new HashSet<string>());
                else
                    p[lhs] = (terminal, rhs);
            }
            
            // For any nonterminal with no production, add an ε-production.
            foreach (var nonTerminal in vN)
            {
                if (!p.ContainsKey(nonTerminal))
                    p[nonTerminal] = ('ε', new HashSet<string>());
            }
            
            return new Grammar(vN, vT, p, s);
        }

        public bool IsDFA()
        {
            // DFA if every transition leads to exactly one state.
            return Delta.Values.All(v => v.Count == 1);
        }

        public FiniteAutomaton ToDFA()
        {
            // States to be added
            HashSet<HashSet<string>> q = new HashSet<HashSet<string>>();
            // Transition functions
            HashSet<char> sigma = [.. Sigma];
            // Transitions to be added
            Dictionary<(HashSet<string>, char), HashSet<string>> delta = new Dictionary<(HashSet<string>, char), HashSet<string>>();  
            // Start state as the NFA form
            string q0 = Q0;
            // Final states to be identified
            HashSet<HashSet<string>> qF = [.. QF];
            // Adding the initial state transitions from the NFA Delta to the DFA one by default
            foreach(var transition in Delta)
            {
                var initialState = new HashSet<string>(){q0};
                // Adding the initial state by default
                q.Add(initialState);
                if(transition.Key.Item1 == initialState)
                {
                    delta[transition.Key] = transition.Value;
                    // We add the resulted states of the transition to the states of the DFA
                    q.Add(transition.Value);
                }
            }

            // Taking each state of 'q' through each function 'sigma' to see if we get new states, 
            // for which we will also repeat the process
            var skippedFirst = false;
            foreach(var state in q.ToList())
            {
                // We skip dealing with the initial state
                if(!skippedFirst)
                {
                    skippedFirst = true;
                    continue;
                }

                foreach(var function in sigma)
                {
                    // We check the transition result for each substate of the states from 'q'
                    // using the functions from 'sigma' for an equivalent in 'Delta' 
                    // then we take those equivalents for each states and 
                    // do the reunion in order to obtain a new state for DFA

                    HashSet<string> temporaryState = new HashSet<string>();
                    foreach(var subState in state)
                    {
                        HashSet<string> subStateSetFrom = new HashSet<string>(){subState};
                        if(!Delta.ContainsKey((subStateSetFrom, function))) continue;
                        temporaryState.UnionWith(Delta[(subStateSetFrom, function)]);
                    }

                    // We add the temp states to the 'q' of the DFA 
                    // and log the transitions to obtain the 'delta' of the DFA
                    // and add the transition results to the final states set if they contain the final state
                    if(temporaryState == null) continue;
                    delta[(state, function)] = temporaryState;
                    q.Add(temporaryState);
                    if(temporaryState.Any(state => qF.Any(finalStates => finalStates.Contains(state)))) qF.Add(temporaryState);
                }
            }
            
            // The new FA of Deterministic type
            return new FiniteAutomaton(q, sigma, delta, q0, qF);
        }

        public override string ToString()
        {
            string qData = "Q = {" + string.Join(", ", Q.Select(s => "{" + string.Join(", ", s) + "}")) + "}\n";
            string sigmaData = "Σ = {" + string.Join(", ", Sigma) + "}\n";
            StringBuilder deltaData = new StringBuilder();
            foreach (var pair in Delta)
            {
                deltaData.Append($"\tδ({{" + string.Join(",", pair.Key.Item1) + "}, " + pair.Key.Item2 +") = {" 
                    + string.Join(",", pair.Value) + "}\n");
            }
            string q0Data = "q₀ = " + Q0 + "\n";
            string qFData = "q_F = {" + string.Join(", ", QF.Select(s => "{" + string.Join(", ", s) + "}")) + "}\n";
            return qData + sigmaData + deltaData.ToString() + q0Data + qFData;
        }
    }

    // Helper comparer for HashSet<string> keys.
    public class HashSetComparer : IEqualityComparer<HashSet<string>>
    {
        public bool Equals(HashSet<string> x, HashSet<string> y)
        {
            return x.SetEquals(y);
        }
        public int GetHashCode(HashSet<string> obj)
        {
            int hash = 19;
            foreach (var s in obj.OrderBy(x => x))
                hash = hash * 31 + s.GetHashCode();
            return hash;
        }
    }
}
