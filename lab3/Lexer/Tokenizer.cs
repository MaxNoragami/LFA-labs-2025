using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Lexer
{
    public class Tokenizer
    {
        private readonly string _input;
        private int _position;
        private int _line;
        private int _column;

        public Tokenizer(string input)
        {
            _input = input;
            _position = 0;
            _line = 1;
            _column = 1;
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();

            while(_position < _input.Length)
            {
                char current = _input[_position];

                // Skip whitespaces
                if(char.IsWhiteSpace(current))
                {
                    _position++;
                    _column++;
                    continue;
                }

                Token token = MatchToken();
                if(token != null)
                {
                    tokens.Add(token);
                }
                else
                {
                    Console.WriteLine(string.Format("Unexpected char '{0}', at line {1}, column {2}.", current, _line, _column));
                    break;
                }
            }
            
            tokens.Add(new Token(TokenType.EOF, "", _line, _column));
            return tokens;
        }

        private Token MatchToken()
        {
            char current = _input[_position];

            // Tokenizing symbols
            if(current == ')') return Advance(TokenType.CLOSE_P, ")");
            if(current == '(') return Advance(TokenType.OPEN_P, "(");
            if(current == ',') return Advance(TokenType.COMMA, ",");
            if(current == '}') return Advance(TokenType.CLOSE_BLOCK, "}");
            if(current == '{') return Advance(TokenType.OPEN_BLOCK, "{");
            if(current == ';') return Advance(TokenType.EOF, ";");

            if (current == '=')
            {
                if (Peek() == '=') return AdvanceTwo(TokenType.EQUAL, "==");
                return Advance(TokenType.ASSIGN, "=");
            }

            if (current == '>')
            {
                if (Peek() == '=') return AdvanceTwo(TokenType.GREATER_EQUAL, ">=");
                return Advance(TokenType.GREATER, ">");
            }

            if (current == '<')
            {
                if (Peek() == '=') return AdvanceTwo(TokenType.SMALLER_EQUAL, "<=");
                return Advance(TokenType.SMALLER, "<");
            }

            if (current == '!')
            {
                if (Peek() == '=') return AdvanceTwo(TokenType.NOT_EQUAL, "!=");
                return null;
            }

            return null;
        }

        private Token Advance(TokenType type, string val)
        {
            _position++;
            _column++;
            return new Token(type, val , _line, _column - 1);
        }

        private Token AdvanceTwo(TokenType type, string val)
        {
            _position += 2;
            _column += 2;
            return new Token(type, val , _line, _column - 2);
        }
        
        // For having a look at the next char
        private char Peek()
        {
            return _position + 1 < _input.Length ? _input[_position + 1] : '\0';
        }
    }
}