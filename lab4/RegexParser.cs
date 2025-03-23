using System.Text;

namespace lab4
{
    class RegexParser
    {
        public RegexNode ParseRegex(string pattern)
        {
            int position = 0;
            return ParseExpression(pattern, ref position);
        }

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
            
            // Check for repetition operators
            if (position < pattern.Length)
            {
                char nextChar = pattern[position];
                
                if (nextChar == '+')
                {
                    position++;
                    return CreateRepetitionNode(baseNode, 1, -1); // 1 or more
                }
                else if (nextChar == '*')
                {
                    position++;
                    return CreateRepetitionNode(baseNode, 0, -1); // 0 or more
                }
                else if (nextChar == '?')
                {
                    position++;
                    return CreateRepetitionNode(baseNode, 0, 1);  // 0 or 1
                }
                else if (nextChar == '{')
                {
                    position++; // Skip '{'
                    
                    // Parse the number(s) inside the curly braces
                    int minRepeat = 0, maxRepeat = 0;
                    StringBuilder numBuilder = new StringBuilder();
                    
                    while (position < pattern.Length && char.IsDigit(pattern[position]))
                    {
                        numBuilder.Append(pattern[position]);
                        position++;
                    }
                    
                    minRepeat = int.Parse(numBuilder.ToString());
                    maxRepeat = minRepeat; // Default for {n} format
                    
                    if (position < pattern.Length && pattern[position] == ',')
                    {
                        position++; // Skip ','
                        numBuilder.Clear();
                        
                        while (position < pattern.Length && char.IsDigit(pattern[position]))
                        {
                            numBuilder.Append(pattern[position]);
                            position++;
                        }
                        
                        maxRepeat = numBuilder.Length > 0 ? int.Parse(numBuilder.ToString()) : -1;
                    }
                    
                    if (position < pattern.Length && pattern[position] == '}')
                        position++; // Skip '}'
                        
                    return CreateRepetitionNode(baseNode, minRepeat, maxRepeat);
                }
            }
            
            return baseNode;
        }
        
        private RegexNode CreateRepetitionNode(RegexNode baseNode, int minRepeat, int maxRepeat)
        {
            RegexNode repetitionNode = new RegexNode(RegexNodeType.Repetition);
            repetitionNode.MinRepeat = minRepeat;
            repetitionNode.MaxRepeat = maxRepeat;
            repetitionNode.Children.Add(baseNode);
            return repetitionNode;
        }
    }
}