# Topic: Determinism in Finite Automata. Conversion from NDFA 2 DFA. Chomsky Hierarchy.
****
### Course: Formal Languages & Finite Automata
### Author: Maxim Alexei
----

## Theory:
****
##### Introduction to Finite Automata
A **finite automaton (FA)** is a mathematical model used to describe computation or processes with a finite number of states. It is often visualized as a state machine where:
- **States:** Represent different configurations or conditions.
- **Transitions:** Dictate how the automaton moves from one state to another based on input symbols.
- **Start and Final States:** The automaton begins in a designated start state and may reach one or more final (accepting) states to signify successful processing.
Finite automata are widely used in fields such as compiler design, pattern matching, and network protocols.
##### Regular Grammars and Finite Automata
- **Regular Grammars** generate regular languages, which can also be recognized by **finite automata**[[4]](#ref4).  
- **Finite Automaton (FA):** A 5-tuple $(Q, \Sigma, \delta, q_0, F)$:  
  - ( $Q$ ): Finite set of states.  
  - ( $\delta$ ): Transition function $( Q \times Σ \rightarrow Q )$.  
  - ( $q_0$ ): Initial state.  
  - ( $F$ ): Accepting states.  
##### Determinism in Finite Automata
The concept of **determinism** in finite automata refers to how predictable the system's transitions are:
- **Deterministic Finite Automata (DFA):**
    - For every state and input symbol, there is exactly one transition to a new state.
    - This makes the computational process unambiguous and straightforward, as each step is clearly defined.
- **Non-Deterministic Finite Automata (NDFA or NFA):**
    - For a given state and input symbol, the automaton may have several possible transitions (including the possibility of epsilon transitions, which occur without consuming an input).
    - Although multiple paths may exist, every NFA has an equivalent DFA that accepts the same language.
Determinism is vital for predictability and efficiency, especially when the automaton is implemented in software. However, nondeterminism can simplify the design or modeling of complex systems even though it might need conversion into a deterministic form for execution.
##### Conversion from NDFA to DFA
To execute a nondeterministic model on deterministic hardware, it is often necessary to convert an NDFA into a DFA. This is achieved using the **subset construction algorithm**:
- **Step 1: Start with the epsilon-closure.**  
    Identify all states reachable from the start state via epsilon (ε) transitions. This set forms the DFA’s initial state.
- **Step 2: Process input symbols.**  
    For each DFA state (which represents a set of NDFA states) and for each input symbol: 
    - Compute the set of NDFA states reachable by that input.
    - Apply the epsilon-closure on that resulting set.
    - This new set of states forms a new DFA state if it hasn’t been encountered before.
- **Step 3: Mark accepting states.**  
    Any DFA state that includes at least one NDFA accepting state is marked as accepting.
- **Step 4: Iterate until all transitions are defined.**  
    Continue processing until no new DFA states are generated.
This algorithm guarantees that the resulting DFA recognizes the same language as the original NDFA, albeit often at the cost of potentially exponential growth in the number of states.
##### Chomsky Hierarchy
*Grammars* are classified into *four types* according to **Chomsky**:  
1. **Type 0 (Unrestricted):** No restrictions on production rules
2. **Type 1 (Context-Sensitive):** Rules of the form  $\alpha A \beta \rightarrow \alpha \gamma \beta$.  
3. **Type 2 (Context-Free):** Rules $A \rightarrow \gamma, \text{where } A \in V_N$.  
4. **Type 3 (Regular):** Rules  $A \rightarrow aB$ (right-linear) or $A \rightarrow Ba$ (left-linear),  $A \rightarrow a$.  
Finite automata are directly associated with **regular grammars (Type 3)**. This relationship means that every regular language (accepted by a DFA or NFA) can be described by a regular grammar, and vice versa.
##### Conversion from Finite Automata to Regular Grammar
The conversion process between a finite automaton and a regular grammar is systematic:
- **For every transition:**  
    If there is a transition from state $A$ to state $B$ on symbol $a$, you can introduce a production rule in the grammar:
	    $A\rightarrow aB$
- **For every accepting state:**  
    If state $A$ is an accepting (final) state, add the production:
		$A \rightarrow \epsilon$
    This rule allows the grammar to generate the empty string, which reflects the automaton’s acceptance of the input when ending in a final state.
This conversion establishes a direct mapping from the structural components of the automaton to the production rules of a regular grammar, thereby showing that the languages defined by finite automata and regular grammars are equivalent.
  
## Objectives:
****
1. Understand what an automaton is and what it can be used for.

2. Continuing the work in the same repository and the same project, the following need to be added: a. Provide a function in your grammar type/class that could classify the grammar based on Chomsky hierarchy.
    b. For this you can use the variant from the previous lab.

3. According to your variant number (by universal convention it is register ID), get the finite automaton definition and do the following tasks:
    a. Implement conversion of a finite automaton to a regular grammar.
    b. Determine whether your FA is deterministic or non-deterministic.
    c. Implement some functionality that would convert an NDFA to a DFA.
    d. Represent the finite automaton graphically (Optional, and can be considered as a **_bonus point_**):
    - You can use external libraries, tools or APIs to generate the figures/diagrams.
    - Your program needs to gather and send the data about the automaton and the lib/tool/API return the visual representation.

Please consider that all elements of the task 3 can be done manually, writing a detailed report about how you've done the conversion and what changes have you introduced. In case if you'll be able to write a complete program that will take some finite automata and then convert it to the regular grammar - this will be **a good bonus point**.

## Implementation description
****

- To begin with, we are going to have a look in  the `Main()` method from `Program.cs`, which starts with the initialization of a `FiniteAutomaton` instance for task 2. However, in order to create an object of type `FiniteAutomaton` we have to initialize all the needed parameters, such as `q` representing the states, `delta` the transitions, etc.
- Compared to the previous lab, despite keeping the same functionality, since I am using *C#* that is a statically-typed language, I had to do a lot of changes regarding the types of most of the properties from both the `Grammar` and the `FiniteAutomaton` class, such as going from `Dictionary<(string, char), HashSet<string>>` to `Dictionary<(HashSet<string>, char), HashSet<string>>` for the transitions, since the states of the *DFA* obtained via the *NFA* will be composed.

```csharp
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
}
```

- Further on, we are going to analyze the second part of the `Main()` method from `Program.cs` where after instantiating a NFA according to the *Variant 1*, we print the details about it to the console. Then we instantiate a `Grammar` out of it, using the `ToRegularGrammar()` method, which has its details displayed in a pretty manner in the console, as well.
- After that, we are checking whether the FA that we are working on is either deterministic or nondeterministic, using the `IsDFA()` method, that returns `true` in case the Finite Automaton is deterministic, and `false` in case it is not. Then we simply generate a graph based on the obtained Finite Automaton using the a method that gets explained later on.
- Last but not least, we convert the obtained Deterministic Finite Automaton out of the *NFA* from *Variant 1*, into a `Grammar` that is of type 3. Followed by, calling the `CheckGrammars()` method that is related to the requirement from *Task 1*, that basically checks of which type are the grammars introduced from the keyboard. 

```csharp
static void Main()
{
	...
	
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

	// Task 1. Checking different grammar types, via input from keyboard
	CheckGrammars();
}
```

* Next, we analyze the `ToRegularGrammar()` method from the `FiniteAutomaton` class, which is responsible for converting a given Finite Automaton (FA) into a regular grammar. The method ensures that each FA state is mapped to a nonterminal symbol in the resulting grammar.
- First, the set of nonterminals (`vN`) is initialized to match the set of states `Q`, while the set of terminals (`vT`) corresponds to the input alphabet `Sigma`. The starting symbol `s` is assigned to the initial state `Q0`, ensuring that the grammar starts from the same initial condition as the FA.
- The core logic involves constructing production rules (`p`) based on the transition function (`Delta`). Each transition from a state with a given input symbol leads to another state, which is directly translated into a grammar production. If a transition leads to a final state, an epsilon-production is also introduced to allow termination at that point.
- Lastly, the method ensures that all nonterminals in `vN` are accounted for in the production rules. If a nonterminal has no defined productions, an epsilon-production is explicitly added to maintain a valid grammar structure.
- The resulting grammar, built from the FA's structure, is returned as an instance of the `Grammar` class, ready to be used for further analysis or transformation.

```csharp
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
		var rhs = transition.Value;     // right-hand side state

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
```

- Check if FA is a DFA

```csharp
public bool IsDFA()
{
	// DFA if every transition leads to exactly one state.
	return Delta.Values.All(v => v.Count == 1);
}
```

- Convert a NFA to DFA

```csharp
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
```

- In continuity, we analyze the `GenerateGraph()` method from the `FiniteAutomaton` class, which is responsible for creating a graphical representation of the finite automaton and exporting it as an image.
- The method first ensures that a `Graphs` directory exists for storing the output files. It generates a unique filename suffix based on the current timestamp to avoid overwriting previous files.
- The core functionality involves constructing a DOT representation of the finite automaton, which is a textual format used by GraphViz to create visual graphs. The method initializes the DOT structure, setting the rank direction (`LR` for left-to-right) and defining the nodes' appearance.
- Each state in the automaton is added as a node in the DOT file. Final states are represented using a double-circle shape, while other states are displayed as standard circles. An invisible initial node is introduced to properly indicate the starting state.
- The method then iterates through the transition function (`Delta`) to define directed edges between states, with labels representing input symbols.
- Once the DOT representation is complete, it is written to a file. The method then attempts to execute the `dot` command from GraphViz to generate a PNG image from the DOT file.
- If the command executes successfully, the generated image path is displayed in the console. Otherwise, an error message is printed, suggesting that the user check their GraphViz installation and PATH configuration.

```csharp
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
```

- Last but not least, we analyze the `CheckType()` method from the `Grammar` class, which is responsible for determining the Chomsky hierarchy classification of a given grammar based on its production rules.
- The method first checks whether any production rules exist. If not, it defaults to returning `3`, assuming a type-3 (regular) grammar as a fallback.
- A list of potential grammar types is initialized to keep track of the classification of different rules. Additionally, two Boolean flags, `seenRightLinear` and `seenLeftLinear`, are used to detect mixed linearity, which would disqualify the grammar from being type-3.
- The method iterates through each production rule, first identifying whether the left-hand side (LHS) contains at least one non-terminal. This ensures that only valid grammar rules are considered.
- If the LHS contains more than one symbol, the rule is evaluated for type-1 (context-sensitive) classification. The method ensures that every right-hand side (RHS) production is at least as long as the LHS, otherwise, the rule is marked as type-0 (unrestricted).
- If the LHS consists of a single non-terminal, the rule is evaluated for type-3 (regular) classification. The method checks for epsilon (`ε`) productions, ensuring they do not invalidate the regular structure. It also examines the position of non-terminals in the RHS to determine whether the grammar is left-linear or right-linear.
- If both left-linear and right-linear rules are detected, the grammar is downgraded to type-2 (context-free), since regular grammars must be exclusively one or the other.
- After evaluating all rules, the method returns the most restrictive (minimum) type detected among them. If no valid classification is found, it returns `-1`, indicating an invalid grammar.

```csharp
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
			// If the rule consists solely of ε and lhs is not S, mark as type 2.
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
```

## Conclusions / Screenshots / Results
****
In this section, besides the conclusion that gets described at the end, an example of output, resulted from the execution of the program, is also explained in-depth. 

- The output begins by showing the current Finite Automaton that was declared based on the Variant number, in this case Variant 1. In order to print it in this way, the program uses the method `ToString()` that was overridden in the previous laboratory.

```
Q = {{q0}, {q1}, {q2}, {q3}}
Σ = {a, b, c}
        δ({q0}, a) = {q0,q1}
        δ({q1}, c) = {q1}
        δ({q1}, b) = {q2}
        δ({q2}, b) = {q3}
        δ({q3}, a) = {q1}
q0 = q0
q_F = {{q2}}
```

- In continuity, after doing the call to `ToRegularGrammar()` method, it creates an instance of a regular grammar based on it, and prints it as well using the overridden `ToString()` method.

```
V_N = {{q0}, {q1}, {q2}, {q3}}
V_T = {a, b, c}
P = {
        {q0} ---> a {q0, q1}
        {q1} ---> c {q1} | b {q2} | b ε
        {q2} ---> b {q3}
        {q3} ---> a {q1}
}
S = q0
```

- Then, we call the `IsDFA()` method on the `FiniteAutomaton` instance, that checks whether the FA is deterministic or not. 

```
Is the FA deterministic? False
```

- After that action, we are calling the `ToDFA()` method that creates another instance of a `FiniteAutomaton`, specifically a deterministic one, based on the non-deterministic `FiniteAutomaton` instance, while also printing it to the console.

```
Q = {{q0}, {q0, q1}, {q2}, {q1}, {q3}}
Σ = {a, b, c}
        δ({q0}, a) = {q0,q1}
        δ({q0,q1}, a) = {q0,q1}
        δ({q0,q1}, b) = {q2}
        δ({q0,q1}, c) = {q1}
        δ({q2}, b) = {q3}
        δ({q1}, b) = {q2}
        δ({q1}, c) = {q1}
        δ({q3}, a) = {q1}
q0 = q0
q_F = {{q2}}
```

- After instantiating the deterministic `FiniteAutomaton` object, we generate the graph, using the GraphViz tool  for the obtained FA, which gets saved in the *Graphs* folder,.

```
DOT file generated at: Graphs\FiniteAutomaton_20250303_131855549.dot
Graph generated successfully as "Graphs\FiniteAutomaton_20250303_131855549.png".
```

- Then we generate the `Grammar` instance for the obtained DFA, using the `ToRegularGrammar()` method once again.

```
V_N = {{q0}, {q0, q1}, {q2}, {q1}, {q3}}
V_T = {a, b, c}
P = {
        {q0} ---> a {q0, q1}
        {q0,q1} ---> a {q0, q1} | b {q2} | b ε | c {q1}
        {q2} ---> b {q3}
        {q1} ---> b {q2} | b ε | c {q1}
        {q3} ---> a {q1}
}
S = q0
```

-  Next, the program focuses on full-filling the Task no.1 where we have to check the grammar types based on Chomsky Hierarchy.
- Here we have an example for when the grammar **Type** is **0**, as in the section `Checking rule: LHS = BC, RHS = B` we can observe that the left-hand-side has less variables than the right-hand-side, which doesn't satisfy the requirements for type 1, 2, and the right-hand-side has more variables than one, so it cannot be type 2 or 3.

```
--- Instantiate Your Grammar for Type Checking---

Non-Terminals: S, B, C
Terminals: a, b
Start: S
INFO:
        - '|' to show more possiblities for the RHS
        - 'cancel' to restart the addition of rules

--- Enter the Prodution Rule #1 ---
LHS: S
RHS: aSBC
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #2 ---
LHS: BC
RHS: B
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #3 ---
LHS: C
RHS: a
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #4 ---
LHS: B
RHS: b
Are you adding more rules? [Anything/no]?
Answer: no

Checking rule: LHS = S, RHS = aSBC
Checking rule: LHS = BC, RHS = B
Checking rule: LHS = C, RHS = a
Checking rule: LHS = B, RHS = b

Type: 0
```

- Here we have an example of when the grammar input is of **Type 2** as it cannot be of type 3 since on the left-hand-side the linear consistency is not kept, as it mixes both *right* and *left* linear combinations.

```
--- Instantiate Your Grammar for Type Checking---
Non-Terminals: S
Terminals: a, b, d
Start: S
INFO:
        - '|' to show more possiblities for the RHS
        - 'cancel' to restart the addition of rules

--- Enter the Prodution Rule #1 ---
LHS: S
RHS: aS | b | Sd | _
Are you adding more rules? [Anything/no]?
Answer: no
Checking rule: LHS = S, RHS = aS | b | Sd | _

Type: 2
```

- Now, we are going to analyze an example where the grammar type is identified correctly as being of **Type 1**, since the variables count on left-hand-side are equal to or more than the ones from the right-hand-side, and the right-hand-side has more than one variable.

```
--- Instantiate Your Grammar for Type Checking---

Non-Terminals: S, A, B, C
Terminals: a, b
Start: S
INFO:
        - '|' to show more possiblities for the RHS
        - 'cancel' to restart the addition of rules

--- Enter the Prodution Rule #1 ---
LHS: S
RHS: ACA
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #2 ---
LHS: AC
RHS: AACA | ABa | AaB
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #3 ---
LHS: B
RHS: AB | A
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #4 ---
LHS: A
RHS: a | b

Are you adding more rules? [Anything/no]?
Answer: no
Checking rule: LHS = S, RHS = ACA
Checking rule: LHS = AC, RHS = AACA | ABa | AaB
Checking rule: LHS = B, RHS = AB | A
Checking rule: LHS = A, RHS = a | b

Type: 1
```

- Last but not least, we check a grammar of **Type 3**, where the we have a right-linear consistency. Once the program detects that the grammar is of type 3, it creates a `FiniteAutomaton` based on it.

```
--- Instantiate Your Grammar for Type Checking---

Non-Terminals: S, P, Q
Terminals: a, b, c, d, e, f
Start: S
INFO:
        - '|' to show more possiblities for the RHS
        - 'cancel' to restart the addition of rules

--- Enter the Prodution Rule #1 ---
LHS: S
RHS: aP | bQ
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #2 ---
LHS: P
RHS: bP | cP | dQ | e
Are you adding more rules? [Anything/no]?
Answer: yes

--- Enter the Prodution Rule #3 ---
LHS: Q
RHS: eQ | fQ | a
Are you adding more rules? [Anything/no]?
Answer: no

Checking rule: LHS = S, RHS = aP | bQ
Checking rule: LHS = P, RHS = bP | cP | dQ | e
Checking rule: LHS = Q, RHS = eQ | fQ | a

Type: 3
V_N = {{S}, {P}, {Q}}
V_T = {a, b, c, d, e, f}
P = {
        {S} ---> a {P} | b {Q}
        {P} ---> b {P} | c {P} | d {Q} | e ε
        {Q} ---> e {Q} | f {Q} | a ε
}
S = S

Q = {{S}, {P}, {Q}, {q_F}}
Σ = {a, b, c, d, e, f}
        δ({S}, a) = {P}
        δ({S}, b) = {Q}
        δ({P}, b) = {P}
        δ({P}, c) = {P}
        δ({P}, d) = {Q}
        δ({P}, e) = {q_F}
        δ({Q}, e) = {Q}
        δ({Q}, f) = {Q}
        δ({Q}, a) = {q_F}
q0 = S
q_F = {{q_F}}
```

- A visualization of my `Deterministic FiniteAutomaton` graph, that was generated using GraphvViz tool, the script of which was generated based on the obtained DFA.

<p align="center">
  <img width="600" src="../Graphs/FiniteAutomaton_20250303_140749281finite_automaton_graph_variant_one.png" alt="Finite Automaton Graph"/>
</p>

In conclusion, while executing this laboratory work, with the topic of *Intro to formal languages, Regular grammars, Finite Automata*, I have managed to understand better the whole concept of alphabets, languages, their types and differences, and what makes one formal, grammars and FA. Moreover, the main objectives of this laboratory have been fulfilled, as I have provided a setup for my evolving project, a GitHub repo, that I will work on during this semester, and I have also chosen a programming language, C#, in order to execute the tasks. According to my variant, number one, I have implemented a class for my `Grammar` and one for the `FiniteAutomaton`, together with two methods in the `Grammar` class, one `GenerateString()` that is used to generate valid strings according to the production rules, and the other method is `ToFiniteAutomaton()` that returns a `FiniteAutomaton` instance based on the existent `Grammar` definition. In the other class, `FiniteAutomaton`, a method named `StringBelongToLanguage()` has also been implemented, having the function to check if an input string can be obtained via the state transition from the FA instance. On top of that, I got a better grasp of what a Regular Grammar is and how it differs from the other types, together with the process of converting it to a Finite Automaton, and how a FA works. Last but not least, I have used an external tool [[5]](#ref5), a visual editor for Graphviz, in order to create a visual representation of my Finite Automaton graph, to better visualize the transitions among the states.

## References
****

<a id="ref1"></a>[1] Laboratory Work 1: Intro to formal languages. Regular grammars. Finite Automata. task.md - Crețu Dumitru, Drumea Vasile, Cojuhari Irina - 
https://github.com/filpatterson/DSL_laboratory_works/blob/master/1_RegularGrammars/task.md

<a id="ref2"></a>[2] "Formal Languages and Finite Automata guide for practical lessons" - COJUHARI Irina, DUCA Ludmila, FIODOROV Ion - 
https://else.fcim.utm.md/pluginfile.php/110458/mod_resource/content/0/LFPC_Guide.pdf

<a id="ref3"></a>[3] Presentation on "Formal Languages and Compiler Design" - conf. univ., dr. Irina Cojuhari - 
https://else.fcim.utm.md/pluginfile.php/110457/mod_resource/content/0/Theme_1.pdf

<a id="ref4"></a>[4] Presentation on "Regular Language. Finite Automata" - TUM - 
https://drive.google.com/file/d/1rBGyzDN5eWMXTNeUxLxmKsf7tyhHt9Jk/view

<a id="ref5"></a>[5] Graphviz Visual Editor - magjac Interactive - https://magjac.com/graphviz-visual-editor/