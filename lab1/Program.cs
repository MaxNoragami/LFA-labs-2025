namespace lab1
{
    class Program
    {
        static void Main()
        {

            // Variant 1 - Alexei Maxim
            Console.WriteLine("Alexei Maxim,\nFAF-232,\nVariant 1\n");

            // My variant's grammar definition
            HashSet<string> vN = new HashSet<string>() {"S", "P", "Q"};
            HashSet<char> vT = new HashSet<char>() {'a', 'b', 'c', 'd', 'e', 'f'};
            Dictionary<string, List<string>> p = new Dictionary<string, List<string>>()
                {
                    {"S", new List<string>() {"aP", "bQ"}},
                    {"P", new List<string>() {"bP", "cP", "dQ", "e"}},
                    {"Q", new List<string>() {"eQ", "fQ", "a"}}
                };
            string s = "S";

            // Outputing the definition to console
            Console.WriteLine("V_n = {" + string.Join(", ", vN) + "}");
            Console.WriteLine("V_t = {" + string.Join(", ", vT) + "}");
            Console.WriteLine("P = {");
            foreach(var pair in p)
            {
                Console.WriteLine("\t" + pair.Key + " ---> " + string.Join(" | ", pair.Value));
            }
            Console.WriteLine("}\nS = {s}\n");
            
            
            



            Grammar grammar = new Grammar(vN, vT, p, s);
            for(int i = 0; i < 5; i++) Console.WriteLine(grammar.GenerateString());
        }
    }
}