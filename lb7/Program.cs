/*

Задание: 
Разработать программу, реализующую работу с рефлексией.
1. Программа должна быть разработана в виде консольного приложения 
на языке C#.
2. Создайте класс, содержащий конструкторы, свойства, методы.
3. С использованием рефлексии выведите информацию о конструкторах, 
свойствах, методах.
4. Создайте класс атрибута (унаследован от класса System.Attribute).
5. Назначьте атрибут некоторым свойствам классам. Выведите только те 
свойства, которым назначен атрибут.
6. Вызовите один из методов класса с использованием рефлексии

*/

var product_type = typeof(lb7.Product);

Console.WriteLine("Product info:");

Console.WriteLine("\nConstructors:");
foreach (var constructor in product_type.GetConstructors())
{
    Console.WriteLine(constructor);
}

Console.WriteLine("\nProperties:");
foreach (var propertie in product_type.GetProperties())
{
    Console.WriteLine(propertie);
}

Console.WriteLine("\nMethods:");
foreach (var method in product_type.GetMethods())
{
    Console.WriteLine(method);
}

Console.WriteLine("\nProperties with attribute:");
foreach (var property in product_type.GetProperties())
{
    var attribute = Array.Find(property.GetCustomAttributes(false)
                                , p => p.GetType() == typeof(lb7.DecimalBounds)
                                );
    if (attribute != null)
    {
        Console.WriteLine($"Property: {property}");
        Console.WriteLine($"  Attribute: {attribute}");
    }
}

var product = new lb7.Product("lb7 product", 100);
Console.WriteLine($"\nBefore: {product}");

var product_method_name = nameof(lb7.Product.doublePrice);
var product_method = product_type.GetMethod(product_method_name);
if (product_method == null)
{
    throw new Exception($"Method {product_method_name} not found");
}

Console.WriteLine($"Call method {product_method}");
product_method.Invoke(product, null);

Console.WriteLine($"\nAfter: {product}");

namespace lb7
{
    class DecimalBounds : Attribute
    {
        public decimal min { get; set; }
        public decimal max { get; set; }

        public DecimalBounds(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    class Product
    {
        public string title
        { get; private set; }

        [DecimalBounds(0, 1000)]
        public decimal price
        { get; private set; }

        public Product(string title) : this(title, 0)
        { }

        public Product(string title, decimal price)
        {
            this.title = title;
            this.price = price;
        }

        public void doublePrice()
        {
            price *= 2;
        }

        public override string ToString()
        {
            return $"{title} : {price}";
        }
    }
}
