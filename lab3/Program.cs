using lab3.Lexer;

namespace lab3
{
    class Program
    {
        static void Main()
        {
            var testInputDir = Directory.EnumerateFiles("TestInputs");
            
            foreach(var testInputFile in testInputDir)
            {
                if(testInputFile.Contains(".pixil"))
                {
                    string input = File.ReadAllText(testInputFile);
                    Console.WriteLine("\n\n{0}",input);

                    // Tokenization Process
                    Tokenizer lexer = new Tokenizer(input);
                    List<Token> tokens = lexer.Tokenize();

                    Console.WriteLine("\nTokens, detailed view:");
                    foreach (var token in tokens)
                    {
                        Console.WriteLine($"{token.Type}: '{token.Value}' at line {token.Line}, column {token.Column}");
                    }
                }
            }
        }
    }
}