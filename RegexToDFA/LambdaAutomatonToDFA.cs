using System.Collections.Generic;
using System.Linq;

namespace RegexToDFA
{
    public class LambdaAutomatonToDFA
    {
        public static DeterministicFiniteAutomaton Build(LambdaAutomaton lambdaAutomaton)
        {
            DeterministicFiniteAutomaton dfa = new DeterministicFiniteAutomaton();

            foreach (var transition in lambdaAutomaton.Transitions)
            {
                if (transition.Symbol != LambdaAutomaton.Lambda)
                    dfa.Symbols.Add(transition.Symbol);
            }
            
            Dictionary<string, int> dfaStateMap = new Dictionary<string, int>();
            Queue<HashSet<int>> queue = new Queue<HashSet<int>>();
            
            HashSet<int> startSet = GetLambdaClosure(lambdaAutomaton.InitialState, lambdaAutomaton.Transitions);
            string startKey = GetSetKey(startSet);
            
            int countDfaState = 0;
            dfaStateMap[startKey] = countDfaState;
            dfa.States.Add(countDfaState);
            dfa.InitialState = countDfaState;
            if (startSet.Contains(lambdaAutomaton.FinalState))
            {
                dfa.FinalStates.Add(countDfaState);
            }
            
            countDfaState++;
            queue.Enqueue(startSet);

            while (queue.Count > 0)
            {
                HashSet<int> currentSet = queue.Dequeue();
                int currentDfaId = dfaStateMap[GetSetKey(currentSet)];

                foreach (var symbol in dfa.Symbols)
                {
                    HashSet<int> foundStates = new HashSet<int>();

                    foreach (var transition in lambdaAutomaton.Transitions)
                    {
                        if (transition.Symbol == symbol && currentSet.Contains(transition.FromState))
                        {
                            foundStates.Add(transition.ToState);
                        }
                    }

                    if (foundStates.Count() > 0)
                    {
                        HashSet<int> newSet = new HashSet<int>();
                        foreach (int state in foundStates)
                        {
                            newSet.UnionWith(GetLambdaClosure(state,  lambdaAutomaton.Transitions));
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
                            
                            if (newSet.Contains(lambdaAutomaton.FinalState))
                            {
                                dfa.FinalStates.Add(nextStateId);
                            }
                            dfa.States.Add(nextStateId);
                            
                            countDfaState++;
                        }
                        
                        dfa.Transitions.Add(new Transition(currentDfaId, nextStateId, symbol));
                    }
                }
            }

            return dfa;
        }

        private static HashSet<int> GetLambdaClosure(int state, List<Transition> transitions)
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
                    if (transition.FromState == currentState && transition.Symbol == LambdaAutomaton.Lambda)
                    {
                        if (!lambdaClosure.Contains(transition.ToState))
                        {
                            lambdaClosure.Add(transition.ToState);
                            stack.Push(transition.ToState);
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