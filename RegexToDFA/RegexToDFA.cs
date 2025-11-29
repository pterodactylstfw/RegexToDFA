

namespace RegexToDFA
{
    public class RegexToDFA
    {
        public static DeterministicFiniteAutomaton regexToDFA(string regex)
        {
            string concatRegex = RegexUtils.concatenationHandle(regex);
            string regexPostfix = RegexUtils.toPostfixForm(concatRegex);
            LambdaAutomaton lambdaAutomaton = RegexToLambdaAutomaton.build(regexPostfix);
            DeterministicFiniteAutomaton dfa = LambdaAutomatonToDFA.dfa(lambdaAutomaton);
            return dfa;
        }
    }
}