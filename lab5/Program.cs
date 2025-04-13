using lab5;

// Variant 1, grammar init
var grammar = new Grammar(
    vN: new HashSet<string>() {"S", "A", "B", "C", "D", "E"},
    vT: new HashSet<char>() {'a', 'b'},
    p:  new Dictionary<string, List<string>>() {
        ["S"] = new List<string>() {"aB", "AC"},
        ["A"] = new List<string>() {"a", "ASC", "BC", "aD"},
        ["B"] = new List<string>() {"b", "bS"},
        ["C"] = new List<string>() {"BA", "ε"},
        ["E"] = new List<string>() {"aB"},
        ["D"] = new List<string>() {"abC"}
    },
    s: "S"
);

// Before CNF
Console.WriteLine("\nGrammar before modifications:");
Console.WriteLine(grammar.ToString());

// Performing CNF
ChomskyNormalForm.Obtain(grammar);

// After CNF
Console.WriteLine("\nGrammar after bringing it to CNF:");
Console.WriteLine(grammar.ToString());