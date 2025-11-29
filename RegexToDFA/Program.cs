using System;
using System.IO;

namespace RegexToDFA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string inputFilePath = "input.txt";
            string outputFilePath = "output.txt";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Fisierul de intrare nu a fost gasit: {inputFilePath}");
                return;
            }

            string regex = File.ReadAllText(inputFilePath).Trim();
            Console.WriteLine($"Expresie regulata citita: {regex}");
            
            string concatRegex = RegexUtils.ConcatenationHandle(regex);
            DeterministicFiniteAutomaton dfa = RegexToDFA.RegexToDfa(concatRegex);

            while (true)
            {
                Console.WriteLine("\nMeniu:");
                Console.WriteLine("1. Afiseaza forma poloneza postfixata a expresiei");
                Console.WriteLine("2. Afiseaza arborele sintactic");
                Console.WriteLine("3. Afiseaza automatul finit determinist (consola si fisier)");
                Console.WriteLine("4. Verifica cuvinte in automat");
                Console.WriteLine("5. Iesire");
                Console.Write("Alege o optiune: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Forma postfixata: {RegexUtils.ToPostfixForm(concatRegex)}");
                        break;
                    case "2":
                        Console.WriteLine("Arbore Sintactic:");
                        RegexUtils.TreeNode treeRoot = RegexUtils.SyntaxTree(RegexUtils.ToPostfixForm(concatRegex));
                        RegexUtils.PrintSyntaxTree(treeRoot);
                        break;
                    case "3":
                        Console.WriteLine("Automatul in consola:");
                        dfa.PrintAutomaton();

                        using (StreamWriter writer = new StreamWriter(outputFilePath))
                        {
                            var originalConsoleOut = Console.Out;
                            Console.SetOut(writer);

                            dfa.PrintAutomaton();

                            Console.SetOut(originalConsoleOut);
                        }
                        Console.WriteLine($"Automatul a fost scris si in fisierul '{outputFilePath}'");
                        break;
                    case "4":
                        Console.WriteLine("Introduceti un cuvant pentru verificare (sau 'EXIT' pentru a reveni la meniu):");
                        string word;
                        while ((word = Console.ReadLine()) != "EXIT")
                        {
                            bool isAccepted = dfa.CheckWord(word); 
                            Console.WriteLine($"Cuvantul '{word}' este {(isAccepted ? "acceptat" : "neacceptat")}.");
                            Console.WriteLine("Introduceti un alt cuvant sau 'EXIT':");
                        }
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Optiune invalida. Va rugam sa incercati din nou.");
                        break;
                }
            }
        }
    }
}
