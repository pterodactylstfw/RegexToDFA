using System.Collections.Generic;
using System.Linq;

namespace RegexToDFA
{
    public class LambdaAutomatonToDFA
    {
        public static DeterministicFiniteAutomaton dfa(LambdaAutomaton lambdaAutomaton)
        {
            DeterministicFiniteAutomaton dfa = new DeterministicFiniteAutomaton();

            foreach (var transition in lambdaAutomaton.transitions)
            {
                if (transition.symbol != LambdaAutomaton.lambda)
                    dfa.symbols.Add(transition.symbol);
            }
            
            Dictionary<string, int> dfaStateMap = new Dictionary<string, int>();
            Queue<HashSet<int>> queue = new Queue<HashSet<int>>();
            
            HashSet<int> startSet = getLambdaClosure(lambdaAutomaton.initialState, lambdaAutomaton.transitions);
            string startKey = GetSetKey(startSet);
            
            int countDfaState = 0;
            dfaStateMap[startKey] = countDfaState;
            dfa.states.Add(countDfaState);
            dfa.initialState = countDfaState;
            if (startSet.Contains(lambdaAutomaton.finalState))
            {
                dfa.finalStates.Add(countDfaState);
            }
            
            countDfaState++;
            queue.Enqueue(startSet);

            while (queue.Count > 0)
            {
                HashSet<int> currentSet = queue.Dequeue();
                int currentDfaId = dfaStateMap[GetSetKey(currentSet)];

                foreach (var symbol in dfa.symbols)
                {
                    HashSet<int> foundStates = new HashSet<int>();

                    foreach (var transition in lambdaAutomaton.transitions)
                    {
                        if (transition.symbol == symbol && currentSet.Contains(transition.fromState))
                        {
                            foundStates.Add(transition.toState);
                        }
                    }

                    if (foundStates.Count() > 0)
                    {
                        HashSet<int> newSet = new HashSet<int>();
                        foreach (int state in foundStates)
                        {
                            newSet.UnionWith(getLambdaClosure(state,  lambdaAutomaton.transitions));
                        }
                        
                        string newStateKey = GetSetKey(newSet);
                        int nextStateId; 
                        
                        if (dfaStateMap.ContainsKey(newStateKey))
                        {
                            nextStateId = dfaStateMap[newStateKey];
                        }
                        else
                        {
                            nextStateId = countDfaState;
                            dfaStateMap.Add(newStateKey, nextStateId);
                            queue.Enqueue(newSet);
                            
                            if (newSet.Contains(lambdaAutomaton.finalState))
                            {
                                dfa.finalStates.Add(nextStateId);
                            }
                            dfa.states.Add(nextStateId);
                            
                            countDfaState++;
                        }
                        
                        dfa.transitions.Add(new Transition(currentDfaId, nextStateId, symbol));
                    }
                }
            }

            return dfa;
        }

        private static HashSet<int> getLambdaClosure(int state, List<Transition> transitions)
        {
            HashSet<int> lambdaClosure = new HashSet<int>();
            Stack<int> stack = new Stack<int>();
            
            lambdaClosure.Add(state);
            stack.Push(state);
            while (stack.Count > 0)
            {
                int currentState = stack.Pop();
                foreach (var transition in transitions)
                {
                    if (transition.fromState == currentState && transition.symbol == LambdaAutomaton.lambda)
                    {
                        if (!lambdaClosure.Contains(transition.toState))
                        {
                            lambdaClosure.Add(transition.toState);
                            stack.Push(transition.toState);
                        }
                    }
                }
            }
            return lambdaClosure;
        }
        
        private static string GetSetKey(HashSet<int> set)
        {
            List<int> sorted = set.ToList();
            sorted.Sort();
            string  key = "";
            foreach(var elem in  sorted)
                key += elem + " ";
            return key;
        }
    }
}