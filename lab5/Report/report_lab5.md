# Topic: Chomsky Normal Form
****
### Course: Formal Languages & Finite Automata
### Author: Maxim Alexei
----
## Theory:
****
### Context-Free Grammars

&ensp;&ensp;&ensp; A **Context-Free Grammar (CFG)** is a formal grammar consisting of a set of production rules that describe all possible strings in a given formal language. CFGs are important in both theoretical computer science and natural language processing.

A context-free grammar G is defined by a 4-tuple G = (V, Σ, R, S) where:
- V is a finite set of non-terminal symbols
- Σ is a finite set of terminal symbols (disjoint from V)
- R is a finite set of production rules of the form A → α, where A ∈ V and α ∈ (V ∪ Σ)*
- S ∈ V is the start symbol

&ensp;&ensp;&ensp; CFGs are more powerful than regular grammars and can describe languages that regular expressions cannot, such as a^n b^n (n ≥ 1).

### What is Chomsky Normal Form?

&ensp;&ensp;&ensp; **Chomsky Normal Form (CNF)** is a simplified form of context-free grammar where all production rules are of the following forms:
1. A → BC, where A, B, and C are non-terminal symbols (B and C cannot be the start symbol)
2. A → a, where A is a non-terminal symbol and a is a terminal symbol
3. S → ε (only if S is the start symbol and S does not appear on the right side of any rule)

CNF is particularly useful because:
- It simplifies many algorithms on CFGs
- It is essential for the CYK (Cocke-Younger-Kasami) parsing algorithm
- It eliminates complex productions while preserving the language generated
- It makes proofs about CFGs more straightforward
### Algorithm to Convert CFG to CNF

1. **Start Symbol Isolation**: If the start symbol S appears on the right side of any production, create a new start symbol S' and add the production S' → S.
2. **Eliminate ε-productions**: Remove all productions of the form A → ε (except possibly S → ε if empty string is in the language).
3. **Eliminate Unit Productions**: Remove all productions of the form A → B where both A and B are non-terminal symbols.
4. **Remove Inaccessible Symbols**: Eliminate all non-terminals that cannot be reached from the start symbol.
5. **Remove Non-productive Symbols**: Eliminate all non-terminals that cannot derive a string of terminals.
6. **Convert Long Productions**: Replace each production A → B₁B₂...Bₙ (where n > 2) with:
    - A → B₁C₁
    - C₁ → B₂C₂
    - ...
    - Cₙ₋₂ → Bₙ₋₁Bₙwhere C₁, C₂, ..., Cₙ₋₂ are new non-terminal symbols.
7. **Handle Terminal-Nonterminal Combinations**: For each production A → aB or A → Ba or A → aBb..., where a, b are terminals and B is a non-terminal, replace the terminal with a new non-terminal:
    - Add X → a for each terminal a
    - Replace A → aB with A → XB
    - Replace A → Ba with A → BX
    - And so on.
## Applications of CNF

1. **Parsing Algorithms**: CNF is essential for the CYK parsing algorithm, which determines if a string belongs to a context-free language in O(n³) time.
2. **Natural Language Processing**: Used in computational linguistics for parsing natural language sentences.
3. **Formal Language Theory**: Simplifies proofs and algorithms on context-free languages.
4. **Compiler Design**: Helps in the design of parsers for programming languages.

## Objectives:
****
1. Learn about Chomsky Normal Form (CNF) [1].
2. Get familiar with the approaches of normalizing a grammar.
3. Implement a method for normalizing an input grammar by the rules of CNF.
    i. The implementation needs to be encapsulated in a method with an appropriate signature (also ideally in an appropriate class/type).
    ii. The implemented functionality needs executed and tested.
    iii. Also, another **BONUS point** would be given if the student will make the aforementioned function to accept any grammar, not only the one from the student's variant, the first one.

## Implementation description
****
##### The logic within the `Program.cs`
&ensp;&ensp;&ensp; The program starts via the initialization of my grammar, where I initialize the non-terminal and terminal symbols, together with the production rules, based on the received variant, the first one. However, any grammar from any variant is accepted as well, just as the BONUS task has required.

```csharp
// Variant 1
var grammar = new Grammar(
    vN: new HashSet<string>() {"S", "A", "B", "C", "D", "E"},
    vT: new HashSet<char>() {'a', 'b'},
    p:  new Dictionary<string, List<string>>() {
        ["S"] = new List<string>() {"aB", "AC"},
        ["A"] = new List<string>() {"a", "ASC", "BC", "aD"},
        ["B"] = new List<string>() {"b", "bS"},
        ["C"] = new List<string>() {"BA", "ε"},
        ["E"] = new List<string>() {"aB"},
        ["D"] = new List<string>() {"abC"}
    },
    s: "S"
);
```

&ensp;&ensp;&ensp; Next, I just print the visualization of the grammar before converting it to CNF, then I simply call the static method `Obtain()` in order to execute the conversion. Then, I simply call the overridden `ToString()` method on the `Grammar` instance, in order to visualize the `grammar` after the conversion from CFG to CNF.

```csharp
// Before CNF
Console.WriteLine("\nGrammar before modifications:");
Console.WriteLine(grammar.ToString())

// Performing CNF
ChomskyNormalForm.Obtain(grammar);

// After CNF
Console.WriteLine("\nGrammar after bringing it to CNF:");
Console.WriteLine(grammar.ToString());
```

##### The `Grammar` template
&ensp;&ensp;&ensp; Efficiently repurposing code from previous work, I've _recycled_ the same `Grammar` class implementation from the first laboratory for this CNF assignment. This decision proved advantageous as the existing class structure works _perfectly_ for representing and manipulating context-free grammars. The class with its properties `VN`, `VT`, `P`, and `S` already provides all the necessary components to define and work with any grammar structure.

&ensp;&ensp;&ensp; This approach significantly enhances flexibility by allowing me to implement _any_ grammar definition, not just the specific variant assigned in my task. The reused `Grammar` class enables seamless creation of different grammar configurations, making it straightforward to convert various CFGs to Chomsky Normal Form and test the algorithm with different examples.

```csharp
public class Grammar(HashSet<string> vN, HashSet<char> vT, Dictionary<string, List<string>> p, string s)
{
	public HashSet<string> VN {get; private set;} = vN;
	public HashSet<char> VT {get; private set;} = vT;
	public Dictionary<string, List<string>> P {get; set;} = p;
	public string S {get; set;} = s;

	public string GenerateString()
	{
		StringBuilder current = new StringBuilder(S); // current = "aa"
		Console.Write("\nWord Build: {0}", current);

		while(true)
		{
			// ...
		}
		return current.ToString();
	}

	public override string ToString()
	{
		// Outputing the definition to console
		// ...
		return string.Format("{0}{1}{2}{3}", vNData, vTData, pData, sData);
	}
}
```

##### The `ChomskyNormalForm` implementation
&ensp;&ensp;&ensp; The `Obtain` method begins with a critical step in the Chomsky Normal Form conversion process by checking if the grammar's start symbol appears on the right-hand side of any production rule. This check is _essential_ because CNF requires that the start symbol never appears on the right side of any production. The code efficiently iterates through all productions, examining each right-hand side to detect any occurrences of the start symbol and sets a boolean flag when found.

&ensp;&ensp;&ensp; When the start symbol is detected on a right-hand side, the method elegantly implements the first transformation rule by creating a new unique non-terminal symbol using the `GetNextAvailableLetter` helper function. The new symbol is _carefully added_ to the grammar's non-terminal set, and a new production rule connecting it to the original start symbol is created. This transformation preserves the language generated by the grammar while ensuring that the resulting structure complies with CNF's strict requirements for the start symbol's usage.

```csharp
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
	// ...
}
```

&ensp;&ensp;&ensp; At the same time, the method implements a comprehensive debugging approach by displaying the grammar's current state after each transformation step, enabling developers to trace the conversion process with clarity. The console output reveals how the grammar evolves through each phase, making it easier to verify correctness and identify potential issues during the CNF conversion. This _transparent approach_ to transformation provides valuable insights into the complex grammar restructuring process.

&ensp;&ensp;&ensp; Following the start symbol adjustment, the implementation methodically applies the four fundamental CNF cleanup steps in precise sequence. The code _systematically_ eliminates empty productions (ε-rules), removes unit rules (A → B), prunes inaccessible symbols, and finally cleans up non-productive symbols. Each cleanup function encapsulates a specific transformation aspect, creating a well-structured and maintainable approach to the CNF conversion process.

```csharp
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
```

&ensp;&ensp;&ensp; Also, the terminal replacement phase addresses a core CNF requirement by ensuring terminals only appear in dedicated single-symbol productions through an elegant dictionary-based approach. The code creates a `terminalToNonTerminal` mapping that efficiently tracks substitutions and prevents redundant non-terminal creation when the same terminal appears multiple times. This _intelligent reuse_ of non-terminals minimizes grammar size while maintaining the language's generative power.

&ensp;&ensp;&ensp; For each production containing terminals mixed with non-terminals, the method performs careful character-by-character analysis to identify and replace terminals with their corresponding non-terminal substitutes. The implementation _preserves single-terminal productions_ (A → a) while transforming mixed productions by generating unique non-terminal symbols and creating appropriate production rules. This transformation ensures strict compliance with CNF's requirement that terminals only appear in isolation, preparing the grammar for the final binary production conversion step.

```csharp
// Step 1: Replace each terminal in the RHS with a new non-terminal
Dictionary<char,string> terminalToNonTerminal = new Dictionary<char, string>();
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
			// ...
		}
		newP[lhs].Add(newRhs);
	}
}
grammar.P = newP;

// Dictionary to map production RHS to existing non-terminals
Dictionary<string, string> rhsToNonTerminal = new Dictionary<string, string>();
```

&ensp;&ensp;&ensp; Morever, the binary production conversion phase transforms any production with more than two symbols on the right-hand side into a series of binary productions, fulfilling a critical CNF requirement. For each production A → B₁B₂...Bₙ where n > 2, the code _iteratively creates_ new intermediate non-terminals and corresponding binary productions. This recursive decomposition maintains the original grammar's generative power while ensuring every production contains at most two non-terminals on the right-hand side.

&ensp;&ensp;&ensp; The implementation intelligently reuses non-terminals through the `rhsToNonTerminal` dictionary to avoid creating redundant symbols for identical right-hand side fragments. After processing all multi-symbol productions, the code _carefully preserves_ existing productions for reused non-terminals by copying them to the new production set. This attention to consistency ensures that all referenced non-terminals have properly defined productions while minimizing the grammar size.

```csharp
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
			// ...
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
```

&ensp;&ensp;&ensp; The final optimization phase implements an intelligent merging strategy to eliminate redundant productions by tracking unique right-hand sides across the grammar. The `productionToNonTerminal` dictionary _efficiently maps_ each unique binary production to a specific non-terminal, identifying opportunities to reuse existing symbols. This deduplication process produces a more compact grammar while preserving the language's generative capabilities and maintaining CNF compliance.

&ensp;&ensp;&ensp; The optimization carefully handles both two-symbol and single-symbol productions through distinct processing strategies tailored to each type. For binary productions (A → BC), the code checks whether an equivalent production already exists elsewhere in the grammar before adding it. Single-symbol productions (A → a) receive _simpler treatment_, with the algorithm merely ensuring uniqueness within each non-terminal's production set. This differential processing creates an optimized CNF grammar with minimal redundancy.

```csharp
{
	// ...
	// Step 3: Final optimization to merge duplicate right-hand sides
	var finalP = new Dictionary<string, List<string>>();
	var productionToNonTerminal = new Dictionary<string, string>();

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
		    // For two-symbol productions, check if we can reuse an existing vT
			if (rhs.Length == 2)
			{
				// ...
			}
			else
			{
				// For single symbol productions, we just add them
				if (!finalP[lhs].Contains(rhs)) finalP[lhs].Add(rhs);
			}
		}
	}
	grammar.P = finalP;
}
```

&ensp;&ensp;&ensp; Furthermore, the `GetNextAvailableLetter` method provides a simple yet effective mechanism for generating unique non-terminal symbols during the CNF conversion process. Starting with the letter 'G' as specified in the problem statement, the function _systematically_ traverses the alphabet until it finds a character not currently used in the grammar's non-terminal set. This approach ensures that newly created non-terminals don't conflict with existing symbols, maintaining the grammar's integrity throughout the transformation.

```csharp
private static string GetNextAvailableLetter(Grammar grammar)
{
	char letter = 'G';
	while (grammar.VN.Contains(letter.ToString()))
	{
		letter++;
	}
	return letter.ToString();
}
```

&ensp;&ensp;&ensp;  Next, the `EliminateEmptyProductions` method begins by identifying all nullable variables—non-terminals that can derive the empty string—through a two-phase detection approach. The first phase _directly identifies_ non-terminals with explicit epsilon productions (A → ε), adding them to the nullable set. This initial scan establishes the foundation for identifying the complete set of nullable variables in the grammar.

&ensp;&ensp;&ensp;  The method then extends this identification through an iterative process to discover indirectly nullable variables—those that can derive the empty string through combinations of other nullable variables. Using a fixed-point algorithm with a `changed` flag, the code _repeatedly examines_ productions until no new nullable variables are found. This comprehensive approach ensures that all variables that could potentially derive empty strings are properly identified, including complex cases where nullability is derived through chains of productions.

&ensp;&ensp;&ensp;  After identifying all nullable variables, the method generates a new set of productions by systematically creating all possible combinations that result from removing nullable elements. For each production that isn't a direct epsilon rule, the code _intelligently generates_ all possible subsets of the right-hand side by removing nullable variables in different combinations. This transformation preserves the language generated by the grammar while eliminating epsilon productions, a crucial step toward achieving Chomsky Normal Form.

```csharp
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
					// ...
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
				// ...
			}
		}
	}
	grammar.P = newP;
}
```

&ensp;&ensp;&ensp; Then, the `EliminateAnyUnitRules` method tackles a critical requirement of Chomsky Normal Form by removing unit productions—rules where one non-terminal directly derives another single non-terminal. The implementation begins by constructing a `unitDerivations` dictionary that _initially_ maps each non-terminal to itself, establishing the reflexive property needed for the transitive closure calculation. This data structure efficiently tracks which non-terminals can be derived from each other through chains of unit productions.

&ensp;&ensp;&ensp;  Using an elegant fixed-point algorithm, the method computes the complete transitive closure of unit productions by repeatedly examining the grammar until no new derivation relationships are discovered. For each production A → B where B is a single non-terminal, the code _propagates_ all of B's possible derivations to A, capturing multi-step unit derivation chains. This comprehensive approach ensures that all possible unit derivation paths are identified, regardless of their length or complexity.

&ensp;&ensp;&ensp;  The final transformation replaces unit productions with their non-unit alternatives, effectively eliminating all unit rules while preserving the language generated by the grammar. For each non-terminal and its derived non-terminals (through unit rules), the method _selectively incorporates_ only non-unit productions from the derived symbols. This substitution ensures that all strings previously generated through unit production chains can still be derived directly, satisfying a key requirement for Chomsky Normal Form while maintaining the language's generative power.

```csharp
private static void EliminateAnyUnitRules(Grammar grammar)
{
	var unitDerivations = new Dictionary<string, HashSet<string>>();

	// Initialize unitDerivations for each non-terminal
	foreach (var nonTerminal in grammar.VN)
		unitDerivations[nonTerminal] = new HashSet<string> { nonTerminal };

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
				// ...
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
			// ...
		}
	}
	grammar.P = newP;
}
```

&ensp;&ensp;&ensp; Also, the `EliminateInaccessibleSymbols` method implements an essential grammar cleanup step that removes non-terminals that cannot be reached from the start symbol in any derivation. Beginning with only the start symbol marked as accessible, the method _systematically explores_ the grammar's production rules to identify all reachable non-terminals. This reachability analysis ensures that the resulting grammar contains only symbols that can actually participate in the language's derivations.

&ensp;&ensp;&ensp; Using a fixed-point algorithm with a `changed` flag, the method iteratively expands the set of accessible symbols until no new reachable symbols are found. For each accessible non-terminal, the code _carefully examines_ all of its production rules to identify additional non-terminals that become reachable. Once all accessible symbols are identified, the method efficiently removes inaccessible symbols by computing the set difference between all non-terminals and accessible ones, then pruning both the non-terminal set and production dictionary accordingly. This optimization reduces the grammar's size while preserving its generated language.

```csharp
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
				// ...
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
```

&ensp;&ensp;&ensp; Last but not least, the `EliminateNonProductiveSymbols` method implements a critical grammar optimization by removing non-terminals that cannot derive any string composed solely of terminal symbols. The implementation uses a two-phase approach to identify productive symbols, first _detecting directly productive_ non-terminals that can immediately derive terminal strings. It then iteratively discovers indirectly productive symbols through a fixed-point algorithm that continues until no new productive symbols are found, ensuring complete identification of all non-terminals capable of generating terminal strings.

&ensp;&ensp;&ensp; After identifying all productive symbols, the method performs a comprehensive cleanup to remove non-productive elements from the grammar. First, it _efficiently calculates_ the set of non-productive symbols by finding the difference between all non-terminals and the productive ones. The method then not only removes the non-productive symbols and their associated productions, but also _carefully filters_ all remaining productions to eliminate any that contain non-productive symbols. This thorough approach ensures the resulting grammar contains only useful productions that can contribute to the language, a crucial property for Chomsky Normal Form.

```csharp
private static void EliminateNonProdcutiveSymbols(Grammar grammar)
{
	HashSet<string> productiveSymbols = new HashSet<string>();

	// Find directly productive symbols (non-terminals that derive terminals)
	foreach (var pair in grammar.P)
	{
		foreach (var rhs in pair.Value)
		{
			// ...
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
					// ...
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
		// ...
	}

	grammar.P = newP;
}
```

## Conclusions / Screenshots / Results
****
&ensp;&ensp;&ensp; In this section, besides the conclusion that gets described at the end, an example of output, resulted from the execution of the program, is also explained in-depth. 

```
Grammar before modifications:
V_n = {S, A, B, C, D, E}
V_t = {a, b}
P = {   S ---> aB | AC
        A ---> a | ASC | BC | aD
        B ---> b | bS
        C ---> BA | ε
        E ---> aB
        D ---> abC
}
S = {S}


Grammar after adding a new start symbol:
V_n = {S, A, B, C, D, E, G}
V_t = {a, b}
P = {   S ---> aB | AC
        A ---> a | ASC | BC | aD
        B ---> b | bS
        C ---> BA | ε
        E ---> aB
        D ---> abC
        G ---> S
}
S = {G}


Grammar after eliminating empty prods:
V_n = {S, A, B, C, D, E, G}
V_t = {a, b}
P = {   S ---> aB | AC | A
        A ---> a | ASC | AS | BC | B | aD
        B ---> b | bS
        C ---> BA
        E ---> aB
        D ---> abC | ab
        G ---> S
}
S = {G}


Grammar after eliminating unit rules:
V_n = {S, A, B, C, D, E, G}
V_t = {a, b}
P = {   S ---> aB | AC | a | ASC | AS | BC | aD | b | bS
        A ---> a | ASC | AS | BC | aD | b | bS
        B ---> b | bS
        C ---> BA
        D ---> abC | ab
        E ---> aB
        G ---> aB | AC | a | ASC | AS | BC | aD | b | bS
}
S = {G}


Grammar after eliminating inaccessible symbols:
V_n = {S, A, B, C, D, G}
V_t = {a, b}
P = {   S ---> aB | AC | a | ASC | AS | BC | aD | b | bS
        A ---> a | ASC | AS | BC | aD | b | bS
        B ---> b | bS
        C ---> BA
        D ---> abC | ab
        G ---> aB | AC | a | ASC | AS | BC | aD | b | bS
}
S = {G}


Grammar after eliminating non-productive symbols:
V_n = {S, A, B, C, D, G}
V_t = {a, b}
P = {   S ---> aB | AC | a | ASC | AS | BC | aD | b | bS
        A ---> a | ASC | AS | BC | aD | b | bS
        B ---> b | bS
        C ---> BA
        D ---> abC | ab
        G ---> aB | AC | a | ASC | AS | BC | aD | b | bS
}
S = {G}


Grammar after bringing it to CNF:
V_n = {S, A, B, C, D, H, G, I, J, K}
V_t = {a, b}
P = {   S ---> HB | AC | a | AJ | AS | BC | HD | b | IS
        J ---> SC
        H ---> a
        I ---> b
        A ---> a | AJ | AS | BC | HD | b | IS
        B ---> b | IS
        C ---> BA
        D ---> HK | HI
        K ---> IC
        G ---> HB | AC | a | AJ | AS | BC | HD | b | IS
}
S = {G}
```

&ensp;&ensp;&ensp; In conclusion, my implementation successfully accomplishes all required tasks for converting a context-free grammar to Chomsky Normal Form through a methodical step-by-step approach. Each transformation stage is clearly displayed in the output, demonstrating how the grammar evolves through the process - from adding a new start symbol to eliminating empty productions, unit rules, inaccessible symbols, and non-productive symbols. This transparent approach provides valuable insight into the inner workings of the CNF conversion algorithm and helps validate the correctness of each transformation step.

&ensp;&ensp;&ensp; Moreover, the comprehensive transformation process effectively converted the original grammar with mixed-length productions and epsilon rules into a well-formed CNF grammar that strictly adheres to the required format. So, my solution intelligently handles all special cases, including the introduction of new non-terminals (H, I, J, K) to replace terminals in complex productions and break down productions with more than two symbols on the right-hand side. The final grammar demonstrates how the algorithm successfully preserves the language while restructuring all productions into the required CNF format.

&ensp;&ensp;&ensp; By displaying each intermediate grammar transformation, it provides an excellent educational tool for understanding the CNF conversion process. The progression from the initial grammar through various cleanup stages to the final CNF form clearly illustrates how each algorithm step contributes to the overall transformation. This approach not only verifies the correctness of the implementation but also helps visualize the theoretical concepts of formal language theory in practice, showing exactly how a grammar evolves through the standardization process.

&ensp;&ensp;&ensp; Last but not least, the final CNF grammar, while containing more non-terminals than the original, now possesses the structural properties that make it suitable for efficient parsing algorithms. So, it balances the competing concerns of correctness, efficiency, and readability by employing intelligent symbol reuse strategies to minimize the explosion of new non-terminals. Thus, the resulting grammar maintains the same generative power as the original while conforming to the strict structural requirements of Chomsky Normal Form, demonstrating a successful application of formal language transformation techniques.

## References
****

<a id="ref3"></a>[1] Presentation on "Formal Languages and Compiler Design" - conf. univ., dr. Irina Cojuhari - 
https://else.fcim.utm.md/pluginfile.php/110457/mod_resource/content/0/Theme_1.pdf

<a id="ref4"></a>[2] Presentation on "Context Free Grammars" - TUM - 
https://drive.google.com/file/d/19muyiabGeGaoNDK-7PeuzYYDe6_c0e-t/view

<a id="ref4"></a>[3] Chomsky Normal Form - Wikipedia - https://en.wikipedia.org/wiki/Chomsky_normal_form
