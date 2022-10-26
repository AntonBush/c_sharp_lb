/*

Задание 2: создать двумерный массив, размерность задается
пользователем, заполнить его случайными числами в диапазоне от 0 до 9.
Отсортировать элементы массива по возрастанию вначале по строкам, а затем
по столбцам. Вывести на экран исходный массив, массив отсортированный по-
строчно, массив отсортированный по столбцам.

*/

lb2_1.IntValidator pozitiveInt = (i) => { return 0 < i; };
lb2_1.IntValidator anyInt = (_) => { return true; };

int n = lb2_1.Console.readInt("Enter n (integer, 0 < n):", pozitiveInt);
int m = lb2_1.Console.readInt("Enter m (integer, 0 < m):", anyInt);
int[,] a = new int[n, m];

for (int i = 0; i < n; ++i)
{
    for (int j = 0; j < m; ++j)
    {
        a[i, j] = lb2_1.Console.readInt("Enter a[" + i.ToString() + ", " + j.ToString() + "] (integer):", anyInt);
    }
}

Func<int, int, bool> ascending_order = (lhs, rhs) => { return lhs <= rhs; };

Console.WriteLine("Entered:");
writeArray(a, "a");

sortIntArrayByRows(a, (lhs, rhs) => { return lhs <= rhs; });
Console.WriteLine("Sorted by rows:");
writeArray(a, "a");

sortIntArrayByColumns(a, intLess);
Console.WriteLine("Sorted by columns:");
writeArray(a, "a");

sortIntArrayByRows_(a, ascending_order);
Console.WriteLine("Sorted by rows:");
writeArray(a, "a");

void writeArray(int[,] array, string array_name)
{
    Console.WriteLine($"{array_name}[{array.GetLength(0)}, {array.GetLength(1)}] {{");
    for (int i = 0; i < array.GetLength(0); ++i)
    {
        for (int j = 0; j < array.GetLength(1); ++j)
        {
            Console.Write($"  {array_name}[{i}, {j}] = {array[i, j]};");
        }
        Console.WriteLine();
    }
    Console.WriteLine("}");
}

void swapInt(ref int lhs, ref int rhs)
{
    int temp = lhs;
    lhs = rhs;
    rhs = temp;
}

void sortIntArrayByRows(int[,] array
                        , lb2_2.IntComparator compare
                        )
{
    int n_rows = array.GetLength(0);
    int n_columns = array.GetLength(1);

    int n_swaps = 1;
    while (n_swaps != 0)
    {
        n_swaps = 0;

        for (int i = 0; i < n_rows; ++i)
        {
            for (int j = 0; j < n_columns - 1; ++j)
            {
                if (!compare(array[i, j], array[i, j + 1]))
                {
                    swapInt(ref array[i, j], ref array[i, j + 1]);
                    ++n_swaps;
                }
            }
        }
    }
}

void sortIntArrayByRows_(int[,] array
                , Func<int, int, bool> compare
                )
{
    int n_rows = array.GetLength(0);
    int n_columns = array.GetLength(1);

    int n_swaps = 1;
    while (n_swaps != 0)
    {
        n_swaps = 0;

        for (int i = 0; i < n_rows; ++i)
        {
            for (int j = 0; j < n_columns - 1; ++j)
            {
                if (!compare(array[i, j], array[i, j + 1]))
                {
                    swapInt(ref array[i, j], ref array[i, j + 1]);
                    ++n_swaps;
                }
            }
        }
    }
}

void sortIntArrayByColumns(int[,] array, lb2_2.IntComparator compare)
{
    int n_rows = array.GetLength(0);
    int n_columns = array.GetLength(1);

    int n_swaps = 1;
    while (n_swaps != 0)
    {
        n_swaps = 0;

        for (int j = 0; j < n_columns; ++j)
        {
            for (int i = 0; i < n_rows - 1; ++i)
            {
                if (!compare(array[i, j], array[i + 1, j]))
                {
                    swapInt(ref array[i, j], ref array[i + 1, j]);
                    ++n_swaps;
                }
            }
        }
    }
}

bool intLess(int lhs, int rhs) { return lhs <= rhs; }

namespace lb2_2
{
    public delegate bool IntComparator(int lhs, int rhs);
}
