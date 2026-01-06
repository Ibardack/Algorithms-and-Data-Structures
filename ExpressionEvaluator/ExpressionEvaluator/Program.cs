using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using System;
using System.Collections.Generic;

public class ExpressionEvaluator
{
    private static Dictionary<string, int> precedence = new Dictionary<string, int> {
        { "^", 4 },
        { "*", 3 },
        { "/", 3 },
        { "+", 2 },
        { "-", 2 },
        { "or", 1 },
        { "and", 1 },
        { "not", 0 },
        { "(", 5 }
    };

    // Convert Infix to Postfix
    public static List<string> InfixToPostfix(string expression)
    {
        var operators = new Stack<string>();
        var output = new List<string>();
        var tokens = SplitExpression(expression);

        foreach (var token in tokens)
        {
            if (IsNumber(token) || IsVariable(token))  // Operand
            {
                output.Add(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Peek() != "(")
                {
                    output.Add(operators.Pop());
                }
                operators.Pop();  // Remove '('
            }
            else if (IsOperator(token))  // Operator
            {
                while (operators.Count > 0 && precedence[operators.Peek()] >= precedence[token])
                {
                    output.Add(operators.Pop());
                }
                operators.Push(token);
            }
        }

        // Pop remaining operators
        while (operators.Count > 0)
        {
            output.Add(operators.Pop());
        }

        return output;
    }

    // Evaluate the Postfix Expression
    public static double EvaluatePostfix(List<string> postfix, Dictionary<string, double> variables)
    {
        var stack = new Stack<double>();

        foreach (var token in postfix)
        {
            if (IsNumber(token))  // Operand (number)
            {
                stack.Push(double.Parse(token));
            }
            else if (IsVariable(token))  // Variable
            {
                stack.Push(variables[token]);
            }
            else if (IsOperator(token))  // Operator
            {
                double operand2 = stack.Pop();
                double operand1 = stack.Pop();
                double result = ApplyOperator(token, operand1, operand2);
                stack.Push(result);
            }
        }

        return stack.Pop();  // The final result
    }

    // Helper methods
    private static double ApplyOperator(string op, double operand1, double operand2)
    {
        return op switch
        {
            "+" => operand1 + operand2,
            "-" => operand1 - operand2,
            "*" => operand1 * operand2,
            "/" => operand1 / operand2,
            "^" => Math.Pow(operand1, operand2),
            "and" => Convert.ToBoolean(operand1) && Convert.ToBoolean(operand2) ? 1 : 0,
            "or" => Convert.ToBoolean(operand1) || Convert.ToBoolean(operand2) ? 1 : 0,
            _ => throw new InvalidOperationException("Operator not supported")
        };
    }

    // Split the expression into tokens
    public static List<string> SplitExpression(string expression)
    {
        var tokens = new List<string>();
        string current = "";

        foreach (var ch in expression)
        {
            if (char.IsWhiteSpace(ch)) continue;

            if ("+-*/^()".Contains(ch))
            {
                if (current.Length > 0) tokens.Add(current);
                tokens.Add(ch.ToString());
                current = "";
            }
            else
            {
                current += ch;
            }
        }

        if (current.Length > 0) tokens.Add(current);
        return tokens;
    }

    // Check if a token is an operator
    private static bool IsOperator(string token)
    {
        return precedence.ContainsKey(token);
    }

    // Check if a token is a number
    private static bool IsNumber(string token)
    {
        return double.TryParse(token, out _);
    }

    // Check if a token is a variable
    private static bool IsVariable(string token)
    {
        return !IsOperator(token) && !IsNumber(token) && !string.IsNullOrWhiteSpace(token);
    }

    // Method to print the postfix expression
    public static void PrintPostfix(List<string> postfix)
    {
        Console.WriteLine("Postfix Expression:");
        foreach (var token in postfix)
        {
            Console.Write(token + " ");
        }
        Console.WriteLine();
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Expression Evaluator!");

        Console.WriteLine("Input your expression (e.g., A + B * C): ");
        string expression = Console.ReadLine().Trim();

        var variables = new Dictionary<string, double>();
        var variableNames = new HashSet<string>();

        var tokens = SplitExpression(expression);

        // Collect all variable names
        foreach (var token in tokens)
        {
            if (!IsNumber(token) && !IsOperator(token) && !token.Equals("(") && !token.Equals(")"))
            {
                variableNames.Add(token);
            }
        }

        if (variableNames.Count > 0)  // Ask for values for variables
        {
            Console.WriteLine("This expression contains variables. Enter values for each variable:");

            foreach (var variable in variableNames)
            {
                Console.Write($"Enter value for {variable}: ");
                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    variables[variable] = value;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    return;
                }
            }
        }

        // Convert the infix expression to postfix
        var postfix = InfixToPostfix(expression);

        // Print the postfix expression
        PrintPostfix(postfix);

        // Evaluate the postfix expression
        try
        {
            double result = EvaluatePostfix(postfix, variables);
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}