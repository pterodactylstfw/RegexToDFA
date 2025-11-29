using System;
using System.Collections.Generic;

namespace RegexToDFA
{
    public class RegexUtils
    {
        /*
        Functie care adauga . la concatenare
        Cazurile posibile:
         1  a . b 
         2  a . ( 
         3  ) . a 
         4  * . a
         5  * . ( 
         6  ) . ( 
         7  + . a
         8  + . ( 
         * */
        public static string ConcatenationHandle(string regex)
        {
            string output = "";
            for (int i = 0; i < regex.Length; i++)
            {
                char currentChar = regex[i];
                output += currentChar;

                if (i + 1 < regex.Length)
                {
                    char nextChar = regex[i + 1];
                    if ((Char.IsLetter(currentChar) || currentChar == '*' || currentChar == '+' || currentChar == ')')
                        && (Char.IsLetter(nextChar) || nextChar == '('))
                    {
                        output += '.';
                    }
                }
            }
            return output;
        }

        // functie cu ajutorul careia setam precedenta operatorilor
        private static int GetPrecedence(char op)
        {
            switch (op)
            {
                case '*': return 3;
                case '+': return 3;
                case '.':  return 2;
                case '|':  return 1;
                default: return 0;
            }
        }

        // functie care face transformarea in forma poloneza postfixata
        public static string ToPostfixForm(string regex)
        {
            string output = "";
            // creem o stiva de operatori
            Stack<char> operators = new Stack<char>();
            
            foreach(char c in regex)
            {
                if(Char.IsLetter(c))
                    output += c;
                else if(c == '(')
                    operators.Push(c);
                else if (c == ')')
                {
                    // cand gasim o paranteza inchisa scoatem din stiva operatorii si ii adaugam in forma finala
                    while(operators.Count > 0 &&  operators.Peek() != '(')
                        output += operators.Pop();
                    // ajungem la ( si o scoatem din stiva
                    if (operators.Count > 0)
                        operators.Pop();
                }
                else
                {
                    /* In cazul in care gasim un operator, verificam precedenta.
                     Scoatem din stiva si adaugam in forma finala daca in stiva se afla un operator
                     cu precedenta mai mare decat operatorul curent. */
                    while(operators.Count > 0 && GetPrecedence(operators.Peek()) >= GetPrecedence(c))
                        output += operators.Pop();
                    operators.Push(c);
                }
            }
            
            // golim ce a ramas in stiva
            while(operators.Count > 0)
                output += operators.Pop();

            return output;
        }

        public class TreeNode
        {
            public char Symbol { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            
            public TreeNode(char symb, TreeNode l = null, TreeNode r = null)
            {
                Symbol = symb;
                Left = l;
                Right = r;
            }
        }

        public static TreeNode SyntaxTree(string regexPostfix)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();

            foreach (char c in regexPostfix)
            {
                if(Char.IsLetter(c))
                    stack.Push(new TreeNode(c));
                else if (c == '*' || c == '+')
                {
                    TreeNode child = stack.Pop();
                    stack.Push(new TreeNode(c, null, child));
                }
                else if (c == '|' || c == '.')
                {
                    TreeNode rightChild =  stack.Pop();
                    TreeNode leftChild = stack.Pop();
                    stack.Push(new TreeNode(c, leftChild, rightChild));
                }
            }
            
            return stack.Pop();
        }

        public static void PrintSyntaxTree(TreeNode root, string indent = "", bool isLast = true)
        {
            if (root != null)
            {
                Console.Write(indent);
                if (isLast)
                {
                    Console.Write("└─");
                    indent += "  ";
                }
                else
                {
                    Console.Write("├─");
                    indent += "| ";
                }
                Console.WriteLine(root.Symbol);
                PrintSyntaxTree(root.Left, indent, root.Right == null);
                PrintSyntaxTree(root.Right, indent, true);
                
            }
        }
        
        
    }
}