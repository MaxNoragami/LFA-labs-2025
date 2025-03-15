using lab3.Lexer;

namespace lab3
{
    class Program
    {
        static void Main()
        {
            string input = """
            {
                BATCH #mybatch = "images/";
                FOREACH IMG $img IN #mybatch {
                    SET $img SEPIA;
                    ROTATE $img LEFT;
                    CROP $img (100, 200);
                    INT $width = METADATA $img WIDTH;
                }
            }
            """;

            Console.WriteLine(input);

            Tokenizer lexer = new Tokenizer(input);
            List<Token> tokens = lexer.Tokenize();
            foreach (var token in tokens)
            {
                Console.WriteLine($"{token.Type}: '{token.Value}' at line {token.Line}, column {token.Column}");
            }
        }
    }
}