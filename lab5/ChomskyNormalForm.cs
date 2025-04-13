using System.Text;

namespace lab5
{
    public static class ChomskyNormalForm
    {
        private static string GetNextAvailableLetter(Grammar grammar)
        {
            // Start from G as specified in the problem statement
            char letter = 'G';
            while (grammar.VN.Contains(letter.ToString()))
            {
                letter++;
            }
            return letter.ToString();
        }

        private static void EliminateEmptyProductions(Grammar grammar)
        {
            
        }

        private static void EliminateAnyUnitRules(Grammar grammar)
        {
            
        }

        private static void EliminateInaccessibleSymbols(Grammar grammar)
        {
            
        }

        private static void EliminateNonProdcutiveSymbols(Grammar grammar)
        {
            
        }

        public static void Obtain(Grammar grammar)
        {
            
        }
    }
}