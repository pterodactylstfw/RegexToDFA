using System;
using System.Collections.Generic;
using System.Linq;

namespace RegexToDFA
{
    public class DeterministicFiniteAutomaton
    {

        public HashSet<int> States { get; set; } = new HashSet<int>();
        public HashSet<char> Symbols { get; set; } = new HashSet<char>();
        public List<Transition> Transitions { get; set; } = new List<Transition>();
        public int InitialState { get; set; }
        public HashSet<int> FinalStates { get; set; } = new HashSet<int>();


        public bool VerifyAutomaton()
        {
            if (!States.Contains(InitialState)) return false;
            if(!FinalStates.IsSubsetOf(States)) return false;
            
            foreach (var transition in Transitions)
            {
                if (!States.Contains(transition.FromState)) return false;
                if (!States.Contains(transition.ToState)) return false;
                if (!Symbols.Contains(transition.Symbol)) return false;
            }
            return true;
            
        }

        public void PrintAutomaton()
        {
            // ordonam simbolurile si starile pentru afisare
            var sortedSymbols = Symbols.OrderBy(s => s).ToList();
            var sortedStates = States.OrderBy(s => s).ToList();
            
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
                if (state == InitialState) prefix += "->"; // start
                if (FinalStates.Contains(state)) prefix += "*"; // final
        
                string stateLabel = prefix + state.ToString();
                Console.Write($"{stateLabel, -8} |");
                
                foreach (var sym in sortedSymbols)
                {
                    // verificam daca exista tranzitie pentru simbolul curent
                    var transition = Transitions.FirstOrDefault(t => t.FromState == state && t.Symbol == sym);
                    if (transition != null)
                    {
                        Console.Write($" {transition.ToState, -5} |");
                    }
                    else
                    {
                        Console.Write($" {"-", -5} |"); // - daca nu exista tranzitie
                    }
                }
                Console.WriteLine(); 
            }
        }

        public bool CheckWord(String word)
        {
            int currentState = InitialState;

            foreach (char c in word)
            {
                var transition = Transitions.FirstOrDefault(t => t.FromState == currentState && t.Symbol == c);
                if (transition == null)
                {
                    return false; 
                }
                currentState = transition.ToState;
            }

            return FinalStates.Contains(currentState);
        }
        
    }
}