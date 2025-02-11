using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class FiniteAutomaton
    {
        public HashSet<string> Q { get; set; } // Here goes V_n
        public HashSet<char> Sigma { get; set; } // Here goes V_t
        public Dictionary<(string, char), HashSet<string>> Delta { get; set; } // Here goes P , we have a Dictionary with a Tuple and a hashset        
        public string Q0 {get; set; } // Here goes S
        public string QF { get; set; } // Here goes q_f

        public FiniteAutomaton(HashSet<string> q, HashSet<char> sigma, Dictionary<(string, char), HashSet<string>> delta, string q0, string qF)
        {
            Q = q;
            Sigma = sigma;
            Delta = delta;
            Q0 = q0;
            QF = qF;
        }

        public bool StringBelongToLanguage(String inputString)
        {
            var currentStates = new HashSet<string>(){Q0};
            
            Console.Write("\nFA Check: ({0}) ", string.Join("", currentStates));

            foreach(var letter in inputString)
            {
                var nextStates = new HashSet<string>();
                var nextPath = new HashSet<char>();

                foreach(var state in currentStates)
                {
                    if(Delta.Keys.Contains((state, letter)))
                    {
                        HashSet<string> possibleStates = [.. Delta[(state, letter)]];
                        nextStates = nextStates.Concat(possibleStates).ToHashSet();
                        nextPath.Add(letter);
                    }
                }
            
                currentStates = nextStates;

                if(currentStates.Count == 0)
                {
                    Console.Write("-----> Invalid", string.Join("", nextPath));
                    return false;
                }

                Console.Write("--{0}--> ({1}) ", string.Join("", nextPath), string.Join("", currentStates));
            }
            return currentStates.Contains(QF);
        }

        public override string ToString()
        {
            string qData = "Q = {" + string.Join(", ", Q) + "}\n";
            string sigmaData = "Σ ={" + string.Join(", ", Sigma) + "}\n";
            StringBuilder deltaData = new StringBuilder();
            foreach(var pair in Delta)
            {
                deltaData.Append($"\tδ(" + pair.Key + ") = {" + string.Join(", ", pair.Value) + "}\n");
            }

            string q0Data = "q_0 = {" + Q0 + "}\n";
            string qFData = "q_F = {" + QF + "}\n";

            return string.Format("{0}{1}{2}{3}{4}", qData, sigmaData, deltaData.ToString(), q0Data, qFData);
        }
    }
}