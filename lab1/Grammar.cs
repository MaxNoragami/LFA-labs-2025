using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Grammar
    {
        public HashSet<string> VN {get; private set;} // Using HashSet because unordered collection of unique elements
        public HashSet<char> VT {get; private set;}
        public Dictionary<string, List<string>> P {get; private set;}
        public string S {get; private set;}

        public Grammar(HashSet<string> vN, HashSet<char> vT, Dictionary<string, List<string>> p, string s)
        {
            VN = vN;
            VT = vT;
            P = p;
            S = s;
        }

        public string GenerateString()
        {
            StringBuilder current = new StringBuilder(S); // current = "aa"
            Console.Write("\n{0}", current);
            while(true)
            {
                bool hasNonTerminal = false;

                for(int i = 0; i < current.Length; i++)
                {
                    string symbol = current[i].ToString();
                    if(VN.Contains(symbol))
                    {
                        hasNonTerminal = true;
                        var productionRule = P[symbol];
                        string usedReplacement = productionRule[RandomNumberGenerator.GetInt32(productionRule.Count)];
                        current.Remove(i, symbol.Length);
                        current.Insert(i, usedReplacement);

                        Console.Write(" ---> {0}", current);

                        break;
                    }
                }

                if(!hasNonTerminal) break;
            }

            return current.ToString();
        }

        public FiniteAutomaton ToFiniteAutomaton()
        {
            string qF = "q_F";
            HashSet<string> q = [.. VN, qF];
            HashSet<char> sigma = [.. VT]; 
            string q0 = S;
            Dictionary<(string, char), HashSet<string>> delta = new Dictionary<(string, char), HashSet<string>>();
            
            foreach(var rule in P)
            {
                foreach(var product in rule.Value)
                {
                    string nonTerminal = string.Empty;
                    char terminal = '\0';
                    
                    char[] productSymbols = product.ToArray();
                    foreach(var symbol in productSymbols)
                    {
                        string symbolS = Convert.ToString(symbol);
                        if(VN.Contains(symbolS))
                        {
                            nonTerminal = symbolS;
                            continue;
                        }
                        terminal = symbol;
                    }

                    (string, char) key = (rule.Key, terminal);

                    if(!delta.ContainsKey(key)) delta[key] = new HashSet<string>();

                    delta[key].Add((nonTerminal == string.Empty)? qF:nonTerminal);
                }
            }

            return new FiniteAutomaton(q, sigma, delta, q0, qF);
        }

        public override string ToString()
        {
            // Outputing the definition to console
            string vNData = "V_n = {" + string.Join(", ", VN) + "}\n";
            string vTData = "V_t = {" + string.Join(", ", VT) + "}\n";
            StringBuilder pData = new StringBuilder("P = {");

            foreach(var pair in P)
            {
                pData.Append("\t" + pair.Key + " ---> " + string.Join(" | ", pair.Value) + "\n");
            }
            pData.Append("}\n");

            string sData = "S = {" + S + "}\n";

            return string.Format("{0}{1}{2}{3}", vNData, vTData, pData, sData);
        }
    }
}