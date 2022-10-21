namespace dz1.parsing;

using runtime = dz1.runtime;
using _ParserTransitionKey = Tuple<Symbol?, Symbol>;

class OperationSymbol
{
    public const char add = '+';
    public const char sub = '-';
    public const char mul = '*';
    public const char div = '/';
    public const char brl = '(';
    public const char brr = ')';
}
class Symbol
{
    public static readonly Symbol add = new Symbol("+");
    public static readonly Symbol sub = new Symbol("-");
    public static readonly Symbol mul = new Symbol("*");
    public static readonly Symbol div = new Symbol("/");
    public static readonly Symbol brl = new Symbol("(");
    public static readonly Symbol brr = new Symbol(")");
    public static readonly Symbol end = new Symbol("End");

    public Symbol(uint number)
    {
        _symbol = number.ToString();
    }

    Symbol(string s)
    {
        _symbol = s;
    }

    public override string ToString()
    {
        return _symbol;
    }

    public runtime.Operation? toOperation()
    {
        if (this == add)
        {
            return runtime.Addition.instance;
        }
        else if (this == sub)
        {
            return runtime.Subtraction.instance;
        }
        else if (this == mul)
        {
            return runtime.Multiplication.instance;
        }
        else if (this == div)
        {
            return runtime.Division.instance;
        }

        uint number;
        if (uint.TryParse(_symbol, out number))
        {
            return new runtime.Operand(number);
        }

        return null;
    }

    string _symbol;
}

class Parser
{
    public Parser()
    {
        _transition_table = new Dictionary<_ParserTransitionKey, _Operation>();

        _transition_table.Add(new _ParserTransitionKey(null, Symbol.add), _transition1);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.sub), _transition1);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.mul), _transition1);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.div), _transition1);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.brr), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(null, Symbol.end), _no_transition);


        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.add), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.sub), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.mul), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.div), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.brr), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.add, Symbol.end), _transition4);

        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.add), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.sub), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.mul), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.div), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.brr), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.sub, Symbol.end), _transition4);


        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.add), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.sub), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.mul), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.div), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.brr), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.mul, Symbol.end), _transition4);

        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.add), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.sub), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.mul), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.div), _transition2);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.brr), _transition4);
        _transition_table.Add(new _ParserTransitionKey(Symbol.div, Symbol.end), _transition4);


        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.add), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.sub), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.mul), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.div), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.brl), _transition1);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.brr), _transition3);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brl, Symbol.end), _throw_transition);

        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.add), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.sub), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.mul), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.div), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.brl), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.brr), _throw_transition);
        _transition_table.Add(new _ParserTransitionKey(Symbol.brr, Symbol.end), _throw_transition);

        _operations = new Stack<Symbol>();
    }

    public List<runtime.Operation>? putSymbol(Symbol symbol)
    {
        {
            uint number;

            if (uint.TryParse(symbol.ToString(), out number))
            {
                var l = new List<runtime.Operation>();
                l.Add(new runtime.Operand(number));
                return l;
            }
        }

        var stack_top = _operations.Count == 0 ? null : _operations.Peek();
        return _transition_table[new _ParserTransitionKey(stack_top, symbol)](_operations, symbol);
    }

    delegate List<runtime.Operation>? _Operation(Stack<Symbol> stack, Symbol symbol);
    Dictionary<_ParserTransitionKey, _Operation> _transition_table;
    Stack<Symbol> _operations;

    List<runtime.Operation>? _transition1(Stack<Symbol> stack, Symbol symbol)
    {
        stack.Push(symbol);
        return null;
    }
    List<runtime.Operation>? _transition2(Stack<Symbol> stack, Symbol symbol)
    {
        var ops = new List<runtime.Operation>();
        {
            var temp = stack.Pop().toOperation();
            if (temp == null)
            {
                throw new Exception("Transition table is invalid");
            }
            ops.Add(temp);
        }
        stack.Push(symbol);

        return ops;
    }
    List<runtime.Operation>? _transition3(Stack<Symbol> stack, Symbol symbol)
    {
        stack.Pop();
        return null;
    }
    List<runtime.Operation>? _transition4(Stack<Symbol> stack, Symbol symbol)
    {
        var ops = new List<runtime.Operation>();
        {
            var stack_top = stack.Pop().toOperation();
            if (stack_top == null)
            {
                throw new Exception("Transition table is invalid");
            }
            ops.Add(stack_top);
        }

        var temp = new _ParserTransitionKey(stack.Count == 0 ? null : stack.Peek(), symbol);
        var extra_ops = _transition_table[temp](stack, symbol);

        if (extra_ops != null)
        {
            ops.AddRange(extra_ops);
        }

        return ops;
    }
    List<runtime.Operation>? _no_transition(Stack<Symbol> stack, Symbol symbol)
    {
        return null;
    }
    List<runtime.Operation>? _throw_transition(Stack<Symbol> stack, Symbol symbol)
    {
        throw new Exception($"Unexpected symbol: {symbol}");
    }
}
