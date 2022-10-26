/*

Задание 1: даны два массива a и b размерностью n и m соответственно,
сформировать массив c таким образом, что первая часть — отсортированный
по возрастанию массив а, а вторая часть — отсортированный по
убыванию массив b.

*/

lb2_1.IntValidator pozitiveInt = (i) => { return 0 < i; };
lb2_1.IntValidator anyInt = (_) => { return true; };

int n = lb2_1.Console.readInt("Enter n (integer, 0 < n):", pozitiveInt);
int[] a = new int[n];
for (int i = 0; i < n; ++i)
{
    a[i] = lb2_1.Console.readInt("Enter a[" + i.ToString() + "] (integer):", anyInt);
}

int m = lb2_1.Console.readInt("Enter m (integer, 0 < m):", pozitiveInt);
int[] b = new int[m];
for (int i = 0; i < m; ++i)
{
    b[i] = lb2_1.Console.readInt("Enter b[" + i.ToString() + "] (integer):", anyInt);
}

Console.WriteLine("Entered:");
writeArray(a, "a");
writeArray(b, "b");

sortIntArray(a, true);
sortIntArray(b, false);

Console.WriteLine("Sorted:");
writeArray(a, "a");
writeArray(b, "b");

void writeArray(int[] array, string array_name)
{
    Console.WriteLine(array_name + "[" + array.Length.ToString() + "] {");
    for (int i = 0; i < array.Length; ++i)
    {
        Console.WriteLine("  " + array_name + "[" + i.ToString() + "] = " + array[i].ToString());
    }
    Console.WriteLine("}");
}

void sortIntArray(int[] array, bool ascending_order)
{
    int n_swaps = 1;
    while (n_swaps != 0)
    {
        n_swaps = 0;
        for (int i = 0; i < array.Length - 1; ++i)
        {
            if (ascending_order ? array[i + 1] < array[i] : array[i] < array[i + 1])
            {
                int temp = array[i];
                array[i] = array[i + 1];
                array[i + 1] = temp;
                ++n_swaps;
            }
        }
    }
}

namespace lb2_1
{
    public delegate bool IntValidator(int i);

    public class Console
    {
        public static bool readInt(out int i)
        {
            return int.TryParse(System.Console.ReadLine(), out i);
        }

        public static int readInt(string message, IntValidator validateInt)
        {
            int x;

            do
            {
                System.Console.WriteLine(message);
            } while ((!readInt(out x)) || (!validateInt(x)));

            return x;
        }
    }
}
