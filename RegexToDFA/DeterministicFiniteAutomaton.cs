using System;
using System.Collections.Generic;
using System.Linq;

namespace RegexToDFA
{
    public class DeterministicFiniteAutomaton
    {

        public HashSet<int> states { get; set; }
        public HashSet<char> symbols { get; set; }
        public List<Transition> transitions { get; set; }
        public int initialState { get; set; }
        public HashSet<int> finalStates { get; set; }

        public DeterministicFiniteAutomaton()
        {
            states = new HashSet<int>();
            symbols = new HashSet<char>();
            transitions = new List<Transition>();
            finalStates = new HashSet<int>();
        }


        public bool verifyAutomaton()
        {
            if (!states.Contains(initialState)) return false;
            if(!finalStates.IsSubsetOf(states)) return false;
            
            foreach (var transition in transitions)
            {
                if (!states.Contains(transition.fromState)) return false;
                if (!states.Contains(transition.toState)) return false;
                if (!symbols.Contains(transition.symbol)) return false;
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
                    var transition = transitions.FirstOrDefault(t => t.fromState == state && t.symbol == sym);
                    if (transition != null)
                    {
                        Console.Write($" {transition.toState, -5} |");
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
            int currentState = initialState;

            foreach (char c in word)
            {
                var transition = transitions.FirstOrDefault(t => t.fromState == currentState && t.symbol == c);
                if (transition == null)
                {
                    return false; 
                }
                currentState = transition.toState;
            }

            return finalStates.Contains(currentState);
        }
    }
}