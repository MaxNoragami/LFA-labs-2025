namespace lab4
{
    class RegexGenerator
    {
        private readonly int _repetitionLimit;
        private readonly RegexParser _regexParser;
        
        public RegexGenerator(RegexParser regexParser,int repetitionLimit = 5)
        {
            _repetitionLimit = repetitionLimit;
            _regexParser = regexParser;
        }

        public List<string> GenerateValidCombinations(string pattern)
        {
            // Create a regex pattern parser
            RegexNode rootNode = _regexParser.ParseRegex(pattern);
            
            // Generate combinations based on the parsed tree
            return GenerateCombinationsFromNode(rootNode); // Limit repetitions to 5
        }
        
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
                    {
                        results.AddRange(GenerateCombinationsFromNode(child));
                    }
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
                            {
                                newResults.Add(existingResult + childCombo);
                            }
                        }
                        
                        results = newResults;
                    }
                    break;
                    
                case RegexNodeType.Repetition:
                    var baseResults = GenerateCombinationsFromNode(node.Children[0]);
                    results.Add(""); // Empty case for * and ?
                    
                    if (node.MinRepeat == 0 && node.MaxRepeat == 1) // ? (0 or 1)
                    {
                        results.AddRange(baseResults);
                    }
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
                        {
                            results.RemoveAt(0); // Remove empty string for {n,m} where n > 0
                        }
                    }
                    break;
            }
            
            return results;
        }
        
        private List<string> GenerateRepetitions(List<string> baseStrings, int count)
        {
            if (count == 0)
                return new List<string> { "" };
                
            if (count == 1)
                return new List<string>(baseStrings);
                
            var result = new List<string>();
            var subResults = GenerateRepetitions(baseStrings, count - 1);
            
            foreach (var subResult in subResults)
            {
                foreach (var baseStr in baseStrings)
                {
                    result.Add(subResult + baseStr);
                }
            }
            
            return result;
        }
    }
}