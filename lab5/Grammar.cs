using System.Security.Cryptography;
using System.Text;

namespace lab5
{
    public class Grammar(HashSet<string> vN, HashSet<char> vT, Dictionary<string, List<string>> p, string s)
    {
        public HashSet<string> VN {get; private set;} = vN; // Using HashSet because unordered collection of unique elements
        public HashSet<char> VT {get; private set;} = vT;
        public Dictionary<string, List<string>> P {get; set;} = p;
        public string S {get; set;} = s;

        public string GenerateString()
        {
            StringBuilder current = new StringBuilder(S); // current = "aa"
            Console.Write("\nWord Build: {0}", current);
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