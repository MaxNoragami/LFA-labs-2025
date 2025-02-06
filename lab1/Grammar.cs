using System;
using System.Collections.Generic;
using System.Linq;
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
            StringBuilder current = new StringBuilder(S);

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
                        break;
                    }
                }

                if(!hasNonTerminal) break;
            }

            return current.ToString();
        }
    }
}