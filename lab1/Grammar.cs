using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }
    }
}