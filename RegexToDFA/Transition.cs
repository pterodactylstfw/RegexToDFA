namespace RegexToDFA
{
    public class Transition
    {
        public int FromState { get; set; }
        public int ToState { get; set; }
        public char Symbol { get; set; }
        
        public Transition(int fromState, int toState, char symbol)
        {
            this.FromState = fromState;
            this.ToState = toState;
            this.Symbol = symbol;
        }
    }
}