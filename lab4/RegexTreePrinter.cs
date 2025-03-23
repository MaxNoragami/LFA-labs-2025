using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab4
{
    static class RegexTreePrinter
    {
        static public void Print(RegexNode node, int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            
            switch (node.Type)
            {
                case RegexNodeType.Literal:
                    Console.WriteLine($"{indent}Literal: '{node.Value}'");
                    break;
                    
                case RegexNodeType.Alternation:
                    Console.WriteLine($"{indent}Alternation:");
                    foreach (var child in node.Children)
                    {
                        Print(child, depth + 1);
                    }
                    break;
                    
                case RegexNodeType.Concatenation:
                    Console.WriteLine($"{indent}Concatenation:");
                    foreach (var child in node.Children)
                    {
                        Print(child, depth + 1);
                    }
                    break;
                    
                case RegexNodeType.Repetition:
                    string repInfo = node.MinRepeat + " to ";
                    repInfo += (node.MaxRepeat == -1) ? "∞" : node.MaxRepeat.ToString();
                    Console.WriteLine($"{indent}Repetition ({repInfo}):");
                    foreach (var child in node.Children)
                    {
                        Print(child, depth + 1);
                    }
                    break;
            }
        }
    }
}