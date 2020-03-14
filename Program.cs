using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SymbolicDifferentiator
{
    class Program
    {
        public enum Variables
        {@x}

        static void Main(string[] args)
        {
            Console.WriteLine("Please type a polynomial expression (e.g. 'x^2 +  5* x - 5')");
            string input = Console.ReadLine();
            Console.WriteLine("{0}", Parser(input));
        }

        private static string Parser(string expression)
        {
            //if our input has no variables, just return 0.
            if (!Enum.GetNames(typeof(Variables)).Any(e => expression.Contains(e)))
                return "0";

            //remove all whitespace to make things easier.
            expression = Regex.Replace(expression, @"\s+", "");

            if (expression[0] == '*' || expression[0] == '/' || expression[0] == '^')
                return "Invalid expression. Invalid usage of operators.";

            for(int i = 0; i < expression.Length - 1; i++)
            {
                if ((expression[i] == '-' || expression[i] == '+' || expression[i] == '/' ||
                    expression[i] == '^') && (!Char.IsDigit(expression[i + 1]) && expression[i+1] != 'x'))
                    return "Invalid expression. Consecutive operators.";
            }

            if (expression[expression.Length - 1] == '-' ||
                expression[expression.Length - 1] == '+' ||
                expression[expression.Length - 1] == '*' ||
                expression[expression.Length - 1] == '/')
                return "Invalid expression. Invalid usage of operators.";

            return Differentiator(expression);
        }

        private static string Differentiator(string expression)
        {
            if (expression.Contains("+"))
            {
                string leftHand = expression.Substring(0, expression.IndexOf("+"));
                string rightHand = expression.Substring(expression.IndexOf("+"), expression.Length - expression.IndexOf("+")).Substring(1);
                expression = Differentiator(leftHand) + "      " + Differentiator(rightHand);
            }

            else if (expression.Contains("-"))
            {
                string leftHand = expression.Substring(0, expression.IndexOf("-"));
                string rightHand = expression.Substring(expression.IndexOf("-"), expression.Length - expression.IndexOf("-")).Substring(1);
                expression = Differentiator(leftHand) + "      " + Differentiator(rightHand);
            }

            else if (expression.Contains("*x"))
                expression = expression.Replace("*x", "");

            else if (expression.Contains("x*"))
                expression = expression.Replace("x*", "");

            else if (expression.Contains("^"))
                expression = expression.Replace("x^", "") + "x";

            else if (expression.Length == 1 && expression.Contains("x"))
                expression = "1";

            else if (!expression.Contains("x"))
                expression = "0";


            return expression;
        }
    }
}
