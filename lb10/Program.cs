/*

Задание:
Дан текстовый файл, содержащий данные о продуктах на складе и их 
описания, например:
3кг Апельсины
10л Квас
100л Вода
3780г Шоколад
10т Бананы
13кг Мангал
1. Определите класс с тремя закрытыми полями:
- Количество в кг (вещественное число);
- Исходное представление количества (строка);
- Название (строка).
2. Реализуйте конструктор, принимающий на вход 
два строковых значения: количество и название товара. Конструктор должен 
генерировать исключение, если количество является некорректным (меньше 
нуля). Добавьте свойства для преобразования количества в кг. В основной 
программе загрузите все температурные данные из исходного файла в 
список List< > .
3. Попытайтесь вызвать метод Sort для загруженного ранее списка 
температур. Возникающее при этом исключение свидетельствует о 
невозможности выполнять сравнение объектов произвольного класса. 
Чтобы это стало возможным, необходимо, например, реализовать в 
классе интерфейс IComparable<T> . Для этого:
o измените заголовок класса на следующий
class ‘Название’: IComparable<’Название’> o Необходимости реализовать метод сравнения. Метод сравнения 
должен возвращать отрицательное число, если объект, для 
которого вызывается метод, меньше объекта, переданного в 
качестве параметра, 0 — если оба объекта равны, и 
положительное число — если исходный объект больше —
реализуйте этот метод;
4. Убедитесь, что метод Sort работает и сортирует список.

*/

Console.WriteLine("App: Sort products from file");

if (args.Length < 1)
{
    throw new Exception("First parameter must be path to DB.");
}

var db_strings = File.ReadAllLines(args[0]);
if (db_strings == null)
{
    throw new Exception("Was not able to load DB.");
}

List<lb10.Product> products = new List<lb10.Product>();
foreach (var db_string in db_strings)
{
    var words = db_string.Trim().Split(' ');
    if (words.Length < 2)
    {
        Console.WriteLine($"DB string was skiped: {db_string}");
        Console.WriteLine("DB item must contain count and title");
        continue;
    }

    try
    {
        products.Add(new lb10.Product(words[0], words[1]));
    }
    catch (Exception e)
    {
        Console.WriteLine($"DB item was skiped: {db_string}");
        Console.WriteLine($"Error message: {e.Message}");
    }
}

Console.WriteLine("Loaded DB:");
Console.WriteLine(string.Join(", ", products));

products.Sort();

Console.WriteLine("Sorted DB:");
Console.WriteLine(string.Join(", ", products));

namespace lb10
{
    class Product : IComparable<Product>
    {
        public double kg { get { return _kg; } }

        public Product(string count, string title)
        {
            _count = count;
            _title = title;

            var cnt = count.Trim();
            double count_number = _parseCount(cnt);
            if (count_number < 0)
            {
                throw new ArgumentException($"Product count is less than zero. Processed number: '{count_number}'. Actual count: '{cnt}'");
            }

            var count_dimension_str = string.Concat(cnt.SkipWhile(ch => char.IsDigit(ch))).Trim();
            switch (count_dimension_str)
            {
                case "г": _kg = count_number / 1000.0; break;
                case "кг": _kg = count_number; break;
                case "л": _kg = count_number; break;
                case "т": _kg = 1000.0 * count_number; break;
                default: throw new ArgumentException($"Unknown mass dimension: '{count_dimension_str}'");
            }
        }

        public override string ToString()
        {
            return $"{{{_count}, {_title}}}";
        }

        int IComparable<Product>.CompareTo(Product? other)
        {
            if (other == null)
            {
                throw new ArgumentException("Other is null");
            }
            return _kg.CompareTo(other._kg);
        }

        double _kg;
        string _count;
        string _title;

        double _parseCount(string count)
        {
            var count_number_str = string.Concat(count.TakeWhile(ch => char.IsDigit(ch)));
            try
            {
                return double.Parse(count_number_str);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Product count is not real number. Processed string: '{count_number_str}'. Actual count: '{count}'\n{e}");
            }
        }
    }
}
