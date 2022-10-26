
/*

Задание: 
Составить программу деления вещественных чисел. программа должна 
выполнять обработку исключений c использованием конструкции try … catch, 
и выдавать следующие сообщения о характере ошибки:
1. не введено число (с помощью оператора условия);
2. введено слишком длинное число (с помощью оператора условия);
3. деление на ноль;
4. ошибка преобразования.

*/

Console.WriteLine("Welcome to Float Divider Tycoon!");
printHelp();

string? input;
lb8.Divider divider = new lb8.Divider();
while (true)
{
    input = Console.ReadLine();
    Console.WriteLine($"Entered: '{(input == null ? "null" : input)}'");

    try
    {
        if (input == null)
        {
            throw new Exception("Nothing was read.");
        }

        input = input.Trim();
        Console.WriteLine($"Trimed: '{input}'");

        if (input == "quit") break;
        if (input == "reset") { divider.reset(); continue; }

        if (20 < input.Length)
        {
            throw new Exception("Real number length must be less or equal 20.");
        }
        Console.WriteLine($"Result: {divideAndConquer(divider, input)}");
        continue;
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Division by zero happened.");
    }
    catch (FormatException)
    {
        Console.WriteLine("Entered line is not real number.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    printHelp();
}

Console.WriteLine("We will miss you");

void printHelp()
{
    Console.WriteLine("Enter 'quit' to exit program");
    Console.WriteLine("Or enter 'reset' to set numerator with the next command");
    Console.WriteLine("Or enter real number to divide numerator (default value is not set):");
}

double divideAndConquer(lb8.Divider divider, string s)
{
    return divider.divide(double.Parse(s));
}

namespace lb8
{
    public class Divider
    {
        public double divide(double denominator)
        {
            if (_numerator == null)
            {
                return (double)(_numerator = denominator);
            }

            if (denominator == 0)
            {
                throw new DivideByZeroException();
            }
            return (double)(_numerator /= denominator);
        }

        public void reset()
        {
            _numerator = null;
        }

        double? _numerator = null;
    };
}
