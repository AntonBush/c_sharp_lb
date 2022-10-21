/*

Задание:
Нужно реализовать консольный калькулятор,
умеющий вычислять выражение, подаваемое на STDIN.

Требуется реализовать:
- сложение;
- вычитание;
- умножение;
- деление;
- поддержка скобок.

Нужно написать тесты, которые покрывают все операции.
Пример: 
"(2+3)-4" => 1
"4-(2*3)" => 2

Варианты:
!- Четные по списку - методом Бауэра-Земельзона 
- Нечетные по списку - стековым методом

*/

using parsing = dz1.parsing;
using runtime = dz1.runtime;

test();

var input_array = string.Join(" ", args)
                            .Split(new char[] { ' ', '\t', '\n', '\r' })
                            .Where((str) => { return str != ""; });
var spaced_input = string.Join(" ", input_array);
Console.WriteLine($"Entered: {spaced_input}");

var input = string.Join("", input_array);
Console.WriteLine($"Will be processed: {input}");
try
{
    Console.WriteLine($"{spaced_input} = {algorithm(input)}");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

string algorithm(string input)
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

List<runtime.Operation> parse(string input)
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

List<parsing.Symbol> tokenize(string input)
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

void test()
{
    string[] inputs = { "1+2", "(234-11)*34", "6*6/6" };
    string[] outputs = { "3", "7582", "6" };

    for (int i = 0; i < inputs.Length; ++i)
    {
        Console.WriteLine($"{inputs[i]} = {outputs[i]}");
        string result = algorithm(inputs[i]);
        if (result == outputs[i])
        {
            Console.WriteLine("OK");
        }
        else
        {
            Console.WriteLine($"Failed: {result}");
        }
    }
}
