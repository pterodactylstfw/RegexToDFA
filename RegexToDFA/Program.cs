using System;

namespace RegexToDFA
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string regex = "ab|c";
            string output = RegexUtils.concatenationHandle(regex);
            Console.WriteLine(output);
            string postfix = RegexUtils.toPostfixForm(output);
            Console.WriteLine(postfix);
        }
    }
}