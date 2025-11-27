namespace RegexToDFA
{
    public class Transition
    {
        public int fromState { get; set; }
        public int toState { get; set; }
        public char symbol { get; set; }
        
        public Transition(int fromState, int toState, char symbol)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.symbol = symbol;
        }
    }
}