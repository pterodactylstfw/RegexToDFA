using System.Collections.Generic;

namespace RegexToDFA
{
    public class LambdaAutomaton
    {
        public const char lambda = '$';

        public int initialState { get; set; }
        public int finalState { get; set; }
        public List<Transition> transitions { get; set; }
        
        public static int stateCount = 0;
        
        public LambdaAutomaton()
        {
            transitions = new List<Transition>();
        }
        
        public LambdaAutomaton(char symbol)
        {
            initialState = stateCount++;
            finalState = stateCount++;
            transitions = new List<Transition>();
            
            transitions.Add(new Transition(initialState, finalState, symbol));
        }

        public LambdaAutomaton(int initialState, int finalState, List<Transition> transitions)
        {
            this.initialState = initialState;
            this.finalState = finalState;
            this.transitions = transitions;
        }
        
        public static void resetStateCount()
        {
            stateCount = 0;
        }

        // operatorul | - alternarea(SAU)
        public static LambdaAutomaton operator |(LambdaAutomaton left, LambdaAutomaton right)
        {
            int newStart = stateCount++;
            int newEnd = stateCount++;
            List<Transition> newTransitions = new List<Transition>();

            newTransitions.AddRange(left.transitions);
            newTransitions.AddRange(right.transitions);
            
            newTransitions.Add(new Transition(newStart, left.initialState, lambda));
            newTransitions.Add(new Transition(newStart, right.initialState, lambda));
            newTransitions.Add(new Transition(left.finalState, newEnd, lambda));
            newTransitions.Add(new Transition(right.finalState, newEnd, lambda));
            
            return new LambdaAutomaton(newStart, newEnd, newTransitions);
        }
        
        // operatorul . - concatenarea
        public static LambdaAutomaton concatenation(LambdaAutomaton left, LambdaAutomaton right)
        {
            List<Transition> newTransitions = new List<Transition>();
            newTransitions.AddRange(left.transitions);
            foreach (var trans in right.transitions)
            {
                int newFrom = trans.fromState;
                int newTo = trans.toState;
                
                if (newFrom == right.initialState)
                {
                    newFrom = left.finalState;
                }
                
                if (newTo == right.initialState)
                {
                    newTo = left.finalState;
                }
                
                newTransitions.Add(new Transition(newFrom, newTo, trans.symbol));
            }
            return new LambdaAutomaton(left.initialState, right.finalState, newTransitions);
        }
        
        // operatorul *
        public void kleeneStar()
        {
            int newStart = stateCount++;
            int newEnd = stateCount++;
            
            transitions.Add(new Transition(newStart, initialState, lambda));
            transitions.Add(new Transition(newStart, newEnd, lambda));
            transitions.Add(new Transition(finalState, newEnd, lambda));
            transitions.Add(new Transition(finalState, initialState, lambda));

            initialState = newStart;
            finalState = newEnd;
        }
        
        // operatorul +
        public void plus()
        {
            int newStart = stateCount++;
            int newEnd = stateCount++;
            
            transitions.Add(new Transition(newStart, initialState, lambda));
            transitions.Add(new Transition(finalState, newEnd, lambda));
            transitions.Add(new Transition(finalState, initialState, lambda));

            initialState = newStart;
            finalState = newEnd;
        }
    }
}