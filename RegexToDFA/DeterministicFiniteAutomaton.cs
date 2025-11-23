using System;
using System.Collections.Generic;

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

        
        public bool verifyAutomaton() {}
        
        public void printAutomaton()
        {}
        
        public bool checkWord() {}
    }
}