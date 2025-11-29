

namespace RegexToDFA
{
    public class RegexToDFA
    {
        public static DeterministicFiniteAutomaton RegexToDfa(string regex)
        {
            string concatRegex = RegexUtils.ConcatenationHandle(regex);
            string regexPostfix = RegexUtils.ToPostfixForm(concatRegex);
            LambdaAutomaton lambdaAutomaton = RegexToLambdaAutomaton.Build(regexPostfix);
            DeterministicFiniteAutomaton dfa = LambdaAutomatonToDFA.Build(lambdaAutomaton);
            return dfa;
        }
    }
}