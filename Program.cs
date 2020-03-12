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
            Console.WriteLine("Please type a polynomial expression (e.g. 'x^2 + x - 5 = 9')");
            string input = Console.ReadLine();
            Console.WriteLine("{0}", Parser(input));
        }

        private static string Parser(string expression)
        {
            if (!Enum.GetNames(typeof(Variables)).Any(e => expression.Contains(e)))
            {
                return "Invalid expression. No variable detected.";
            }

            int equalCount = Regex.Matches(expression, "=").Count;
            if (equalCount > 1)
                return "Invalid expression. Multiple '=' detected.";

            else if(equalCount == 1)
            {
                string[] polyonim = expression.Split("=");
                if (String.IsNullOrWhiteSpace(polyonim[0]) || String.IsNullOrWhiteSpace(polyonim[1]))
                    return "Invalid expression.";
            }

            expression = Regex.Replace(expression, @"\s+", "");
            if (expression[0] == '*' || expression[0] == '/' || expression[0] == '^')
                return "Invalid expression. Invalid usage of operators.";

            for(int i = 0; i < expression.Length - 1; i++)
            {
                if ((expression[i] == '-' || expression[i] == '+' || expression[i] == '/' ||
                    expression[i] == '^' || expression[i] == '=') && (!Char.IsDigit(expression[i + 1]) && expression[i+1] != 'x'))
                    return "Invalid expression. Consecutive operators.";
            }

            if (expression[expression.Length - 1] == '-' ||
                expression[expression.Length - 1] == '+' ||
                expression[expression.Length - 1] == '*' ||
                expression[expression.Length - 1] == '/')
                return "Invalid expression. Invalid usage of operators.";

            return Differentiator(expression);
        }

        public static string Differentiator(string expression)
        {
            if (!expression.Contains("x"))
                expression = "0";

            if (expression.Contains("+"))
            {
                string leftHand = expression.Substring(0, expression.IndexOf("+"));
                string rightHand = expression.Substring(expression.IndexOf("+"), expression.Length - expression.IndexOf("+")).Substring(1);
                expression = Differentiator(leftHand) + "+" + Differentiator(rightHand);
            }

            if (expression.Contains("-"))
            {
                string leftHand = expression.Substring(0, expression.IndexOf("-"));
                string rightHand = expression.Substring(expression.IndexOf("-"), expression.Length - expression.IndexOf("-")).Substring(1);
                expression = Differentiator(leftHand) + "-" + Differentiator(rightHand);
            }

            if (expression.Length == 1 && expression.Contains("x"))
                expression =  "1";

            if (expression.Contains("*"))
                 expression = expression.Replace("*x", "");

            if (expression.Contains("^"))
                 expression = expression.Replace("x^", "") + "x";

            return expression;
        }
    }
}
