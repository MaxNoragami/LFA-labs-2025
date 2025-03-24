# Topic: Regular expressions
****
### Course: Formal Languages & Finite Automata
### Author: Maxim Alexei
----
## Theory:
****
&ensp;&ensp;&ensp; In formal language theory, **Regular Grammars** define a class of grammars that generate **regular languages**. These languages are closely tied to **Finite Automata** and **Regular Expressions**. Regular Grammars establish the rules for producing regular languages, while Finite Automata offer a mechanism to recognize them. Regular Expressions, however, are widely used in practical applications like text searching and manipulation due to their flexibility and expressiveness. This response briefly covers Regular Grammars and Finite Automata before delving into Regular Expressions.
##### Regular Grammars and Finite Automata
- **Regular Grammars**: These grammars feature production rules with a specific structure, typically having a single non-terminal on the left and either a terminal or a terminal followed by a non-terminal on the right. They outline the framework of regular languages.
- **Finite Automata**: These are abstract machines with a limited number of states, shifting between them based on input symbols. They come in two forms—deterministic (DFA) and non-deterministic (NFA)—and are used to identify regular languages.

&ensp;&ensp;&ensp; Together, Regular Grammars and Finite Automata lay the theoretical groundwork for regular languages. For hands-on tasks, though, **Regular Expressions** often take the spotlight.
##### Regular Expressions
&ensp;&ensp;&ensp; **Regular Expressions**, often shortened to *regex*, are sequences of characters that specify search patterns, commonly employed in text processing for matching and manipulating strings. They play a vital role in computer science, appearing in tools from text editors to programming languages for tasks like validation, searching, and data extraction. A regular expression is constructed from basic elements and combined with operations to form robust patterns.
##### Basic Components
&ensp;&ensp;&ensp; Regular Expressions blend literal characters with special symbols, known as *metacharacters*, to create patterns. Here are the main components:
- **Literal characters**: These match themselves directly. For example, the regex `a` matches the string "a".
- **Metacharacters**: These symbols carry special meanings, enhancing regex versatility:
  - `.` (dot): Matches any single character (except newline in some cases).
  - `*` (star): Matches *zero or more* instances of the preceding character or group.
  - `+`: Matches *one or more* instances of the preceding character or group.
  - `?`: Matches *zero or one* instance of the preceding character or group.
  - `^`: Indicates the start of the string.
  - `$`: Indicates the end of the string.
- **Character classes**: Enclosed in square brackets `[]`, they match any single character from a defined set. For instance, `[a-z]` matches any lowercase letter, and `[0-9]` matches any digit.
- **Groups**: Parentheses `()` bundle parts of an expression, allowing operators to act on the entire group. For example, `(ab)` treats "ab" as a single unit.
##### Operations
To craft more intricate patterns, Regular Expressions rely on several operations:
- **Concatenation**: Placing two expressions side by side matches the first followed by the second. For example, `ab` matches "ab".
- **Union**: Represented by the `|` symbol, it matches either the expression before or after it. For instance, `a|b` matches "a" or "b".
- **Kleene star**: Applied with `*`, it matches *zero or more* occurrences of an expression. For example, `a*` matches "", "a", "aa", and so forth.

&ensp;&ensp;&ensp; These operations enable regex to describe complex patterns, making them invaluable for text processing.
##### Applications
Regular Expressions are essential across various computing domains:
- **Input validation**: Verifying formats like emails, phone numbers, or dates.
- **Text processing**: Searching and replacing text in editors or command-line tools.
- **Data parsing**: Extracting specific details from logs or structured text.
- **Programming**: Integrated into languages like Python, Java, and JavaScript.

&ensp;&ensp;&ensp; Though implementations might differ slightly across tools and languages, the fundamental principles of regex remain consistent, making it a universal skill for developers and researchers.

## Objectives:
****
1. Write and cover what regular expressions are, what they are used for;
2. Below you will find 3 complex regular expressions per each variant. Take a variant depending on your number in the list of students and do the following:
    a. Write a code that will generate valid combinations of symbols conform given regular expressions (examples will be shown). Be careful that idea is to interpret the given regular expressions dinamycally, not to hardcode the way it will generate valid strings. You give a set of regexes as input and get valid word as an output
    b. In case you have an example, where symbol may be written undefined number of times, take a limit of 5 times (to evade generation of extremely long combinations);
    c. **Bonus point**: write a function that will show sequence of processing regular expression (like, what you do first, second and so on)
Write a good report covering all performed actions and faced difficulties.

## Implementation description
****
##### The logic within the `Main()` method
&ensp;&ensp;&ensp; In the initial part of the `Main()` method, the code sets the stage for the regular expression processing by first defining a list of regex patterns. This snippet establishes the foundation of the application by initializing key variables and objects that will be used throughout the program. For example, the list of patterns is declared using a **strongly-typed** collection from the `System.Collections.Generic` namespace, ensuring that only strings are accepted. This careful initialization is crucial because it directly impacts how the rest of the logic behaves when parsing and generating valid combinations from these patterns.

&ensp;&ensp;&ensp;  Furthermore, this portion of the code is designed with clarity in mind. By clearly declaring the patterns in a dedicated block, the developer makes it easier to maintain and modify the regexes later if needed. The structured approach, which includes using a formatted output with the `$"..."` syntax, not only helps in debugging but also provides an intuitive way for the user to follow the processing flow. This initial setup, typically found in a file such as `Program.cs`, emphasizes the importance of well-organized code as the backbone of a robust application.

```csharp
List<string> patterns = new List<string> { 
	@"(a|b)(c|d)E+G?", 
	@"P(Q|R|S)T(UV|W|X)*Z+", 
	@"1(0|1)*2(3|4){5}36" 
};
```

&ensp;&ensp;&ensp; The following code snippet demonstrates the process of generating valid combinations based on a given regex pattern. In this section, the code employs a `RegexParser` in combination with a `RegexGenerator` to interpret and process the pattern dynamically. The parser is tasked with breaking down the regular expression into its constituent parts, while the generator then takes this parsed structure to produce a set of valid string combinations that conform to the regex. This approach avoids hardcoding any specific pattern matching logic, thereby emphasizing the flexibility and dynamism of the implementation.

&ensp;&ensp;&ensp; Additionally, the snippet shows how the code communicates the processing state to the user by printing out the pattern number and the associated regex pattern using formatted console output. The use of a `HashSet<string>` to store the valid combinations ensures that each output string is unique. This part of the code, likely located in a central file such as `Main.cs`, is critical for demonstrating how the system seamlessly transitions from parsing the regex to actually generating and displaying valid string examples. The clarity of this design also aids future developers in understanding the workflow without delving into lower-level implementation details.

```csharp
for (int i = 2; i < 3; i++)
{
	int countOfGeneratedStrings = 0;
	Console.WriteLine($"Pattern {i+1}: {patterns[i]}");
	RegexParser regexParser = new RegexParser();
	RegexGenerator regexGenerator = new RegexGenerator(regexParser);

	// Generating example combinations
	HashSet<string> validCombinations = regexGenerator.GenerateValidCombinations(patterns[i]).ToHashSet();
	
	Console.WriteLine($"Generated valid combinations:");
	foreach (var combo in validCombinations)
	{
		Console.WriteLine($" - {combo}");
		countOfGeneratedStrings++;
	}
	// ...
}
```

&ensp;&ensp;&ensp; Next, the focus shifts from generation to verification of the valid combinations produced earlier. The code creates a new instance of the `Regex` class by compiling the given regex pattern, which is then used to validate each generated string. This ensures that all outputs strictly match the defined pattern, thereby reinforcing the reliability of the generation process. By iterating over the generated combinations and applying the `IsMatch` method, the snippet robustly confirms the correctness of the results in a clear and concise manner.

&ensp;&ensp;&ensp;  The process is also made user-friendly by printing the results of the validation directly to the console. The output includes a boolean confirmation indicating whether all combinations are valid, as well as the total count of generated symbols. This dual feedback mechanism not only aids in debugging but also provides a transparent view of the internal workings of the program. Overall, this verification step, often integrated into a file like `Validator.cs`, underscores the importance of ensuring that dynamically generated content faithfully adheres to its original specification.

```csharp
for (int i = 2; i < 3; i++)
{
	// ...
	
	// Verifying generated combinations are valid
	Regex regex = new Regex($"^{patterns[i]}$", RegexOptions.Compiled);
	bool allValid = validCombinations.All(regex.IsMatch);
	Console.WriteLine($"All combinations valid: {allValid}");
	Console.WriteLine($"Total amount of generated symbols: {countOfGeneratedStrings}");
	Console.WriteLine();
	
	// ...
}
```

&ensp;&ensp;&ensp;  In continuity, the code snippet is dedicated to illustrating the parsing sequence of the regular expression. Here, the code makes a call to the parser to obtain the abstract syntax tree (AST) representation of the regex pattern, which is then displayed using a `RegexTreePrinter`. This step is crucial for understanding how the regex is decomposed into its individual components, which can include literals, alternations, and repetitions. By printing the tree structure, the program provides a visual insight into the parsing process, which is invaluable for debugging and learning purposes.

&ensp;&ensp;&ensp;  This approach not only highlights the inner workings of the regular expression parser but also reinforces the concept of _step-by-step_ processing in compiler design. The output, rendered in a clean and structured format, makes it clear how the parser interprets different segments of the pattern. Typically found in a file such as `RegexParser.cs`, this snippet is an excellent example of how visual representations can simplify the understanding of complex parsing algorithms, ultimately making the system more accessible to both developers and students.

```csharp
for (int i = 2; i < 3; i++)
{
	// ...
	
	// Showing parse sequence
	Console.WriteLine($"Processing sequence for pattern {i+1}:");
	RegexNode rootNode = regexParser.ParseRegex(patterns[i]);
	RegexTreePrinter.Print(rootNode, 0);
	Console.WriteLine("\n");
}
```

##### The acceptable Nodes/Tokens
&ensp;&ensp;&ensp;  Furthermore, the attached snippet of code defines an enumeration that categorizes the different types of nodes used in the regular expression’s abstract syntax tree. The `enum RegexNodeType` includes categories such as **Literal**, **Alternation**, **Concatenation**, and **Repetition**, which together provide a comprehensive framework for representing any regular expression. This classification is vital because it allows the rest of the program to process each node according to its specific behavior, enabling a modular approach to handling various regex operations.

&ensp;&ensp;&ensp;  By clearly delineating the possible node types, this code establishes a solid foundation for subsequent parsing and generation tasks. The use of an enumeration not only enforces type safety but also improves code readability by making the intent of each node type explicit. This definition, typically found in a file like `RegexNode.cs`, is a prime example of how thoughtful data structures can simplify the overall design of a complex system. With well-defined types, developers can more easily extend functionality or troubleshoot issues as they arise, ensuring the long-term maintainability of the application.

```csharp
enum RegexNodeType
{
	Literal,
	Alternation,
	Concatenation,
	Repetition
}
```

##### The class def of a `RegexNode`
&ensp;&ensp;&ensp; Moreover, the code defining the `RegexNode` class plays a vital role in representing the structure of a regular expression. This class encapsulates the fundamental elements needed to construct an abstract syntax tree (AST) for regex processing. It includes properties for the node type, its value, and a list of child nodes that represent subexpressions, as well as minimum and maximum repetition parameters. By providing a clear and organized way to store each part of a regex, this class enables both the parser and generator to work in unison and interpret the regular expression accurately.

&ensp;&ensp;&ensp; In continuity with the program’s design, this class promotes modularity and ease of maintenance. The well-defined properties allow developers to extend the system or troubleshoot issues without getting lost in a tangled codebase. Its straightforward structure, with clear types and default values, ensures that every regex component is represented consistently throughout the processing pipeline. This thoughtful design underlines the importance of robust data structures in complex systems like a regex engine.

```csharp
class RegexNode
{
	public RegexNodeType Type { get; set; }
	public string Value { get; set; } = "";
	public List<RegexNode> Children { get; set; } = new List<RegexNode>();
	public int MinRepeat { get; set; }
	public int MaxRepeat { get; set; }

	public RegexNode(RegexNodeType type) { Type = type; }
}
```

##### Demystifying the `RegexParser.cs`
&ensp;&ensp;&ensp;  Next, the snippet from `RegexParser.cs` that handles the parsing of expressions is a key element in breaking down the regular expression into its constituent parts. The method `ParseExpression` iterates over the pattern string and constructs a node tree that represents concatenation and alternation. When the parser encounters a pipe symbol (`|`), it correctly identifies the need to split the expression into alternatives, creating an alternation node that holds both the current and subsequent parts of the expression. This dynamic parsing approach ensures that the logical structure of the regex is captured comprehensively.

&ensp;&ensp;&ensp;  Furthermore, this method contributes significantly to the clarity of the overall process. By employing recursion, it neatly manages nested expressions and correctly interprets the grouping and order of operations. The clear separation of concerns—identifying when to stop parsing based on certain characters—exemplifies a disciplined approach to parser design. This makes it not only an effective tool for regex interpretation but also an educational example of how recursive descent parsers operate in compiler construction.

```csharp
private RegexNode ParseExpression(string pattern, ref int position)
{
	RegexNode concatNode = new RegexNode(RegexNodeType.Concatenation);
	while (position < pattern.Length)
	{
		char currentChar = pattern[position];
		if (currentChar == ')')
			break;

		if (currentChar == '|')
		{
			position++; // Skip '|'
			var alternationNode = new RegexNode(RegexNodeType.Alternation);
			alternationNode.Children.Add(concatNode);
			alternationNode.Children.Add(ParseExpression(pattern, ref position));
			return alternationNode;
		}

		RegexNode termNode = ParseTerm(pattern, ref position);
		concatNode.Children.Add(termNode);
	}
	return concatNode;
}
```

&ensp;&ensp;&ensp;  Continuing the discussion, the second part of the parsing process is demonstrated in the `ParseTerm` method. This segment of code handles individual units of the expression, whether they are grouped subexpressions or single literal characters. By checking if the current character is an opening parenthesis `(`, the method initiates a recursive call to process the enclosed expression, thereby correctly handling grouping constructs. This is crucial because groups often alter the interpretation of the regex, especially when combined with repetition operators.

&ensp;&ensp;&ensp;  Moreover, the handling of literals in this method is straightforward yet effective. If the current character is not a special character (like `(` or a repetition operator), it is treated as a literal. This clear distinction ensures that the parser accurately captures the intended meaning of the regular expression. The method's structured approach to advancing through the string and managing different scenarios provides a solid foundation for the overall regex parsing logic, demonstrating both efficiency and clarity.

```csharp
private RegexNode ParseTerm(string pattern, ref int position)
{
	char currentChar = pattern[position];
	RegexNode baseNode;
	
	if (currentChar == '(')
	{
		position++; // Skip '('
		baseNode = ParseExpression(pattern, ref position);
		
		if (position < pattern.Length && pattern[position] == ')')
			position++; // Skip ')'
	}
	else
	{
		// Handle literals
		baseNode = new RegexNode(RegexNodeType.Literal);
		baseNode.Value = currentChar.ToString();
		position++;
	}
	// ...
}
```

&ensp;&ensp;&ensp;   In continuity, the subsequent snippet further extends the `ParseTerm` method by addressing repetition operators. After processing the base element of a term, the method checks if the next character represents a repetition operator such as `+`, `*`, or `?`. Each operator is handled with precision: for instance, a `+` operator creates a repetition node that signifies one or more occurrences, while a `*` indicates zero or more repetitions. This conditional logic is essential for dynamically interpreting the repetition semantics encoded in the regex.

&ensp;&ensp;&ensp;   Additionally, the snippet also accounts for more complex repetition operators like `{}` which specify an exact or ranged number of repetitions. By dynamically creating a repetition node using the helper method `CreateRepetitionNode`, the code maintains a high level of abstraction and reusability. This design choice allows the parser to seamlessly support various repetition scenarios, ensuring that even more intricate regex patterns can be interpreted correctly. The clarity and modularity of this implementation contribute significantly to the robustness of the regex processing engine.

```csharp
private RegexNode ParseTerm(string pattern, ref int position)
{
	// ...

	// Check for repetition operators
	if (position < pattern.Length)
	{
		char nextChar = pattern[position];
		
		if (nextChar == '+')
			position++;
			return CreateRepetitionNode(baseNode, 1, -1); // 1 or more
		else if (nextChar == '*')
		{
			position++;
			return CreateRepetitionNode(baseNode, 0, -1); // 0 or more
		}
		else if (nextChar == '?')
		{
			position++;
			return CreateRepetitionNode(baseNode, 0, 1);  // 0 or 1
		}
		else if (nextChar == '{')
		{
			// ...
			return CreateRepetitionNode(baseNode, minRepeat, maxRepeat);
		}
	}
	return baseNode;
}
```

&ensp;&ensp;&ensp;  Moreover, the `CreateRepetitionNode` method is a focused piece of code that encapsulates the logic for handling repetition in a unified manner. This helper function takes a base node and the specified minimum and maximum repeat values to produce a new repetition node. By isolating this functionality into its own method, the code promotes reusability and simplifies the parsing process. It allows the main parsing function to remain clean and focused on higher-level logic without being cluttered by the details of repetition handling.

&ensp;&ensp;&ensp;  In addition, this method enhances the overall readability and maintainability of the codebase. The clear structure of creating a new node, setting its repetition parameters, and attaching the base node as a child makes the operation explicit and easy to follow. Such modularization is particularly beneficial in complex systems, where encapsulating specific functionalities into separate methods can significantly reduce cognitive load and improve debugging efficiency. This design philosophy is a testament to the careful planning behind the regex engine’s architecture.

```csharp
private RegexNode CreateRepetitionNode(RegexNode baseNode, int minRepeat, int maxRepeat)
{
	RegexNode repetitionNode = new RegexNode(RegexNodeType.Repetition);
	repetitionNode.MinRepeat = minRepeat;
	repetitionNode.MaxRepeat = maxRepeat;
	repetitionNode.Children.Add(baseNode);
	return repetitionNode;
}
```

##### Generating the words using `RegexGenerator.cs`
&ensp;&ensp;&ensp;  Next, the `GenerateValidCombinations` method in the `RegexGenerator.cs` file illustrates the transition from parsing to generation. This method leverages the abstract syntax tree produced by the parser to generate all valid string combinations that match the given regex pattern. By recursively traversing the node tree, it constructs possible strings by concatenating results from literals, alternations, and repetitions. This systematic approach ensures that every combination adheres to the defined pattern, showcasing the dynamic capability of the generator to handle complex regex structures.

&ensp;&ensp;&ensp;  Furthermore, this method serves as the bridge between theoretical regex parsing and practical application. It not only demonstrates the power of recursion in handling nested structures but also reinforces the importance of a clear AST representation for generating output. The integration of this generation step into the overall system highlights the seamless interaction between the parsing and processing components, ultimately ensuring that the application remains both flexible and reliable. This detailed process is crucial for validating the correctness of the regex engine and serves as a comprehensive example of applied formal language theory.

```csharp
public List<string> GenerateValidCombinations(string pattern)
{
	// Create a regex pattern parser
	RegexNode rootNode = _regexParser.ParseRegex(pattern);

	// Generate combinations based on the parsed tree
	return GenerateCombinationsFromNode(rootNode);
}
```

&ensp;&ensp;&ensp; Next, the `GenerateCombinationsFromNode` method is responsible for recursively building valid string combinations by traversing the abstract syntax tree of a regular expression. In the case of a **Literal** node, the method simply adds the literal value to the results. When encountering an **Alternation** node, it iterates over all its children and aggregates the combinations generated from each branch. This design allows for dynamic handling of expressions that involve choices between multiple alternatives, ensuring that all possible strings are captured without hardcoding specific scenarios.

&ensp;&ensp;&ensp; In continuity, the method also handles **Concatenation** by starting with an empty string and progressively appending the combinations of each child node. For each concatenated segment, it creates new results by combining existing partial strings with the new set of possibilities. This iterative process not only demonstrates a clear recursive structure but also emphasizes the importance of maintaining order when constructing valid strings. The resulting implementation is both robust and modular, which is key for processing complex regular expressions.

```csharp
private List<string> GenerateCombinationsFromNode(RegexNode node)
{
	List<string> results = new List<string>();
	switch (node.Type)
	{
		case RegexNodeType.Literal:
			results.Add(node.Value);
			break;
		case RegexNodeType.Alternation:
			foreach (var child in node.Children)
				results.AddRange(GenerateCombinationsFromNode(child));
			break;
		case RegexNodeType.Concatenation:
			// Start with empty string
			results.Add("");
			// For each child, generate combinations and append to current results
			foreach (var child in node.Children)
			{
				var childCombos = GenerateCombinationsFromNode(child);
				var newResults = new List<string>();
				
				foreach (var existingResult in results)
				{
					foreach (var childCombo in childCombos)
						newResults.Add(existingResult + childCombo);
				}
				results = newResults;
			}
			break;
	// ...
}
```

&ensp;&ensp;&ensp; Moreover, the handling of the **Repetition** node within the same `GenerateCombinationsFromNode` method adds a significant layer of complexity to the combination generation process. Here, the method first derives the base combinations from the child node of the repetition element. It then accommodates different repetition scenarios by checking the minimum and maximum repeat bounds. For instance, when the repetition operator corresponds to the `?` (zero or one) case, the code adds both the empty string and the base combinations, ensuring that the optional element is properly represented.

&ensp;&ensp;&ensp; In addition, the snippet thoughtfully differentiates between the `+` (one or more) and `*` (zero or more) operators. For the `+` operator, it generates combinations starting from one occurrence up to a defined repetition limit, while for the `*` operator, it also includes the possibility of zero occurrences. The logic further extends to handle specific ranges (like `{n}` or `{n,m}`) by iterating from the minimum to a calculated maximum repeat value. This careful branching ensures that the generator can dynamically adjust to various repetition semantics defined in the regex pattern, thereby reinforcing the system's flexibility and correctness.

```csharp
private List<string> GenerateCombinationsFromNode(RegexNode node)
{
	// ...
		case RegexNodeType.Repetition:
			var baseResults = GenerateCombinationsFromNode(node.Children[0]);
			results.Add(""); // Empty case for * and ?
			if (node.MinRepeat == 0 && node.MaxRepeat == 1) // ? (0 or 1)
				results.AddRange(baseResults);
			else if (node.MinRepeat == 1 && node.MaxRepeat == -1) // + (1 or more)
			{
				// Generate combinations for 1 to repetitionLimit
				for (int count = 1; count <= _repetitionLimit; count++)
				{
					var combinations = GenerateRepetitions(baseResults, count);
					results.AddRange(combinations);
				}
				results.RemoveAt(0); // Remove empty string for + operator
			}
			else if (node.MinRepeat == 0 && node.MaxRepeat == -1) // * (0 or more)
			{
				// Generate combinations for 0 to repetitionLimit
				for (int count = 1; count <= _repetitionLimit; count++)
				{
					var combinations = GenerateRepetitions(baseResults, count);
					results.AddRange(combinations);
				}
			}
			else // {n} or {n,m}
			{
				int max = node.MaxRepeat == -1 ? _repetitionLimit : Math.Min(node.MaxRepeat, _repetitionLimit);
				for (int count = node.MinRepeat; count <= max; count++)
				{
					var combinations = GenerateRepetitions(baseResults, count);
					results.AddRange(combinations);
				}
				if (node.MinRepeat > 0)
					results.RemoveAt(0);
			}
			break;
	}
	return results;
}
```

Last but not least, the `GenerateRepetitions` method focuses on the recursive construction of string repetitions based on a given list of base strings and a specified count. The method starts by addressing the simplest cases: if the count is zero, it returns a list containing an empty string, and if the count is one, it simply returns the original list of base strings. This clear base case handling is crucial to prevent infinite recursion and to set a proper foundation for the recursive process.

Moreover, when the count exceeds one, the method calls itself recursively to build up combinations of the required length. It does so by combining the results of a smaller repetition (i.e., count minus one) with each of the base strings. This recursive concatenation effectively constructs longer strings step by step, maintaining the order and correctness of the overall combination. The elegance of this approach lies in its simplicity and reusability, making it an essential component of the regex generator that seamlessly handles repetitive patterns in a dynamic and scalable manner.

```csharp
 private List<string> GenerateRepetitions(List<string> baseStrings, int count)
{
	if (count == 0)
		return new List<string> { "" };
	if (count == 1)
		return [.. baseStrings];

	var result = new List<string>();
	var subResults = GenerateRepetitions(baseStrings, count - 1);
	foreach (var subResult in subResults)
	{
		foreach (var baseStr in baseStrings)
			result.Add(subResult + baseStr);
	}
	return result;
}
```

## Conclusions / Screenshots / Results
****
&ensp;&ensp;&ensp; In this section, besides the conclusion that gets described at the end, an example of output, resulted from the execution of the program, is also explained in-depth. 

```
REGEX GENERATOR:
-------------------------
Pattern 1: (a|b)(c|d)E+G?
Generated valid combinations:
 - acE
 - acEG
 - acEE
 - acEEG
 - acEEE
 - acEEEG
 - acEEEE
 - acEEEEG
 - acEEEEE
 - acEEEEEG
 - adE
 - adEG
 - adEE
 - adEEG
 - adEEE
 - adEEEG
 - adEEEE
 - adEEEEG
 - adEEEEE
 - adEEEEEG
 - bcE
 - bcEG
 - bcEE
 - bcEEG
 - bcEEE
 - bcEEEG
 - bcEEEE
 - bcEEEEG
 - bcEEEEE
 - bcEEEEEG
 - bdE
 - bdEG
 - bdEE
 - bdEEG
 - bdEEE
 - bdEEEG
 - bdEEEE
 - bdEEEEG
 - bdEEEEE
 - bdEEEEEG
All combinations valid: True
Total amount of generated symbols: 40

Processing sequence for pattern 1:
Concatenation:
  Alternation:
    Concatenation:
      Literal: 'a'
    Concatenation:
      Literal: 'b'
  Alternation:
    Concatenation:
      Literal: 'c'
    Concatenation:
      Literal: 'd'
  Repetition (1 to ∞):
    Literal: 'E'
  Repetition (0 to 1):
    Literal: 'G'
```

Computational proof that, the amount of strings that were generated is correct:
##### Exercise 1: `(a|b)(c|d)E+G?`
Breaking this down:
- `(a|b)` - 2 possibilities: a or b
- `(c|d)` - 2 possibilities: c or d
- `E+` - With a 5-time repetition limit: E, EE, EEE, EEEE, or EEEEE (5 possibilities)
- `G?` - 2 possibilities: present or absent
Total: 2 × 2 × 5 × 2 = 40 possible valid strings for Variant 1
##### Exercise 2: `P(Q|R|S)T(UV|W|X)*Z+`
Breaking this down:
- `P` - 1 possibility
- `(Q|R|S)` - 3 possibilities: Q, R, or S
- `T` - 1 possibility
- `(UV|W|X)*` - With a 5-time repetition limit:
  - 0 repetitions: 1 possibility (empty string)
  - 1 repetition: 3 possibilities (UV, W, or X)
  - 2 repetitions: 3² = 9 possibilities
  - 3 repetitions: 3³ = 27 possibilities
  - 4 repetitions: 3⁴ = 81 possibilities
  - 5 repetitions: 3⁵ = 243 possibilities
  - Total for this part: 1 + 3 + 9 + 27 + 81 + 243 = 364 possibilities
- `Z+` - With a 5-time repetition limit: Z, ZZ, ZZZ, ZZZZ, or ZZZZZ (5 possibilities)
Total: 1 × 3 × 1 × 364 × 5 = 5,460 possible valid strings for Variant 2
##### Exercise 3: `1(0|1)*2(3|4){5}36`
Breaking this down:
- `1` - 1 possibility
- `(0|1)*` - With a 5-time repetition limit:
  - 0 repetitions: 1 possibility (empty string)
  - 1 repetition: 2 possibilities (0 or 1)
  - 2 repetitions: 2² = 4 possibilities
  - 3 repetitions: 2³ = 8 possibilities
  - 4 repetitions: 2⁴ = 16 possibilities
  - 5 repetitions: 2⁵ = 32 possibilities
  - Total for this part: 1 + 2 + 4 + 8 + 16 + 32 = 63 possibilities
- `2` - 1 possibility
- `(3|4){5}` - Exactly 5 repetitions of either 3 or 4: 2⁵ = 32 possibilities
- `36` - 1 possibility
Total: 1 × 63 × 1 × 32 × 1 = 2,016 possible valid strings for Variant 3

**Therefore:**
- Exercise 1: 40 possible valid strings
- Exercise 2: 5,460 possible valid strings
- Exercise 3: 2,016 possible valid strings
Which is exactly, the answers my program has printed.

&ensp;&ensp;&ensp; In conclusion, through the development, as a solution to the task, of a **parser, lexer, and interpreter**, this whole result that I have gained demonstrates a **dynamic and adaptable** approach to working with regular expressions. Unlike rigid, hardcoded solutions, my implementation allows for flexibility by **interpreting and generating valid strings dynamically** based on given regular expressions. This means that the solution is **not limited to specific cases** but can be extended to handle a wide variety of regular expressions without modifications to the core logic.

&ensp;&ensp;&ensp;  By **breaking down regular expressions into an abstract syntax tree (AST)**, the parser correctly identifies fundamental components such as **literals, alternations, concatenations, and repetitions**, allowing the generator to construct valid strings with precision. The **interpreter further ensures correctness**, validating that generated outputs conform to the expected regex structure.

&ensp;&ensp;&ensp;  Additionally, the **modular nature** of the design allows for enhancements, such as adding support for more complex regex operations, optimizing performance, or even integrating visualization tools for debugging regex structures. The **results produced align mathematically** with theoretical expectations, confirming the correctness and efficiency of the approach.

&ensp;&ensp;&ensp;  All in all, the whole solution I have developed, showcases the power of **formal language processing** and **finite automata theory** in practical applications. The **parser, lexer, and interpreter work in unison** to provide a **robust, scalable, and extendable** framework for processing regular expressions. This not only proves the feasibility of automating regex-based tasks but also lays a foundation for further advancements in the field of language processing and compiler design.

## References
****

<a id="ref3"></a>[1] Presentation on "Formal Languages and Compiler Design" - conf. univ., dr. Irina Cojuhari - 
https://else.fcim.utm.md/pluginfile.php/110457/mod_resource/content/0/Theme_1.pdf

<a id="ref4"></a>[2] Presentation on "Regular Language. Finite Automata" - TUM - 
https://drive.google.com/file/d/1rBGyzDN5eWMXTNeUxLxmKsf7tyhHt9Jk/view

[3] LLVM - "Kaleidoscope: Kaleidoscope Introduction and the Lexer" - [https://llvm.org/docs/tutorial/MyFirstLanguageFrontend/LangImpl01.html](https://llvm.org/docs/tutorial/MyFirstLanguageFrontend/LangImpl01.html)

[4] Wikipedia - "Lexical Analysis" - [https://en.wikipedia.org/wiki/Lexical_analysis](https://en.wikipedia.org/wiki/Lexical_analysis)

[5] regex101 - https://regex101.com/