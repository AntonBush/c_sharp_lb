/*

Создать консольное приложение – калькулятор. Необходимо обеспечить 
защиту от неправильного ввода, выбор операций, а также защиту от деления 
на ноль.

*/

lb1.Calculator calculator = new lb1.Calculator();

Console.WriteLine("App: Calculator.");
printHelp();

while (true)
{
    Console.Write($"{calculator} >>> ");
    if (calculator.operation == null)
    {
        Console.Write("change first operand or set operation: ");
    }
    else
    {
        Console.Write("change operation or set second operand: ");
    }

    var input = Console.ReadLine();

    if (input == null)
    {
        Console.WriteLine("Nothing was read.");
        printHelp();
        continue;
    }
    Console.WriteLine($"Entered: {input}");
    var trimed_input = input.Trim();
    Console.WriteLine($"Wil be processed: {trimed_input}");
    if (trimed_input == "quit")
    {
        break;
    }

    double temp;
    lb1.Operation? op;
    if (calculator.operation == null)
    {
        if (double.TryParse(trimed_input, out temp))
        {
            calculator.lhs = temp;
            continue;
        }

        op = lb1.Operation.from(trimed_input);
        if (op != null)
        {
            calculator.operation = op;
            continue;
        }
        Console.WriteLine("Invalid input.");
    }
    else
    {
        op = lb1.Operation.from(trimed_input);
        if (op != null)
        {
            calculator.operation = op;
            continue;
        }

        if (double.TryParse(trimed_input, out temp))
        {
            calculator.rhs = temp;
            try
            {
                Console.WriteLine($"Result: {calculator.calculate()}");
                continue;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Division by zero happend");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }

    printHelp();
}

Console.WriteLine("App was closed.");

void printHelp()
{
    Console.WriteLine("Enter 'quit' to close app;");
    Console.WriteLine("Enter real number to change operand;");
    Console.WriteLine("Enter {'+', '-', '*', '/'} to change operation;");
}

namespace lb1
{
    class Operation
    {
        public static Operation addition = new Operation("+", (lhs, rhs) => { return lhs + rhs; });
        public static Operation subtraction = new Operation("-", (lhs, rhs) => { return lhs - rhs; });
        public static Operation multiplication = new Operation("*", (lhs, rhs) => { return lhs * rhs; });
        public static Operation division = new Operation("/", (lhs, rhs) => { if (rhs == 0) throw new DivideByZeroException(); return lhs / rhs; });

        public static Operation? from(string s)
        {
            switch (s)
            {
                case "+": return addition;
                case "-": return subtraction;
                case "*": return multiplication;
                case "/": return division;
            }
            return null;
        }

        public double operate(double lhs, double rhs)
        {
            return _operation(lhs, rhs);
        }

        public override string ToString()
        {
            return _operation_string;
        }

        delegate double _Operation(double lhs, double rhs);
        string _operation_string;
        _Operation _operation;
        Operation(string s, _Operation operation)
        {
            _operation_string = s;
            _operation = operation;
        }
    }

    class Calculator
    {
        public double lhs { get; set; }
        public Operation? operation { get; set; }
        public Nullable<double> rhs { get; set; }

        public Calculator()
        {
            lhs = 0;
            operation = null;
            rhs = null;
        }

        public override string ToString()
        {
            string s = lhs.ToString();
            if (operation == null)
            {
                return s;
            }

            s += $" {operation}";
            if (!rhs.HasValue)
            {
                return s;
            }

            return $"{s} {rhs.Value}";
        }

        public double calculate()
        {
            if (operation == null)
            {
                throw new Exception("Operation is null");
            }
            if (!rhs.HasValue)
            {
                throw new Exception("Rhs is null");
            }
            try
            {
                lhs = operation.operate(lhs, rhs.Value);
            }
            catch (DivideByZeroException e)
            {
                rhs = null;
                throw e;
            }
            operation = null;
            rhs = null;
            return lhs;
        }
    }
}
