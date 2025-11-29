using System.Collections.Generic;

namespace RegexToDFA
{
    public class LambdaAutomaton
    {
        public const char Lambda = '$';

        public int InitialState { get; set; }
        public int FinalState { get; set; }
        public List<Transition> Transitions { get; set; }
        
        public static int StateCount = 0;
        
        public LambdaAutomaton()
        {
            Transitions = new List<Transition>();
        }
        
        public LambdaAutomaton(char symbol)
        {
            InitialState = StateCount++;
            FinalState = StateCount++;
            Transitions = new List<Transition>();
            
            Transitions.Add(new Transition(InitialState, FinalState, symbol));
        }

        public LambdaAutomaton(int initialState, int finalState, List<Transition> transitions)
        {
            this.InitialState = initialState;
            this.FinalState = finalState;
            this.Transitions = transitions;
        }
        
        public static void ResetStateCount()
        {
            StateCount = 0;
        }

        // operatorul | - alternarea(SAU)
        public static LambdaAutomaton operator |(LambdaAutomaton left, LambdaAutomaton right)
        {
            int newStart = StateCount++;
            int newEnd = StateCount++;
            List<Transition> newTransitions = new List<Transition>();

            newTransitions.AddRange(left.Transitions);
            newTransitions.AddRange(right.Transitions);
            
            newTransitions.Add(new Transition(newStart, left.InitialState, Lambda));
            newTransitions.Add(new Transition(newStart, right.InitialState, Lambda));
            newTransitions.Add(new Transition(left.FinalState, newEnd, Lambda));
            newTransitions.Add(new Transition(right.FinalState, newEnd, Lambda));
            
            return new LambdaAutomaton(newStart, newEnd, newTransitions);
        }
        
        // operatorul . - concatenarea
        public static LambdaAutomaton Concatenation(LambdaAutomaton left, LambdaAutomaton right)
        {
            List<Transition> newTransitions = new List<Transition>();
            newTransitions.AddRange(left.Transitions);
            foreach (var trans in right.Transitions)
            {
                int newFrom = trans.FromState;
                int newTo = trans.ToState;
                
                if (newFrom == right.InitialState)
                {
                    newFrom = left.FinalState;
                }
                
                if (newTo == right.InitialState)
                {
                    newTo = left.FinalState;
                }
                
                newTransitions.Add(new Transition(newFrom, newTo, trans.Symbol));
            }
            return new LambdaAutomaton(left.InitialState, right.FinalState, newTransitions);
        }
        
        // operatorul *
        public void KleeneStar()
        {
            int newStart = StateCount++;
            int newEnd = StateCount++;
            
            Transitions.Add(new Transition(newStart, InitialState, Lambda));
            Transitions.Add(new Transition(newStart, newEnd, Lambda));
            Transitions.Add(new Transition(FinalState, newEnd, Lambda));
            Transitions.Add(new Transition(FinalState, InitialState, Lambda));

            InitialState = newStart;
            FinalState = newEnd;
        }
        
        // operatorul +
        public void Plus()
        {
            int newStart = StateCount++;
            int newEnd = StateCount++;
            
            Transitions.Add(new Transition(newStart, InitialState, Lambda));
            Transitions.Add(new Transition(FinalState, newEnd, Lambda));
            Transitions.Add(new Transition(FinalState, InitialState, Lambda));

            InitialState = newStart;
            FinalState = newEnd;
        }
    }
}