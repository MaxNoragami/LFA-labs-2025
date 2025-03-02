using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Nonterminals for the grammar are taken from the FA's states
            var vN = Q;
            var vT = Sigma;
            string s = Q0;
            
            // Build production rules
            var p = new Dictionary<HashSet<string>, HashSet<(char, HashSet<string>)>>(new HashSetComparer());
            
            foreach (var transition in Delta)
            {
                var lhs = transition.Key.Item1; // left-hand side is a set of strings
                char terminal = transition.Key.Item2;
                var rhs = transition.Value;     // right-hand side state
                
                // Create an entry for this non-terminal if it doesn't exist
                if (!p.ContainsKey(lhs))
                    p[lhs] = new HashSet<(char, HashSet<string>)>();
                
                // Add the production rule
                p[lhs].Add((terminal, rhs));
                
                // Add an epsilon-production if the target state is a final state
                // Use Any() with a custom equality comparison to properly check against QF
                if (QF.Any(finalState => finalState.SetEquals(rhs)))
                {
                    p[lhs].Add((terminal, new HashSet<string>()));
                }
            }
            
            // For any nonterminal with no production, add an ε-production
            foreach (var nonTerminal in vN)
            {
                if (!p.ContainsKey(nonTerminal))
                    p[nonTerminal] = new HashSet<(char, HashSet<string>)> { ('ε', new HashSet<string>()) };
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
            // Create a custom comparer for hash sets to ensure proper equality checking
            var setComparer = new HashSetComparer();
            
            // Initialize the DFA components
            var q = new HashSet<HashSet<string>>(setComparer);
            var sigma = new HashSet<char>(Sigma);
            var delta = new Dictionary<(HashSet<string>, char), HashSet<string>>(new TupleComparer());
            
            // Initial state of the DFA is {q0}
            var initialState = new HashSet<string> { Q0 };
            q.Add(initialState);
            
            // Keep track of states we still need to process
            var statesToProcess = new Queue<HashSet<string>>();
            statesToProcess.Enqueue(initialState);
            
            // Final states of the DFA
            var qF = new HashSet<HashSet<string>>(setComparer);
            
            // Process each state in the queue
            while (statesToProcess.Count > 0)
            {
                var currentState = statesToProcess.Dequeue();
                
                // Determine if this state contains any final states from the NFA
                foreach (var finalState in QF)
                {
                    if (currentState.Overlaps(finalState))
                    {
                        qF.Add(currentState);
                        break;
                    }
                }
                
                // For each symbol in the alphabet, compute the next state
                foreach (var symbol in sigma)
                {
                    var nextState = new HashSet<string>();
                    
                    // For each state in the current composite state
                    foreach (var state in currentState)
                    {
                        // Try to find transitions for this individual state
                        foreach (var transition in Delta)
                        {
                            if (transition.Key.Item2 == symbol && 
                                transition.Key.Item1.Contains(state))
                            {
                                // Add destination states to our next composite state
                                foreach (var destState in transition.Value)
                                {
                                    nextState.Add(destState);
                                }
                            }
                        }
                    }
                    
                    // If we found a valid next state
                    if (nextState.Count > 0)
                    {
                        // Add the transition to the DFA
                        delta[(currentState, symbol)] = nextState;
                        
                        // If this is a new state, add it to the queue
                        if (!q.Contains(nextState))
                        {
                            q.Add(nextState);
                            statesToProcess.Enqueue(nextState);
                        }
                    }
                }
            }
            
            return new FiniteAutomaton(q, sigma, delta, Q0, qF);
        }

public void GenerateGraph()
{
    // Define the output folder and file names
    string folderPath = "Graphs";
    if (!Directory.Exists(folderPath))
    {
        Directory.CreateDirectory(folderPath);
    }
    
    // Create a unique suffix using the current timestamp.
    string uniqueSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");

    // Use the unique suffix in the file names.
    string dotFilePath = Path.Combine(folderPath, $"FiniteAutomaton_{uniqueSuffix}.dot");
    string outputImagePath = Path.Combine(folderPath, $"FiniteAutomaton_{uniqueSuffix}.png");

    // Build the DOT representation
    StringBuilder dot = new StringBuilder();
    dot.AppendLine("digraph FA {");
    dot.AppendLine("\trankdir=LR;");
    dot.AppendLine("\tsize=\"8,5\";");
    dot.AppendLine("\tnode [shape = circle];");

    // Define an invisible initial node with no label.
    dot.AppendLine("\t\"init\" [shape=point, label=\"\"];");

    // Create nodes for each state in Q.
    foreach (var state in Q)
    {
        // Create a unique node id by joining state names with an underscore.
        string nodeId = string.Join("_", state);
        // For display, show the state with curly braces (e.g. {q0} or {q1,q2})
        string label = "{" + string.Join(",", state) + "}";
        if (QF.Any(final => final.SetEquals(state)))
        {
            dot.AppendLine($"\t\"{nodeId}\" [label=\"{label}\", shape=doublecircle];");
        }
        else
        {
            dot.AppendLine($"\t\"{nodeId}\" [label=\"{label}\"];");
        }
    }

    // Determine the initial state node id (assuming Q0 is a string and corresponds to {Q0} in Q)
    string initialNodeId = string.Join("_", new HashSet<string> { Q0 });

    // Connect the invisible node to the initial state
    dot.AppendLine($"\t\"init\" -> \"{initialNodeId}\";");

    // Force the invisible node and the initial state to be on the same rank
    dot.AppendLine("\t{ rank = same; \"init\"; \"" + initialNodeId + "\"; }");

    // Add edges for each transition in Delta.
    foreach (var transition in Delta)
    {
        var fromState = transition.Key.Item1;
        char symbol = transition.Key.Item2;
        var toState = transition.Value;

        string fromId = string.Join("_", fromState);
        string toId = string.Join("_", toState);

        dot.AppendLine($"\t\"{fromId}\" -> \"{toId}\" [label=\"{symbol}\"];");
    }
    dot.AppendLine("}");

    // Write the DOT file
    File.WriteAllText(dotFilePath, dot.ToString());
    Console.WriteLine("DOT file generated at: " + dotFilePath);

    // Call GraphViz to generate a PNG from the DOT file.
    Process process = new Process();
    process.StartInfo.FileName = "dot"; // Ensure 'dot' is available in your PATH
    process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"";
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.UseShellExecute = false;

    try
    {
        process.Start();
        process.WaitForExit();
        Console.WriteLine($"Graph generated successfully as \"{outputImagePath}\".");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error generating PNG with GraphViz: " + ex.Message);
        Console.WriteLine("Please ensure GraphViz is installed and the 'dot' command is available in your PATH.");
    }
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
