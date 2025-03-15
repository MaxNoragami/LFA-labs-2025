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

            if (char.IsLetter(current) || current == '$' || current == '#')
            {
                string word = ReadWord();
                return CreateWordToken(word);
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

        // Reads the entire word
        private string ReadWord()
        {
            // To keep track where the word started
            int start = _position;

            while(_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '$' || _input[_position] == '#'))
            {
                _position++;
                _column++;
            }
            return _input.Substring(start, _position - start);
        }

        // Tokenizes the read word
        private Token CreateWordToken(string word)
        {
            // Checks if reserved word
            switch(word.ToUpper())
            {
                case "INT": return new Token(TokenType.TYPE_INT, word, _line, _column - word.Length);
                case "PIXEL": return new Token(TokenType.TYPE_PXLS, word, _line, _column - word.Length);
                case "DOUBLE": return new Token(TokenType.TYPE_DBL, word, _line, _column - word.Length);
                case "STRING": return new Token(TokenType.TYPE_STR, word, _line, _column - word.Length);
                case "BOOL": return new Token(TokenType.TYPE_BOOL, word, _line, _column - word.Length);
                case "IMG": return new Token(TokenType.TYPE_IMG, word, _line, _column - word.Length);
                case "BATCH": return new Token(TokenType.TYPE_BATCH, word, _line, _column - word.Length);
                case "TRUE": return new Token(TokenType.BOOL_VALUE, word, _line, _column - word.Length);
                case "FALSE": return new Token(TokenType.BOOL_VALUE, word, _line, _column - word.Length);
                case "IN": return new Token(TokenType.IN, word, _line, _column - word.Length);
                case "FOREACH": return new Token(TokenType.FOREACH, word, _line, _column - word.Length);
                case "FSIZE": return new Token(TokenType.FSIZE, word, _line, _column - word.Length);
                case "FNAME": return new Token(TokenType.FNAME, word, _line, _column - word.Length);
                case "FHEIGHT": return new Token(TokenType.FHEIGHT, word, _line, _column - word.Length);
                case "FWIDTH": return new Token(TokenType.FWIDTH, word, _line, _column - word.Length);
                case "METADATA": return new Token(TokenType.METADATA, word, _line, _column - word.Length);
                case "RIGHT": return new Token(TokenType.RIGHT, word, _line, _column - word.Length);
                case "LEFT": return new Token(TokenType.LEFT, word, _line, _column - word.Length);
                case "SET": return new Token(TokenType.SET, word, _line, _column - word.Length);
                case "ELIF": return new Token(TokenType.ELIF, word, _line, _column - word.Length);
                case "ELSE": return new Token(TokenType.ELSE, word, _line, _column - word.Length);
                case "IF": return new Token(TokenType.IF, word, _line, _column - word.Length);
                case "SHARPEN": return new Token(TokenType.SHARPEN, word, _line, _column - word.Length);
                case "NEGATIVE": return new Token(TokenType.NEGATIVE, word, _line, _column - word.Length);
                case "BW": return new Token(TokenType.BW, word, _line, _column - word.Length);
                case "SEPIA": return new Token(TokenType.SEPIA, word, _line, _column - word.Length);
                case "CROP": return new Token(TokenType.CROP, word, _line, _column - word.Length);
                case "ROTATE": return new Token(TokenType.ROTATE, word, _line, _column - word.Length);
            }

            // Checks if identifiers
            if (word.First() == '$') return new Token(TokenType.VAR_IDENTIFIER, word, _line, _column - word.Length);
            if (word.First() == '#') return new Token(TokenType.VAR_IDENTIFIER, word, _line, _column - word.Length);   

            throw new Exception(string.Format("Unexpected word '{0}', at line {1}, column {2}.", word, _line, _column - word.Length));
        }
    }
}