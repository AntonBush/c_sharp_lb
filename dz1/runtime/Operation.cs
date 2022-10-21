namespace dz1.runtime;

interface Operation
{
    public void operate(Stack<double> operands);
}

class Addition : Operation
{
    static public readonly Addition instance = new Addition();

    void Operation.operate(Stack<double> operands)
    {
        operands.Push(operands.Pop() + operands.Pop());
    }

    override public string ToString()
    {
        return "+";
    }
}
class Subtraction : Operation
{
    static public readonly Subtraction instance = new Subtraction();

    void Operation.operate(Stack<double> operands)
    {
        double rhs = operands.Pop();
        double lhs = operands.Pop();
        operands.Push(lhs - rhs);
    }

    override public string ToString()
    {
        return "-";
    }
}
class Multiplication : Operation
{
    static public readonly Multiplication instance = new Multiplication();

    void Operation.operate(Stack<double> operands)
    {
        operands.Push(operands.Pop() * operands.Pop());
    }

    override public string ToString()
    {
        return "*";
    }
}
class Division : Operation
{
    static public readonly Division instance = new Division();

    void Operation.operate(Stack<double> operands)
    {
        double rhs = operands.Pop();
        double lhs = operands.Pop();
        operands.Push(lhs / rhs);
    }

    override public string ToString()
    {
        return "/";
    }
}
class Operand : Operation
{
    public Operand(double operand)
    {
        this._operand = operand;
    }

    void Operation.operate(Stack<double> operands)
    {
        operands.Push(_operand);
    }

    override public string ToString()
    {
        return $"constant{{{_operand.ToString()}}}";
    }

    double _operand;
}
