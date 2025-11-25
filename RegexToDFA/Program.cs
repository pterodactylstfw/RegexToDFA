using System;

namespace RegexToDFA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string regex = "ab|c";
            string output = Regex.concatenationHandle(regex);
            Console.WriteLine(output);
            string postfix = Regex.toPostfixForm(output);
            Console.WriteLine(postfix);
        }
    }
}