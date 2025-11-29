using System;
using System.Collections.Generic;

namespace RegexToDFA
{
    public class RegexToLambdaAutomaton
    {
        public static LambdaAutomaton Build(string regexPostfix)
        {
            LambdaAutomaton.ResetStateCount();
            Stack<LambdaAutomaton> automatonStack = new Stack<LambdaAutomaton>();

            foreach (char c in regexPostfix)
            {
                if(Char.IsLetter(c))
                    automatonStack.Push(new LambdaAutomaton(c));
                else if (c == '|')
                {
                    LambdaAutomaton right = automatonStack.Pop();
                    LambdaAutomaton left = automatonStack.Pop();
                    automatonStack.Push(left |  right);
                }
                else if (c == '.')
                {
                    LambdaAutomaton right = automatonStack.Pop();
                    LambdaAutomaton left = automatonStack.Pop();
                    automatonStack.Push(LambdaAutomaton.Concatenation(left, right));
                }
                else if (c == '*')
                {
                    LambdaAutomaton automaton = automatonStack.Pop();
                    automaton.KleeneStar();
                    automatonStack.Push(automaton);
                }
                else if (c == '+')
                {
                    LambdaAutomaton automaton = automatonStack.Pop();
                    automaton.Plus();
                    automatonStack.Push(automaton);
                }
            }
            if (automatonStack.Count > 0)
                return automatonStack.Pop();
            
            return null;
        }
    }
}