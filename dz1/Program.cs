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

using dz1.utility;

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
    Console.WriteLine($"{spaced_input} = {Utility.algorithm(input)}");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

void test()
{
    string[] inputs = { "1+2", "(234-11)*34", "6*6/6" };
    string[] outputs = { "3", "7582", "6" };

    for (int i = 0; i < inputs.Length; ++i)
    {
        Console.WriteLine($"{inputs[i]} = {outputs[i]}");
        string result = Utility.algorithm(inputs[i]);
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
