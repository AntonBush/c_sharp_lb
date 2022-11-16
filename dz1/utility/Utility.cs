namespace dz1.utility;

using parsing = dz1.parsing;
using runtime = dz1.runtime;

public class Utility
{
    public static string algorithm(string input)
    {
        if (input == "")
        {
            throw new Exception("Input string is empty");
        }

        var operands = new Stack<double>();
        var operations = parse(input).ToArray();

        try
        {
            for (int i = 0; i < operations.Length; ++i)
            {
                operations[i].operate(operands);
            }

            if (operands.Count == 0)
            {
                throw new Exception($"Operand stack is empty after operation execution!");
            }
        }
        catch (Exception e)
        {
            var operation_string = string.Join<runtime.Operation>(", ", operations);
            throw new Exception($"Runtime error. Message: {e.Message}. Operations: {operation_string}");
        }

        return operands.Pop().ToString();
    }

    static List<runtime.Operation> parse(string input)
    {
        var parser = new parsing.Parser();
        var operations = new List<runtime.Operation>();

        var symbols = tokenize(input).ToArray();

        try
        {
            for (int i = 0; i < symbols.Length; ++i)
            {
                var ops = parser.putSymbol(symbols[i]);
                if (ops != null)
                {
                    operations.AddRange(ops);
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Parse error. Message: {e.Message} Tokens: {string.Join<parsing.Symbol>(", ", symbols)}.");
        }

        return operations;
    }

    static List<parsing.Symbol> tokenize(string input)
    {
        var symbols = new List<parsing.Symbol>();

        uint number = 0;
        bool is_last_digit = false;
        for (int i = 0; i < input.Length; ++i)
        {
            if (char.IsDigit(input[i]))
            {
                number *= 10;
                number += (uint)(input[i] - '0');
                is_last_digit = true;
                continue;
            }

            if (is_last_digit)
            {
                symbols.Add(new parsing.Symbol(number));
                number = 0;
                is_last_digit = false;
            }

            switch (input[i])
            {
                case parsing.OperationSymbol.add:
                    symbols.Add(parsing.Symbol.add);
                    break;
                case parsing.OperationSymbol.sub:
                    symbols.Add(parsing.Symbol.sub);
                    break;
                case parsing.OperationSymbol.mul:
                    symbols.Add(parsing.Symbol.mul);
                    break;
                case parsing.OperationSymbol.div:
                    symbols.Add(parsing.Symbol.div);
                    break;
                case parsing.OperationSymbol.brl:
                    symbols.Add(parsing.Symbol.brl);
                    break;
                case parsing.OperationSymbol.brr:
                    symbols.Add(parsing.Symbol.brr);
                    break;
                default:
                    throw new Exception($"Tokenize error. Invalid character: '{input[i]}'");
            }
        }

        if (is_last_digit)
        {
            symbols.Add(new parsing.Symbol(number));
        }

        symbols.Add(parsing.Symbol.end);
        return symbols;
    }
}
