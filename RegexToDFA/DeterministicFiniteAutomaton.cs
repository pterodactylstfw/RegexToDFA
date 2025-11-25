using System;
using System.Collections.Generic;
using System.Linq;

namespace RegexToDFA
{
    public class DeterministicFiniteAutomaton
    {

        private HashSet<int> states { get; set; }
        private HashSet<char> symbols { get; set; }
        private Dictionary<int, Dictionary<char, int>> transitions { get; set; }
        private int initialState { get; set; }
        private HashSet<int> finalStates { get; set; }

        public DeterministicFiniteAutomaton(int q0)
        {
            states = new HashSet<int>();
            symbols = new HashSet<char>();
            transitions = new Dictionary<int, Dictionary<char, int>>();
            initialState = q0;
            finalStates = new HashSet<int>();
            
            states.Add(initialState);
        }


        public bool verifyAutomaton()
        {
            if (!states.Contains(initialState)) return false;
            if(!finalStates.IsSubsetOf(states)) return false;
            //if all transitions initial states are in states
            foreach (var transition in transitions)
            {
                if (!states.Contains(transition.Key)) return false;
                foreach (var pair in transition.Value)
                {
                    if (!symbols.Contains(pair.Key)) return false;
                    if (!states.Contains(pair.Value)) return false;
                }
            }
            return true;
            
        }

        public void printAutomaton()
        {
            // ordonam simbolurile si starile pentru afisare
            var sortedSymbols = symbols.OrderBy(s => s).ToList();
            var sortedStates = states.OrderBy(s => s).ToList();
            
            Console.Write($"{"State", -8} |"); 
            foreach (var sym in sortedSymbols)
            {
                Console.Write($" {sym, -5} |"); 
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 10 + sortedSymbols.Count * 8));
            
            foreach (var state in sortedStates)
            {
                string prefix = "";
                if (state == initialState) prefix += "->"; // start
                if (finalStates.Contains(state)) prefix += "*"; // final
        
                string stateLabel = prefix + state.ToString();
                Console.Write($"{stateLabel, -8} |");
                
                foreach (var sym in sortedSymbols)
                {
                    // verificam daca exista tranzitie pentru simbolul curent
                    if (transitions.ContainsKey(state) && transitions[state].ContainsKey(sym))
                    {
                        int nextState = transitions[state][sym];
                        Console.Write($" {nextState, -5} |");
                    }
                    else
                    {
                        Console.Write($" {"-", -5} |"); // - daca nu exista tranzitie
                    }
                }
                Console.WriteLine(); 
            }
        }

        public bool checkWord(String word)
        {
            while (word.Length > 0)
            {
                char currentChar = word[0];
                word = word.Substring(1);

                // verificam daca simbolul curent este valid
                if (!symbols.Contains(currentChar))
                {
                    return false; 
                }

                if (!transitions.ContainsKey(initialState) || !transitions[initialState].ContainsKey(currentChar)) 
                {
                    return false; // tranzitie inexistenta
                }

                initialState = transitions[initialState][currentChar];
            }
            return finalStates.Contains(initialState);
        }
    }
}