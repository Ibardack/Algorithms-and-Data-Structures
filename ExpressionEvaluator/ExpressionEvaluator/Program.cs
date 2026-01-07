using System;
using System.Collections.Generic;

public class ExpressionEvaluator
{
    private static Dictionary<string, int> precedence = new Dictionary<string, int>
    {
        { "u-", 5 },   // unary minus
        { "^", 4 },
        { "*", 3 },
        { "/", 3 },
        { "+", 2 },
        { "-", 2 },
        { ">", 1 },
        { "<", 1 },
        { ">=", 1 },
        { "<=", 1 },
        { "==", 1 },
        { "!=", 1 },
        { "and", 0 },
        { "or", 0 },
        { "not", 5 }
    };

    private static HashSet<string> rightAssociative = new HashSet<string>
    {
        "^", "u-", "not"
    };

    public static List<string> InfixToPostfix(string expression)
    {
        var operators = new Stack<string>();
        var output = new List<string>();
        var tokens = SplitExpression(expression);

        string previous = null;

        foreach (var token in tokens)
        {
            if (IsNumber(token) || IsVariable(token))
            {
                output.Add(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                    output.Add(operators.Pop());

                if (operators.Count == 0)
                    throw new InvalidOperationException("Mismatched parentheses");

                operators.Pop();
            }
            else if (IsOperator(token))
            {
                string op = token;

                // Detect unary minus
                if (op == "-" && (previous == null || IsOperator(previous) || previous == "("))
                    op = "u-";

                while (operators.Count > 0 &&
                       operators.Peek() != "(" &&
                       (
                           precedence[operators.Peek()] > precedence[op] ||
                           (
                               precedence[operators.Peek()] == precedence[op] &&
                               !rightAssociative.Contains(op)
                           )
                       ))
                {
                    output.Add(operators.Pop());
                }

                operators.Push(op);
            }

            previous = token;
        }

        while (operators.Count > 0)
        {
            var op = operators.Pop();
            if (op == "(")
                throw new InvalidOperationException("Mismatched parentheses");

            output.Add(op);
        }

        return output;
    }

    public static double EvaluatePostfix(List<string> postfix, Dictionary<string, double> variables)
    {
        var stack = new Stack<double>();

        foreach (var token in postfix)
        {
            if (IsNumber(token))
            {
                stack.Push(double.Parse(token));
            }
            else if (variables.ContainsKey(token))
            {
                stack.Push(variables[token]);
            }
            else if (IsOperator(token))
            {
                if (token == "u-")
                {
                    stack.Push(-stack.Pop());
                }
                else if (token == "not")
                {
                    stack.Push(Convert.ToBoolean(stack.Pop()) ? 0 : 1);
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();
                    stack.Push(ApplyOperator(token, a, b));
                }
            }
        }

        return stack.Pop();
    }

    private static double ApplyOperator(string op, double a, double b)
    {
        return op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            "^" => Math.Pow(a, b),

            ">" => a > b ? 1 : 0,
            "<" => a < b ? 1 : 0,
            ">=" => a >= b ? 1 : 0,
            "<=" => a <= b ? 1 : 0,
            "==" => a == b ? 1 : 0,
            "!=" => a != b ? 1 : 0,

            "and" => (Convert.ToBoolean(a) && Convert.ToBoolean(b)) ? 1 : 0,
            "or" => (Convert.ToBoolean(a) || Convert.ToBoolean(b)) ? 1 : 0,

            _ => throw new InvalidOperationException("Unsupported operator")
        };
    }

    public static List<string> SplitExpression(string expression)
    {
        var tokens = new List<string>();
        string current = "";

        for (int i = 0; i < expression.Length; i++)
        {
            char ch = expression[i];

            if (char.IsWhiteSpace(ch)) continue;

            if ("()+-*/^<>!=".Contains(ch))
            {
                if (current.Length > 0)
                {
                    tokens.Add(current);
                    current = "";
                }

                // Handle multi-character operators
                if (i + 1 < expression.Length &&
                    ("<>!=.".Contains(ch)) &&
                    expression[i + 1] == '=')
                {
                    tokens.Add($"{ch}=");
                    i++;
                }
                else
                {
                    tokens.Add(ch.ToString());
                }
            }
            else
            {
                current += ch;
            }
        }

        if (current.Length > 0)
            tokens.Add(current);

        return tokens;
    }

    private static bool IsOperator(string token)
    {
        return precedence.ContainsKey(token);
    }

    private static bool IsNumber(string token)
    {
        return double.TryParse(token, out _);
    }

    private static bool IsVariable(string token)
    {
        return !IsNumber(token) && !IsOperator(token) && token != "(" && token != ")";
    }

    public static void Main()
    {
        Console.Write("Enter expression: ");
        string expression = Console.ReadLine();

        var variables = new Dictionary<string, double>();
        foreach (var token in SplitExpression(expression))
        {
            if (IsVariable(token) && !variables.ContainsKey(token))
            {
                Console.Write($"Enter value for {token}: ");
                variables[token] = double.Parse(Console.ReadLine());
            }
        }

        var postfix = InfixToPostfix(expression);
        Console.WriteLine("Postfix: " + string.Join(" ", postfix));

        double result = EvaluatePostfix(postfix, variables);
        Console.WriteLine("Result: " + result);
    }
}
