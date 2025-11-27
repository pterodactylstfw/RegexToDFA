using System;

namespace RegexToDFA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string regex = "ab|c";
            
            string output = RegexUtils.concatenationHandle(regex);
            Console.WriteLine(output);
            
            string postfix = RegexUtils.toPostfixForm(output);
            Console.WriteLine(postfix);

            RegexUtils.TreeNode syntacticTree = RegexUtils.syntaxTree(postfix); 
            RegexUtils.printSyntaxTree(syntacticTree);
            
            LambdaAutomaton nfa = RegexToLambdaAutomaton.build(postfix);

            Console.WriteLine("\n--- NFA Construit ---");
            Console.WriteLine($"Stare Start: {nfa.initialState}");
            Console.WriteLine($"Stare Final: {nfa.finalState}");
            Console.WriteLine("Tranzitii:");

            foreach(var t in nfa.transitions)
            {
                Console.WriteLine($"  {t.fromState} --({t.symbol})--> {t.toState}");
            }
        }
    }
}