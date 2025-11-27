using System;
using System.Collections.Generic;

namespace RegexToDFA
{
    public class RegexToLambdaAutomaton
    {
        public static LambdaAutomaton build(string regexPostfix)
        {
            LambdaAutomaton.resetStateCount();
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
                    automatonStack.Push(LambdaAutomaton.concatenation(left, right));
                }
                else if (c == '*')
                {
                    LambdaAutomaton automaton = automatonStack.Pop();
                    automaton.kleeneStar();
                    automatonStack.Push(automaton);
                }
                else if (c == '+')
                {
                    LambdaAutomaton automaton = automatonStack.Pop();
                    automaton.plus();
                    automatonStack.Push(automaton);
                }
            }
            if (automatonStack.Count > 0)
                return automatonStack.Pop();
            else
                return null;
        }
    }
}