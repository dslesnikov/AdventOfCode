using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day18
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private string[] _expressions;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 18;
        
        public async Task InitializeAsync()
        {
            _expressions = (await _reader.ReadLinesAsync()).Select(line => line.Replace(" ", "")).ToArray();
        }

        public string PartOne()
        {
            var precedence = new Dictionary<char, int>
            {
                {'+', 10},
                {'*', 10},
                {'(', 1 }
            };
            var result = _expressions
                .Select(line => EvaluateExpression(line, precedence))
                .Aggregate((acc, item) => acc + item);
            return result.ToString();
        }

        public string PartTwo()
        {
            var precedence = new Dictionary<char, int>
            {
                {'+', 20},
                {'*', 10},
                {'(', 1 }
            };
            var result = _expressions
                .Select(line => EvaluateExpression(line, precedence))
                .Aggregate((acc, item) => acc + item);
            return result.ToString();
        }

        private ulong EvaluateExpression(string expression, Dictionary<char, int> precedence)
        {
            var valueStack = new Stack<ulong>();
            var operatorStack = new Stack<char>();
            foreach (var token in expression)
            {
                if (char.IsDigit(token))
                {
                    valueStack.Push((ulong)(token - '0'));
                }
                else if (token == '(')
                {
                    operatorStack.Push('(');
                }
                else if (token == ')')
                {
                    while (operatorStack.Peek() != '(')
                    {
                        EvaluateOnce(valueStack, operatorStack);
                    }
                    operatorStack.Pop();
                }
                else
                {
                    var operation = token;
                    while (operatorStack.Count > 0 && precedence[operatorStack.Peek()] >= precedence[operation])
                    {
                        EvaluateOnce(valueStack, operatorStack);
                    }
                    operatorStack.Push(operation);
                }
            }
            while (operatorStack.Count > 0)
            {
                EvaluateOnce(valueStack, operatorStack);
            }
            return valueStack.Pop();
        }

        private static void EvaluateOnce(Stack<ulong> values, Stack<char> operators)
        {
            var op = operators.Pop();
            var first = values.Pop();
            var second = values.Pop();
            var result = op switch
            {
                '+' => first + second,
                '*' => first * second,
                _ => throw new NotSupportedException()
            };
            values.Push(result);
        } 
    }
}